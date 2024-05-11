using Finder.Data.Entities;
using Finder.Data.Enums;
using Finder.Domain.Models.Change;
using Finder.Domain.Services.Abstraction;
using Finder.Domain.Validators.Extensions;
using FluentValidation;

namespace Finder.Domain.Validators;

internal class ChangeUserRolesModelValidator : AbstractValidator<ChangeUserRoleModel>
{
    public ChangeUserRolesModelValidator(IValidationService validationService)
    {
        RuleFor(changeUserRolesModel => changeUserRolesModel.UserId)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithStatusCode(StatusCode.UserIdRequired)
            .MustAsync(validationService.IsExistAsync<User>)
            .WithStatusCode(StatusCode.UserNotFound)
            .MustAsync(validationService.CanUserRoleBeChanged)
            .WithStatusCode(StatusCode.UserRoleCannotBeChanged);

        RuleFor(changeUserRolesModel => changeUserRolesModel.RoleId)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithStatusCode(StatusCode.RoleIdRequired)
            .MustAsync(validationService.IsExistAsync<Role>)
            .WithStatusCode(StatusCode.RoleNotFound);
    }
}