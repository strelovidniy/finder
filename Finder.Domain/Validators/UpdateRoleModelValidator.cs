using Finder.Data.Entities;
using Finder.Data.Enums;
using Finder.Domain.Models.Update;
using Finder.Domain.Services.Abstraction;
using Finder.Domain.Validators.Extensions;
using FluentValidation;

namespace Finder.Domain.Validators;

internal class UpdateRoleModelValidator : AbstractValidator<UpdateRoleModel>
{
    public UpdateRoleModelValidator(IValidationService validationService)
    {
        RuleFor(updateRoleModel => updateRoleModel.Id)
            .Cascade(CascadeMode.Stop)
            .MustAsync(validationService.IsExistAsync<Role>)
            .WithStatusCode(StatusCode.RoleNotFound);

        RuleFor(updateRoleModel => updateRoleModel.Name)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithStatusCode(StatusCode.RoleNameRequired)
            .MustAsync(validationService.CanRoleNameBeChangedAsync)
            .WithStatusCode(StatusCode.RoleAlreadyExists);

        When(updateRoleModel => updateRoleModel.Type is not null, () =>
        {
            RuleFor(updateRoleModel => updateRoleModel.Type)
                .Cascade(CascadeMode.Stop)
                .IsInEnum()
                .WithStatusCode(StatusCode.InvalidRoleType);
        });
    }
}