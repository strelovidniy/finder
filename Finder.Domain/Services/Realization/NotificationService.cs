using System.Net;
using Finder.Data.Enums.RichEnums;
using Finder.Domain.Services.Abstraction;
using Finder.Domain.Settings.Abstraction;
using Microsoft.Extensions.Logging;
using WebPush;
using PushSubscription = Finder.Data.Entities.PushSubscription;

namespace Finder.Domain.Services.Realization;

internal class NotificationService(
    IWebPushSettings webPushSettings,
    IUrlSettings urlSettings,
    IPushSubscriptionService pushSubscriptionService,
    ILogger<NotificationService> logger
) : INotificationService
{
    public async Task SendNotificationAsync(
        IEnumerable<PushSubscription> recipients,
        NotificationTitle title,
        NotificationContent content,
        string pageUrl,
        CancellationToken cancellationToken = default
    )
    {
        var client = new WebPushClient();

        string stringContent = content;
        string stringTitle = title;

        stringContent = stringContent.Replace("\"", "\\\"");
        stringTitle = stringTitle.Replace("\"", "\\\"");

        var jsonBody
            = $"{{\"notification\":{{\"data\":{{\"onActionClick\":{{\"default\":{{\"operation\":\"openWindow\",\"url\":\"${pageUrl}\"}}}}}},\"body\":\"{stringContent}\",\"title\":\"{stringTitle}\",\"icon\":\"{urlSettings.WebApiUrl.TrimEnd('/')}/api/v1/static-files/icon\",\"vibrate\":[100]}}}}";

        var pushSubscriptionsToDelete = new List<PushSubscription>();

        await Task.WhenAll(
            recipients.Select(async recipient =>
            {
                try
                {
                    await client.SendNotificationAsync(
                        new WebPush.PushSubscription(
                            recipient.Endpoint,
                            recipient.P256dh,
                            recipient.Auth
                        ),
                        jsonBody,
                        new VapidDetails(
                            urlSettings.AppUrl,
                            webPushSettings.PublicKey,
                            webPushSettings.PrivateKey
                        ),
                        cancellationToken
                    );
                }
                catch (WebPushException exception)
                {
                    logger.LogError(exception, ErrorMessage.NotificationSendingError);

                    if (exception.StatusCode is HttpStatusCode.Gone)
                    {
                        pushSubscriptionsToDelete.Add(new PushSubscription
                        {
                            Auth = exception.PushSubscription.Auth,
                            Endpoint = exception.PushSubscription.Endpoint,
                            P256dh = exception.PushSubscription.P256DH
                        });
                    }
                }
            })
        );

        if (pushSubscriptionsToDelete.Any())
        {
            foreach (var pushSubscription in pushSubscriptionsToDelete)
            {
                await pushSubscriptionService.DeletePushSubscriptionAsync(
                    pushSubscription,
                    cancellationToken
                );
            }
        }
    }
}