using Finder.Data.Entities;
using Finder.Data.Enums;
using Finder.Domain.Models.Update;
using Finder.Domain.Services.Abstraction;
using Finder.Domain.Validators.Extensions;
using FluentValidation;

namespace Finder.Domain.Validators;

internal class UpdateProfileModelValidator : AbstractValidator<UpdateProfileModel>
{
    public UpdateProfileModelValidator(IValidationService validationService)
    {
        RuleFor(updateProfileModel => updateProfileModel.Id)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithStatusCode(StatusCode.UserIdRequired)
            .MustAsync(validationService.IsExistAsync<User>)
            .WithStatusCode(StatusCode.UserNotFound);

        RuleFor(updateProfileModel => updateProfileModel.FirstName)
            .Cascade(CascadeMode.Stop)
            .MaximumLength(50)
            .WithStatusCode(StatusCode.FirstNameTooLong);

        RuleFor(updateProfileModel => updateProfileModel.LastName)
            .Cascade(CascadeMode.Stop)
            .MaximumLength(50)
            .WithStatusCode(StatusCode.LastNameTooLong);

        When(
            updateProfileModel => updateProfileModel.Email is not null,
            () =>
            {
                RuleFor(updateProfileModel => updateProfileModel.Email)
                    .Cascade(CascadeMode.Stop)
                    .EmailAddress()
                    .WithStatusCode(StatusCode.InvalidEmailFormat)
                    .MustAsync(validationService.IsEmailUniqueAsync)
                    .WithStatusCode(StatusCode.EmailAlreadyExists);
            }
        );
    }
}