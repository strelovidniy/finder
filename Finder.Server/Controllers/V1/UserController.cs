using Finder.Domain.Attributes;
using Finder.Domain.Models;
using Finder.Domain.Models.Change;
using Finder.Domain.Models.Create;
using Finder.Domain.Models.Set;
using Finder.Domain.Models.Update;
using Finder.Domain.Services.Abstraction;
using Finder.Server.Controllers.Base;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Finder.Server.Controllers.V1;

[RouteV1("users")]
public class UserController(
    IServiceProvider services,
    IUserService userService,
    IUserAccessService userAccessService
) : BaseController(services)
{
    [AllowAnonymous]
    [HttpGet("refresh-token")]
    public async Task<IActionResult> LoginAsync(
        [FromQuery] Guid refreshToken,
        CancellationToken cancellationToken = default
    ) => Ok(
        await userService.RefreshTokenAsync(
            refreshToken,
            cancellationToken
        )
    );

    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<IActionResult> LoginAsync(
        [FromBody] LoginModel loginModel,
        CancellationToken cancellationToken = default
    )
    {
        await ValidateAsync(loginModel, cancellationToken);

        return Ok(await userService.LoginAsync(loginModel, cancellationToken));
    }

    [AllowAnonymous]
    [HttpPost("reset-password")]
    public async Task<IActionResult> ResetPasswordAsync(
        [FromBody] ResetPasswordModel resetPasswordModel,
        CancellationToken cancellationToken = default
    )
    {
        await ValidateAsync(resetPasswordModel, cancellationToken);

        await userService.ResetPasswordAsync(resetPasswordModel, cancellationToken);

        return Ok();
    }

    [AllowAnonymous]
    [HttpPost("set-new-password")]
    public async Task<IActionResult> SetNewPasswordAsync(
        [FromBody] SetNewPasswordModel setNewPasswordModel,
        CancellationToken cancellationToken = default
    )
    {
        await ValidateAsync(setNewPasswordModel, cancellationToken);

        await userService.SetNewPasswordAsync(setNewPasswordModel, cancellationToken);

        return Ok();
    }

    [HttpPost("change-password")]
    public async Task<IActionResult> ChangePasswordAsync(
        [FromBody] ChangePasswordModel changePasswordModel,
        CancellationToken cancellationToken = default
    )
    {
        await ValidateAsync(changePasswordModel, cancellationToken);

        await userService.ChangePasswordAsync(changePasswordModel, cancellationToken);

        return Ok();
    }

    [AllowAnonymous]
    [HttpPost("sign-up")]
    public async Task<IActionResult> SignUpAsync(
        [FromBody] RegisterUserModel registerUserModel,
        CancellationToken cancellationToken = default
    )
    {
        await ValidateAsync(registerUserModel, cancellationToken);

        await userService.RegisterUserAsync(registerUserModel, cancellationToken);

        return Ok();
    }

    [AllowAnonymous]
    [HttpPost("confirm-email")]
    public async Task<IActionResult> ConfirmEmailAsync(
        [FromBody] CreateUserModel createUserModel,
        CancellationToken cancellationToken = default
    )
    {
        await ValidateAsync(createUserModel, cancellationToken);

        await userService.CreateUserAsync(createUserModel, cancellationToken);

        return Ok();
    }

    [HttpPut("update")]
    public async Task<IActionResult> UpdateUserAsync(
        [FromBody] UpdateUserModel updateUserModel,
        CancellationToken cancellationToken = default
    )
    {
        if (updateUserModel.Id != CurrentUserId)
        {
            await userAccessService.CheckIfUserCanEditUsersAsync(cancellationToken);
        }

        await ValidateAsync(updateUserModel, cancellationToken);

        await userService.UpdateUserAsync(updateUserModel, cancellationToken);

        return Ok();
    }

    [HttpGet("me")]
    public async Task<IActionResult> GetMeAsync(
        CancellationToken cancellationToken = default
    ) => Ok(
        await userService.GetUserViewAsync(
            cancellationToken
        )
    );

    [HttpGet]
    public async Task<IActionResult> GetUsersAsync(
        [FromQuery] QueryParametersModel queryParametersModel,
        CancellationToken cancellationToken = default
    )
    {
        await userAccessService.CheckIfUserCanSeeUsersAsync(cancellationToken);

        return Ok(await userService.GetAllUsersAsync(queryParametersModel, cancellationToken));
    }

    [HttpGet("get-user")]
    public async Task<IActionResult> GetUserAsync(
        [FromQuery] Guid id,
        CancellationToken cancellationToken = default
    ) => Ok(await userService.GetUserByUserIdAsync(id, cancellationToken));


    [HttpDelete("delete")]
    public async Task<IActionResult> DeleteUserAsync(
        [FromQuery] Guid id,
        CancellationToken cancellationToken = default
    )
    {
        await userAccessService.CheckIfUserCanDeleteUsersAsync(cancellationToken);

        await userService.DeleteUserAsync(id, cancellationToken);

        return Ok();
    }

    [HttpGet("restore")]
    public async Task<IActionResult> RestoreUserAsync(
        [FromQuery] Guid id,
        CancellationToken cancellationToken = default
    )
    {
        await userAccessService.CheckIfUserCanRestoreUsersAsync(cancellationToken);

        await userService.RestoreUserAsync(id, cancellationToken);

        return Ok();
    }

    [AllowAnonymous]
    [HttpPost("complete-registration")]
    public async Task<IActionResult> CompleteRegistrationAsync(
        [FromBody] CompleteRegistrationModel completeRegistrationModel,
        CancellationToken cancellationToken = default
    )
    {
        await ValidateAsync(completeRegistrationModel, cancellationToken);

        await userService.CompleteRegistrationAsync(completeRegistrationModel, cancellationToken);

        return Ok();
    }

    [HttpPost("add-push-subscription")]
    public async Task<IActionResult> AddPushSubscriptionAsync(
        [FromBody] CreatePushSubscriptionModel model,
        CancellationToken cancellationToken = default
    )
    {
        await ValidateAsync(model, cancellationToken);

        await userService.AddPushSubscriptionAsync(
            model,
            cancellationToken
        );

        return Ok();
    }
}