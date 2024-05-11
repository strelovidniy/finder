using Finder.Data.Entities;
using Finder.Data.Enums.RichEnums;

namespace Finder.Domain.Services.Abstraction;

internal interface INotificationService
{
    public Task SendNotificationAsync(
        IEnumerable<PushSubscription> recipients,
        NotificationTitle title,
        NotificationContent content,
        string pageUrl,
        CancellationToken cancellationToken = default
    );
}