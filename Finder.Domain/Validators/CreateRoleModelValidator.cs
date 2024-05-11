using Finder.Data.Enums;
using Finder.Domain.Models.Create;
using Finder.Domain.Services.Abstraction;
using Finder.Domain.Validators.Extensions;
using FluentValidation;

namespace Finder.Domain.Validators;

internal class CreateRoleModelValidator : AbstractValidator<CreateRoleModel>
{
    public CreateRoleModelValidator(IValidationService validationService)
    {
        RuleFor(createRoleModel => createRoleModel.Name)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithStatusCode(StatusCode.RoleNameRequired)
            .MustAsync(async (name, cancellationToken) =>
                !await validationService.IsRoleExistAsync(name, cancellationToken))
            .WithStatusCode(StatusCode.RoleAlreadyExists);

        RuleFor(createRoleModel => createRoleModel.Type)
            .Cascade(CascadeMode.Stop)
            .NotNull()
            .WithStatusCode(StatusCode.RoleTypeRequired)
            .IsInEnum()
            .WithStatusCode(StatusCode.InvalidRoleType);
    }
}