using Finder.Data.Entities;
using Finder.Data.Enums;
using Finder.Domain.Models.Update;
using Finder.Domain.Services.Abstraction;
using Finder.Domain.Validators.Extensions;
using FluentValidation;

namespace Finder.Domain.Validators;

internal class UpdateUserModelValidator : AbstractValidator<UpdateUserModel>
{
    public UpdateUserModelValidator(IValidationService validationService)
    {
        RuleFor(updateUserModel => updateUserModel.Id)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithStatusCode(StatusCode.UserIdRequired)
            .MustAsync(validationService.IsExistAsync<User>)
            .WithStatusCode(StatusCode.UserNotFound);

        When(updateUserModel => updateUserModel.FirstName is not null, () =>
        {
            RuleFor(updateUserModel => updateUserModel.FirstName)
                .Cascade(CascadeMode.Stop)
                .NotEmpty()
                .WithStatusCode(StatusCode.FirstNameRequired)
                .Must(firstName => !firstName!.Contains(" "))
                .WithStatusCode(StatusCode.FirstNameShouldNotContainWhiteSpace);
        });

        When(updateUserModel => updateUserModel.LastName is not null, () =>
        {
            RuleFor(updateUserModel => updateUserModel.LastName)
                .Cascade(CascadeMode.Stop)
                .NotEmpty()
                .WithStatusCode(StatusCode.LastNameRequired)
                .Must(lastName => !lastName!.Contains(" "))
                .WithStatusCode(StatusCode.LastNameShouldNotContainWhiteSpace);
        });
    }
}