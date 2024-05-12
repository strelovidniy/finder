using Finder.Data.Entities;

namespace Finder.Domain.Services.Abstraction;

internal interface ISearchOperationNotificationService
{
    public Task NotifyAboutUpdatingSearchOperationAsync(
        SearchOperation helpRequest,
        CancellationToken cancellationToken = default
    );

    public Task NotifyAboutCreatingSearchOperationAsync(
        SearchOperation helpRequest,
        CancellationToken cancellationToken = default
    );

    Task NotifyAboutApplicationReceivedAsync(
        SearchOperation operation,
        User currentUser,
        CancellationToken cancellationToken
    );
}