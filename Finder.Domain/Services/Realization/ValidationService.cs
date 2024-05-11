using EntityFrameworkCore.RepositoryInfrastructure;
using Finder.Data.Entities;
using Finder.Domain.Extensions;
using Finder.Domain.Helpers;
using Finder.Domain.Models.Update;
using Finder.Domain.Services.Abstraction;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Finder.Domain.Services.Realization;

internal class ValidationService(
    IServiceProvider services
) : IValidationService
{
    public Task<bool> IsUserExistAsync(
        string email,
        CancellationToken cancellationToken = default
    ) => GetRepository<User>()
        .Query()
        .AnyAsync(user => user.Email == email, cancellationToken);

    public Task<bool> IsExistAsync<T>(
        Guid id,
        CancellationToken cancellationToken = default
    ) where T : EntityBase, IEntity => GetRepository<T>()
        .Query()
        .AnyAsync(entity => entity.Id == id && entity.DeletedAt == null, cancellationToken);

    public Task<bool> IsExistAsync<T>(
        Guid? id,
        CancellationToken cancellationToken = default
    ) where T : EntityBase, IEntity => GetRepository<T>()
        .Query()
        .AnyAsync(entity => entity.Id == id && entity.DeletedAt == null, cancellationToken);

    public async Task<bool> AreAllExistAsync<T>(
        IEnumerable<Guid> ids,
        CancellationToken cancellationToken = default
    ) where T : EntityBase, IEntity => await GetRepository<T>()
            .Query()
            .Where(entity => ids.Contains(entity.Id) && entity.DeletedAt == null)
            .CountAsync(cancellationToken)
        == ids.Count();

    public Task<bool> IsUserWithVerificationCodeExistAsync(
        Guid verificationCode,
        CancellationToken cancellationToken = default
    ) => GetRepository<User>()
        .Query()
        .AnyAsync(user => user.VerificationCode == verificationCode, cancellationToken);

    public async Task<bool> CanRoleNameBeChangedAsync(
        UpdateRoleModel updateRoleModel,
        string name,
        CancellationToken cancellationToken = default
    ) => await GetRepository<Role>()
            .Query()
            .AnyAsync(vendor => vendor.Id == updateRoleModel.Id && vendor.Name == name, cancellationToken)
        || !await IsRoleExistAsync(name, cancellationToken);

    public Task<bool> IsPendingUserExistAsync(
        Guid registrationToken,
        CancellationToken cancellationToken = default
    ) => GetRepository<User>()
        .Query()
        .AnyAsync(user => user.RegistrationToken == registrationToken, cancellationToken);

    public Task<bool> IsCurrentUserPasswordCorrectAsync(
        string password,
        CancellationToken cancellationToken = default
    ) => GetRepository<User>()
        .Query()
        .AnyAsync(
            user => user.PasswordHash == PasswordHasher.GetHash(password)
                && user.Id
                == services
                    .GetRequiredService<IHttpContextAccessor>()
                    .GetCurrentUserId(),
            cancellationToken
        );

    public async Task<bool> IsEmailUniqueAsync(
        string email,
        CancellationToken cancellationToken = default
    ) => !await GetRepository<User>()
        .Query()
        .AnyAsync(user => user.Email == email, cancellationToken);

    public Task<bool> IsRoleActiveAsync(
        Guid roleId,
        CancellationToken cancellationToken = default
    ) => GetRepository<Role>()
        .Query()
        .AnyAsync(role => role.Id == roleId && role.DeletedAt == null, cancellationToken);

    public Task<bool> IsRoleActiveAsync(
        Guid? roleId,
        CancellationToken cancellationToken = default
    ) => GetRepository<Role>()
        .Query()
        .AnyAsync(role => role.Id == roleId && role.DeletedAt == null, cancellationToken);

    public Task<bool> IsRoleExistAsync(
        string name,
        CancellationToken cancellationToken = default
    ) => GetRepository<Role>()
        .Query()
        .AnyAsync(role => role.Name == name, cancellationToken);

    public Task<bool> CanUserRoleBeChanged(
        Guid id,
        CancellationToken cancellationToken = default
    ) => GetRepository<User>()
        .Query()
        .AnyAsync(user => user.Id == id && !user.Role!.IsHidden, cancellationToken);

    private IRepository<T> GetRepository<T>() where T : class, IEntity
        => services
            .GetRequiredService<IRepository<T>>();
}