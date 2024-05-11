using Finder.Data.Enums;
using Finder.Domain.Models.Set;
using Finder.Domain.Services.Abstraction;
using Finder.Domain.Validators.Extensions;
using FluentValidation;

namespace Finder.Domain.Validators;

internal class SetNewPasswordModelValidator : AbstractValidator<SetNewPasswordModel>
{
    public SetNewPasswordModelValidator(IValidationService validationService)
    {
        RuleFor(setNewPasswordModel => setNewPasswordModel.NewPassword)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithStatusCode(StatusCode.PasswordRequired)
            .MinimumLength(8)
            .WithStatusCode(StatusCode.PasswordMustHaveAtLeast8Characters)
            .MaximumLength(32)
            .WithStatusCode(StatusCode.PasswordMustHaveNotMoreThan32Characters)
            .Matches("[A-Z]")
            .WithStatusCode(StatusCode.PasswordMustHaveAtLeastOneUppercaseLetter)
            .Matches("[a-z]")
            .WithStatusCode(StatusCode.PasswordMustHaveAtLeastOneLowercaseLetter)
            .Matches("[0-9]")
            .WithStatusCode(StatusCode.PasswordMustHaveAtLeastOneDigit);

        RuleFor(setNewPasswordModel => setNewPasswordModel.ConfirmNewPassword)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithStatusCode(StatusCode.ConfirmPasswordRequired)
            .Equal(setNewPasswordModel => setNewPasswordModel.NewPassword)
            .WithStatusCode(StatusCode.PasswordsDoNotMatch);

        RuleFor(setNewPasswordModel => setNewPasswordModel.VerificationCode)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithStatusCode(StatusCode.VerificationCodeRequired)
            .MustAsync(validationService.IsUserWithVerificationCodeExistAsync)
            .WithStatusCode(StatusCode.InvalidVerificationCode);
    }
}