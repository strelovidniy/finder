using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Web;
using AutoMapper;
using EntityFrameworkCore.RepositoryInfrastructure;
using Finder.Data.Entities;
using Finder.Data.Enums;
using Finder.Data.Enums.RichEnums;
using Finder.Domain.Exceptions;
using Finder.Domain.Extensions;
using Finder.Domain.Helpers;
using Finder.Domain.Models;
using Finder.Domain.Models.Change;
using Finder.Domain.Models.Create;
using Finder.Domain.Models.Set;
using Finder.Domain.Models.Update;
using Finder.Domain.Models.ViewModels;
using Finder.Domain.Models.Views;
using Finder.Domain.Services.Abstraction;
using Finder.Domain.Settings.Abstraction;
using Finder.Domain.Validators.Runtime;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;

namespace Finder.Domain.Services.Realization;

internal class UserService(
    IJwtSettings jwtSettings,
    IRepository<User> userRepository,
    IUrlSettings urlSettings,
    IEmailService emailService,
    IHttpContextAccessor httpContextAccessor,
    IMapper mapper,
    ILogger<UserService> logger,
    IRoleService roleService,
    IPushSubscriptionService pushSubscriptionService
) : IUserService
{
    public async Task DeleteUserAsync(
        Guid userId,
        CancellationToken cancellationToken = default
    )
    {
        var userToDelete = await GetUserByUserIdAsync(userId, cancellationToken);

        if (userToDelete is not null)
        {
            userToDelete.DeletedAt = DateTime.UtcNow;
            userToDelete.Status = UserStatus.Deleted;
            await userRepository.SaveChangesAsync(cancellationToken);
        }
    }

    public async Task RestoreUserAsync(
        Guid userId,
        CancellationToken cancellationToken = default
    )
    {
        var userToDelete = await GetUserByUserIdAsync(userId, cancellationToken);

        if (userToDelete is not null)
        {
            userToDelete.DeletedAt = null;
            userToDelete.Status = UserStatus.Active;
            await userRepository.SaveChangesAsync(cancellationToken);
        }
    }

    public async Task CompleteRegistrationAsync(
        CompleteRegistrationModel completeRegistrationModel,
        CancellationToken cancellationToken = default
    )
    {
        var user = await userRepository
            .Query()
            .FirstOrDefaultAsync(
                user => user.RegistrationToken == completeRegistrationModel.RegistrationToken
                    && user.Status != UserStatus.Deleted,
                cancellationToken
            );

        RuntimeValidator.Assert(user is not null, StatusCode.InvalidVerificationCode);

        user!.RegistrationToken = null;
        user.PasswordHash = PasswordHasher.GetHash(completeRegistrationModel.Password);
        user.FirstName = completeRegistrationModel.FirstName;
        user.LastName = completeRegistrationModel.LastName;
        user.Status = UserStatus.Active;
        user.UpdatedAt = DateTime.UtcNow;

        await userRepository.SaveChangesAsync(cancellationToken);
    }

    public Task AddPushSubscriptionAsync(
        CreatePushSubscriptionModel createPushSubscriptionModel,
        CancellationToken cancellationToken = default
    ) => pushSubscriptionService.AddPushSubscriptionAsync(createPushSubscriptionModel, cancellationToken);

    public async Task UpdateProfileAsync(
        UpdateProfileModel model,
        CancellationToken cancellationToken
    )
    {
        var user = await userRepository
            .Query()
            .Include(user => user.Role)
            .FirstOrDefaultAsync(u => u.Id == model.Id, cancellationToken);

        RuntimeValidator.Assert(user is not null, StatusCode.UserNotFound);

        if (model.FirstName is not null)
        {
            user!.FirstName = model.FirstName;
        }

        if (model.LastName is not null)
        {
            user!.LastName = model.LastName;
        }

        if (model.Email is not null)
        {
            user!.Email = model.Email;
        }

        if (model.FirstName is not null)
        {
            user!.FirstName = model.FirstName;
        }

        if (model.LastName is not null)
        {
            user!.LastName = model.LastName;
        }

        if (model.Email is not null)
        {
            user!.Email = model.Email;
        }

        user!.UpdatedAt = DateTime.UtcNow;

        await userRepository.SaveChangesAsync(cancellationToken);
    }

    public Task<User?> GetUserAsync(
        CancellationToken cancellationToken = default
    ) => GetFullUserQueryable()
        .FirstOrDefaultAsync(
            u => u.Id == httpContextAccessor.GetCurrentUserId(),
            cancellationToken
        );

    public async Task<UserView?> GetUserViewAsync(
        CancellationToken cancellationToken = default
    )
    {
        var currentUser = await GetUserAsync(cancellationToken);

        RuntimeValidator.Assert(currentUser is not null, StatusCode.Unauthorized);

        return mapper.Map<UserView>(currentUser);
    }

    /// <summary>
    ///     Gets User's General Info.
    /// </summary>
    public Task<User?> GetUserByUserIdAsync(
        Guid userId,
        CancellationToken cancellationToken = default
    ) => GetFullUserQueryable()
        .FirstOrDefaultAsync(
            u => u.Id == userId,
            cancellationToken
        );

    public async Task<AuthToken> RefreshTokenAsync(
        Guid refreshToken,
        CancellationToken cancellationToken = default
    )
    {
        var user = await userRepository
            .Query()
            .FirstOrDefaultAsync(
                user => user.RefreshToken == refreshToken.ToString(),
                cancellationToken
            );

        RuntimeValidator.Assert(user is not null, StatusCode.Unauthorized);
        RuntimeValidator.Assert(user!.RefreshTokenExpiresAt > DateTime.UtcNow, StatusCode.Unauthorized);

        return GenerateToken(user);
    }

    public async Task<LoginView> LoginAsync(
        LoginModel loginModel,
        CancellationToken cancellationToken = default
    )
    {
        var user = await GetUserAsync(loginModel.Email, loginModel.Password, cancellationToken);

        RuntimeValidator.Assert(user is not null, StatusCode.IncorrectPassword);

        var refreshToken = Guid.NewGuid();
        var refreshTokenExpireAt = DateTime.UtcNow.AddSeconds(jwtSettings.SecondsToExpireRefreshToken);

        if (loginModel.RememberMe)
        {
            user!.RefreshToken = refreshToken.ToString();
            user.RefreshTokenExpiresAt = refreshTokenExpireAt;
        }
        else
        {
            user!.RefreshToken = null;
            user.RefreshTokenExpiresAt = null;
        }

        await userRepository.SaveChangesAsync(cancellationToken);

        var authTokenModel = GenerateToken(user);

        return new LoginView(mapper.Map<UserView>(user), authTokenModel);
    }

    public async Task RegisterUserAsync(
        RegisterUserModel registerUserModel,
        CancellationToken cancellationToken = default
    )
    {
        var addedUser = await userRepository.AddAsync(
            new User
            {
                Email = registerUserModel.Email,
                Status = UserStatus.Pending,
                PasswordHash = PasswordHasher.GetHash(registerUserModel.Password),
                RegistrationToken = Guid.NewGuid(),
                FirstName = registerUserModel.FirstName,
                LastName = registerUserModel.LastName
            },
            cancellationToken
        );

        await userRepository.SaveChangesAsync(cancellationToken);

        await SendRegistrationEmailAsync(addedUser, cancellationToken);
    }

    public async Task ResetPasswordAsync(
        ResetPasswordModel resetPasswordModel,
        CancellationToken cancellationToken = default
    )
    {
        var user = await userRepository
                .Query()
                .FirstOrDefaultAsync(user => user.Email == resetPasswordModel.Email, cancellationToken)
            ?? throw new ApiException(StatusCode.UserNotFound);

        user.VerificationCode = Guid.NewGuid();

        await userRepository.SaveChangesAsync(cancellationToken);

        var uriBuilder = new UriBuilder(urlSettings.ResetPasswordUrl);
        var query = HttpUtility.ParseQueryString(uriBuilder.Query);

        query["vc"] = user.VerificationCode.ToString();
        uriBuilder.Query = query.ToString();

        await emailService.SendEmailAsync(
            resetPasswordModel.Email,
            EmailSubject.ResetPassword,
            new ResetPasswordEmailViewModel(uriBuilder.ToString()),
            cancellationToken: cancellationToken
        );
    }

    public async Task SetNewPasswordAsync(
        SetNewPasswordModel setNewPasswordModel,
        CancellationToken cancellationToken = default
    )
    {
        var user = await userRepository
                .Query()
                .FirstOrDefaultAsync(
                    user => user.VerificationCode == setNewPasswordModel.VerificationCode,
                    cancellationToken
                )
            ?? throw new ApiException(StatusCode.UserNotFound);

        user.PasswordHash = PasswordHasher.GetHash(setNewPasswordModel.NewPassword);
        user.VerificationCode = null;

        await userRepository.SaveChangesAsync(cancellationToken);
    }

    public async Task CreateUserAsync(
        CreateUserModel createUserModel,
        CancellationToken cancellationToken = default
    )
    {
        var user = await userRepository
            .Query()
            .FirstOrDefaultAsync(
                user => user.RegistrationToken == createUserModel.RegistrationToken
                    && user.Status != UserStatus.Deleted,
                cancellationToken
            );

        RuntimeValidator.Assert(user is not null, StatusCode.InvalidVerificationCode);

        var role = await roleService.GetUserRoleAsync(cancellationToken);

        RuntimeValidator.Assert(role is not null, StatusCode.UserRoleNotFound);

        user!.RegistrationToken = null;
        user.Status = UserStatus.Active;
        user.UpdatedAt = DateTime.UtcNow;
        user.RoleId = role!.Id;

        await userRepository.SaveChangesAsync(cancellationToken);
    }

    public Task<PagedCollectionView<User>> GetAllUsersAsync(
        QueryParametersModel queryParametersModel,
        CancellationToken cancellationToken = default
    ) => GetPagedUsersAsync(
        userRepository
            .Query()
            .AsNoTracking()
            .Where(user => user.Status != UserStatus.Deleted),
        queryParametersModel,
        cancellationToken
    );

    public async Task<IEnumerable<User>> GetAllUsersAsync(
        CancellationToken cancellationToken = default
    ) => await GetFullUserQueryable()
        .AsNoTracking()
        .Where(user => user.Status != UserStatus.Deleted)
        .ToListAsync(cancellationToken);

    public async Task ChangePasswordAsync(
        ChangePasswordModel changePasswordModel,
        CancellationToken cancellationToken = default
    )
    {
        var currentUser = await userRepository
                .Query()
                .FirstOrDefaultAsync(user => user.Id == httpContextAccessor.GetCurrentUserId(), cancellationToken)
            ?? throw new ApiException(StatusCode.Unauthorized);

        RuntimeValidator.Assert(
            currentUser.PasswordHash == PasswordHasher.GetHash(changePasswordModel.OldPassword),
            StatusCode.OldPasswordIncorrect
        );

        RuntimeValidator.Assert(
            changePasswordModel.NewPassword == changePasswordModel.ConfirmNewPassword,
            StatusCode.PasswordsDoNotMatch
        );

        currentUser.PasswordHash = PasswordHasher.GetHash(changePasswordModel.NewPassword);

        await userRepository.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateUserAsync(
        UpdateUserModel model,
        CancellationToken cancellationToken
    )
    {
        var user = await userRepository
            .Query()
            .FirstOrDefaultAsync(u => u.Id == model.Id, cancellationToken);

        RuntimeValidator.Assert(user is not null, StatusCode.UserNotFound);

        if (model.FirstName is not null)
        {
            user!.FirstName = model.FirstName;
            user.UpdatedAt = DateTime.UtcNow;
        }

        if (model.LastName is not null)
        {
            user!.LastName = model.LastName;
            user.UpdatedAt = DateTime.UtcNow;
        }

        await userRepository.SaveChangesAsync(cancellationToken);
    }


    private Task<User?> GetUserAsync(
        string username,
        string password,
        CancellationToken cancellationToken = default
    ) => userRepository
        .Query()
        .FirstOrDefaultAsync(
            user => user.Email == username && user.PasswordHash == PasswordHasher.GetHash(password),
            cancellationToken
        );

    private IQueryable<User> GetFullUserQueryable() =>
        userRepository
            .Query()
            .Include(user => user.Role)
            .Include(user => user.Details)
            .ThenInclude(details => details!.Address)
            .Include(user => user.Details)
            .ThenInclude(details => details!.ContactInfo)
            .Include(user => user.NotificationSettings);

    private Task SendRegistrationEmailAsync(
        User registeredUser,
        CancellationToken cancellationToken = default
    )
    {
        var uriBuilder = new UriBuilder(urlSettings.ConfirmEmailUrl);
        var query = HttpUtility.ParseQueryString(uriBuilder.Query);

        query["vc"] = registeredUser.RegistrationToken.ToString();
        uriBuilder.Query = query.ToString();

        return emailService.SendEmailAsync(
            registeredUser.Email,
            EmailSubject.CreateAccount,
            new CreateAccountEmailViewModel(uriBuilder.ToString()),
            cancellationToken: cancellationToken
        );
    }

    private AuthToken GenerateToken(User user)
    {
        var tokenHandler = new JwtSecurityTokenHandler();

        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new(ClaimTypes.Role, user.Role?.Name ?? string.Empty)
        };

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddSeconds(jwtSettings.SecondsToExpireToken),
            Issuer = jwtSettings.Issuer,
            IssuedAt = DateTime.UtcNow,
            NotBefore = DateTime.UtcNow,
            SigningCredentials = new SigningCredentials(
                new SymmetricSecurityKey(jwtSettings.TokenSecretString.ToByteArray()),
                SecurityAlgorithms.HmacSha512Signature
            )
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);

        return new AuthToken
        {
            Token = tokenHandler.WriteToken(token),
            ExpireAt = token.ValidTo.ToUniversalTime().ToString("yyyy-MM-ddTHH:mm:ssZ"),
            RefreshToken = user.RefreshToken,
            RefreshTokenExpireAt = user.RefreshTokenExpiresAt?.ToString("yyyy-MM-ddTHH:mm:ssZ")
        };
    }

    private async Task<PagedCollectionView<User>> GetPagedUsersAsync(
        IQueryable<User> users,
        QueryParametersModel queryParametersModel,
        CancellationToken cancellationToken = default
    )
    {
        var currentUser = await GetUserAsync(cancellationToken);

        RuntimeValidator.Assert(currentUser is not null, StatusCode.Unauthorized);
        RuntimeValidator.Assert(currentUser?.Role?.Type is RoleType.Admin, StatusCode.Unauthorized);

        if (!string.IsNullOrWhiteSpace(queryParametersModel.SearchQuery))
        {
            queryParametersModel
                .SearchQuery
                .Split(' ', StringSplitOptions.RemoveEmptyEntries)
                .ToList()
                .ForEach(searchElement =>
                {
                    users = users
                        .Where(user =>
                            user.FirstName!.Contains(searchElement)
                            || user.LastName!.Contains(searchElement)
                            || user.Email.Contains(searchElement)
                            || user.Role!.Name.Contains(searchElement)
                            || ((string) (object) user.Status).Contains(searchElement));
                });
        }

        users = users.ExpandAndSort(queryParametersModel, logger);

        try
        {
            return new PagedCollectionView<User>(
                await users
                    .Skip(queryParametersModel.PageSize * (queryParametersModel.PageNumber - 1))
                    .Take(queryParametersModel.PageSize)
                    .ToListAsync(cancellationToken),
                await users.CountAsync(cancellationToken)
            );
        }
        catch (Exception e)
        {
            logger.LogError(e, ErrorMessage.UsersGettingError);

            throw new ApiException(StatusCode.QueryResultError);
        }
    }
}