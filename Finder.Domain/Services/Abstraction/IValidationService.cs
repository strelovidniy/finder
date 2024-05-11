using EntityFrameworkCore.RepositoryInfrastructure;
using Finder.Data.Entities;
using Finder.Domain.Models.Update;

namespace Finder.Domain.Services.Abstraction;

public interface IValidationService
{
    public Task<bool> IsUserExistAsync(
        string email,
        CancellationToken cancellationToken = default
    );

    public Task<bool> IsExistAsync<T>(
        Guid id,
        CancellationToken cancellationToken = default
    ) where T : EntityBase, IEntity;


    public Task<bool> IsExistAsync<T>(
        Guid? id,
        CancellationToken cancellationToken = default
    ) where T : EntityBase, IEntity;

    public Task<bool> AreAllExistAsync<T>(
        IEnumerable<Guid> ids,
        CancellationToken cancellationToken = default
    ) where T : EntityBase, IEntity;

    public Task<bool> IsUserWithVerificationCodeExistAsync(
        Guid verificationCode,
        CancellationToken cancellationToken = default
    );

    public Task<bool> CanRoleNameBeChangedAsync(
        UpdateRoleModel updateRoleModel,
        string name,
        CancellationToken cancellationToken = default
    );

    public Task<bool> IsPendingUserExistAsync(
        Guid registrationToken,
        CancellationToken cancellationToken = default
    );

    public Task<bool> IsCurrentUserPasswordCorrectAsync(
        string password,
        CancellationToken cancellationToken = default
    );

    public Task<bool> IsEmailUniqueAsync(
        string email,
        CancellationToken cancellationToken = default
    );

    public Task<bool> IsRoleActiveAsync(
        Guid roleId,
        CancellationToken cancellationToken = default
    );

    public Task<bool> IsRoleActiveAsync(
        Guid? roleId,
        CancellationToken cancellationToken = default
    );

    public Task<bool> IsRoleExistAsync(
        string name,
        CancellationToken cancellationToken = default
    );

    public Task<bool> CanUserRoleBeChanged(
        Guid id,
        CancellationToken cancellationToken = default
    );
}