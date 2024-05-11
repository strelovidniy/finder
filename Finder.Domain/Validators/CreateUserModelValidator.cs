using Finder.Data.Enums;
using Finder.Domain.Models.Create;
using Finder.Domain.Services.Abstraction;
using Finder.Domain.Validators.Extensions;
using FluentValidation;

namespace Finder.Domain.Validators;

internal class CreateUserModelValidator : AbstractValidator<CreateUserModel>
{
    public CreateUserModelValidator(IValidationService validationService)
    {
        RuleFor(createUserModel => createUserModel.RegistrationToken)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithStatusCode(StatusCode.InvitationTokenRequired)
            .MustAsync(validationService.IsPendingUserExistAsync)
            .WithStatusCode(StatusCode.UserNotFound);
    }
}