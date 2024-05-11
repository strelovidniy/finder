namespace Finder.Domain.Services.Abstraction;

public interface IUserAccessService
{
    public Task CheckIfUserCanSeeUsersAsync(
        CancellationToken cancellationToken = default
    );

    public Task CheckIfUserCanDeleteUsersAsync(
        CancellationToken cancellationToken = default
    );

    public Task CheckIfUserCanRestoreUsersAsync(
        CancellationToken cancellationToken = default
    );

    public Task CheckIfUserCanEditUsersAsync(
        CancellationToken cancellationToken = default
    );

    public Task CheckIfUserCanCreateRolesAsync(
        CancellationToken cancellationToken = default
    );

    public Task CheckIfUserCanEditRolesAsync(
        CancellationToken cancellationToken = default
    );

    public Task CheckIfUserCanDeleteRolesAsync(
        CancellationToken cancellationToken = default
    );

    public Task CheckIfUserCanSeeRolesAsync(
        CancellationToken cancellationToken = default
    );

    public Task CheckIfUserCanCreateHelpRequestsAsync(
        CancellationToken cancellationToken = default
    );

    public Task CheckIfUserCanSeeHelpRequests(
        CancellationToken cancellationToken = default
    );
}