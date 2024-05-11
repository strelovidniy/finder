using Finder.Data.Entities;
using Finder.Domain.Models.Create;

namespace Finder.Domain.Services.Abstraction;

internal interface IPushSubscriptionService
{
    public Task AddPushSubscriptionAsync(
        CreatePushSubscriptionModel createPushSubscriptionModel,
        CancellationToken cancellationToken = default
    );

    public Task DeletePushSubscriptionAsync(
        PushSubscription pushSubscription,
        CancellationToken cancellationToken
    );
}