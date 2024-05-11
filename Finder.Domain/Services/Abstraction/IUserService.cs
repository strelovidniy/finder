using Finder.Data.Entities;
using Finder.Domain.Models;
using Finder.Domain.Models.Change;
using Finder.Domain.Models.Create;
using Finder.Domain.Models.Set;
using Finder.Domain.Models.Update;
using Finder.Domain.Models.Views;

namespace Finder.Domain.Services.Abstraction;

public interface IUserService
{
    public Task<AuthToken> RefreshTokenAsync(
        Guid refreshToken,
        CancellationToken cancellationToken = default
    );

    public Task<LoginView> LoginAsync(
        LoginModel loginModel,
        CancellationToken cancellationToken = default
    );

    public Task ChangePasswordAsync(
        ChangePasswordModel changePasswordModel,
        CancellationToken cancellationToken = default
    );

    public Task ResetPasswordAsync(
        ResetPasswordModel resetPasswordModel,
        CancellationToken cancellationToken = default
    );

    public Task SetNewPasswordAsync(
        SetNewPasswordModel setNewPasswordModel,
        CancellationToken cancellationToken = default
    );

    public Task CreateUserAsync(
        CreateUserModel createUserModel,
        CancellationToken cancellationToken = default
    );

    public Task RegisterUserAsync(
        RegisterUserModel registerUserModel,
        CancellationToken cancellationToken = default
    );

    public Task UpdateUserAsync(
        UpdateUserModel model,
        CancellationToken cancellationToken
    );

    public Task UpdateProfileAsync(
        UpdateProfileModel model,
        CancellationToken cancellationToken
    );

    public Task<User?> GetUserAsync(
        CancellationToken cancellationToken = default
    );

    public Task<UserView?> GetUserViewAsync(
        CancellationToken cancellationToken = default
    );

    public Task<User?> GetUserByUserIdAsync(
        Guid userId,
        CancellationToken cancellationToken = default
    );

    public Task<PagedCollectionView<User>> GetAllUsersAsync(
        QueryParametersModel queryParametersModel,
        CancellationToken cancellationToken = default
    );

    public Task<IEnumerable<User>> GetAllUsersAsync(
        CancellationToken cancellationToken = default
    );

    public Task DeleteUserAsync(
        Guid userId,
        CancellationToken cancellationToken = default
    );

    public Task RestoreUserAsync(
        Guid userId,
        CancellationToken cancellationToken = default
    );

    public Task CompleteRegistrationAsync(
        CompleteRegistrationModel completeRegistrationModel,
        CancellationToken cancellationToken = default
    );

    public Task AddPushSubscriptionAsync(
        CreatePushSubscriptionModel createPushSubscriptionModel,
        CancellationToken cancellationToken = default
    );
}