using EntityFrameworkCore.RepositoryInfrastructure;
using Finder.Data.Entities;
using Finder.Data.Enums.RichEnums;
using Finder.Domain.Services.Abstraction;
using Microsoft.EntityFrameworkCore;

namespace Finder.Domain.Services.Realization;

internal class SearchOperationNotificationService(
    IRepository<NotificationSettings> notificationSettingsRepository,
    INotificationService notificationService
) : ISearchOperationNotificationService
{
    public async Task NotifyAboutCreatingSearchOperationAsync(
        SearchOperation helpRequest,
        CancellationToken cancellationToken = default
    )
    {
        var helpRequestTitle = helpRequest.Title;
        var tags = helpRequest.Tags?.ToList();

        var query = notificationSettingsRepository
            .NoTrackingQuery()
            .Include(notificationSettings => notificationSettings.User)
            .ThenInclude(user => user!.PushSubscriptions)
            .Where(notificationSettings => notificationSettings.User != null
                                           && notificationSettings.User.PushSubscriptions != null
                                           && notificationSettings.User.PushSubscriptions.Any())
            .Where(notificationSettings => notificationSettings.EnableNotifications)
            .Where(notificationSettings => notificationSettings.FilterTitles == null
                                           || (notificationSettings.FilterTitles as object as string)!.Contains(
                                               helpRequestTitle));

        if (tags is not null && tags.Any())
        {
            query = query.Where(notificationSettings => notificationSettings.FilterTags == null
                                                        || tags.Any(tag =>
                                                            (notificationSettings.FilterTags as object as string)!
                                                            .Contains(tag)));
        }

        var pushSubscriptions = await query
            .SelectMany(notificationSettings => notificationSettings.User!.PushSubscriptions!)
            .ToListAsync(cancellationToken);

        if (!pushSubscriptions.Any())
        {
            return;
        }

        await notificationService.SendNotificationAsync(
            pushSubscriptions,
            NotificationTitle.NewHelpRequest,
            NotificationContent.NewHelpRequest(helpRequest.Title),
            $"/#/requests/details?id={helpRequest.Id}",
            cancellationToken
        );
    }

    public async Task NotifyAboutUpdatingSearchOperationAsync(SearchOperation helpRequest,
        CancellationToken cancellationToken = default
    )
    {
        var helpRequestTitle = helpRequest.Title;
        var helpRequestTags = helpRequest.Tags?.ToList();

        var query = notificationSettingsRepository
            .NoTrackingQuery()
            .Include(notificationSettings => notificationSettings.User)
            .ThenInclude(user => user!.PushSubscriptions)
            .Where(notificationSettings => notificationSettings.User != null
                                           && notificationSettings.User.PushSubscriptions != null
                                           && notificationSettings.User.PushSubscriptions.Any())
            .Where(notificationSettings => notificationSettings.EnableUpdateNotifications
                                           && notificationSettings.EnableNotifications)
            .Where(notificationSettings => notificationSettings.FilterTitles == null
                                           || (notificationSettings.FilterTitles as object as string)!.Contains(
                                               helpRequestTitle));

        var isNotified = false;
        ///

        if (helpRequestTags is not null && helpRequestTags.Any())
        {
            query = query.Where(notificationSettings => notificationSettings.FilterTags == null
                                                        || helpRequestTags.Any(tag =>
                                                            (notificationSettings.FilterTags as object as string)!
                                                            .Contains(tag)));
        }

        var pushSubscriptions = await query
            .SelectMany(notificationSettings => notificationSettings.User!.PushSubscriptions!)
            .ToListAsync(cancellationToken);

        if (!pushSubscriptions.Any())
        {
            return;
        }


        await notificationService.SendNotificationAsync(
            pushSubscriptions,
            NotificationTitle.HelpRequestUpdated,
            NotificationContent.HelpRequestUpdated(helpRequest.Title),
            $"/#/requests/details?id={helpRequest.Id}",
            cancellationToken
        );
    }
}