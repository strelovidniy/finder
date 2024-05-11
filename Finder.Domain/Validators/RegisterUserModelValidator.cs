using Finder.Data.Enums;
using Finder.Domain.Models;
using Finder.Domain.Services.Abstraction;
using Finder.Domain.Validators.Extensions;
using FluentValidation;

namespace Finder.Domain.Validators;

internal class RegisterUserModelValidator : AbstractValidator<RegisterUserModel>
{
    public RegisterUserModelValidator(IValidationService validationService)
    {
        RuleFor(registerUserModel => registerUserModel.FirstName)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithStatusCode(StatusCode.FirstNameRequired)
            .Must(firstName => !firstName.Contains(' '))
            .WithStatusCode(StatusCode.FirstNameShouldNotContainWhiteSpace);

        RuleFor(registerUserModel => registerUserModel.LastName)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithStatusCode(StatusCode.LastNameRequired)
            .Must(lastName => !lastName.Contains(' '))
            .WithStatusCode(StatusCode.LastNameShouldNotContainWhiteSpace);

        RuleFor(registerUserModel => registerUserModel.Email)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithStatusCode(StatusCode.EmailRequired)
            .EmailAddress()
            .WithStatusCode(StatusCode.InvalidEmailFormat)
            .MustAsync(async (email, cancellationToken) =>
                !await validationService.IsUserExistAsync(email, cancellationToken))
            .WithStatusCode(StatusCode.UserAlreadyExists)
            .MaximumLength(200)
            .WithStatusCode(StatusCode.EmailTooLong);

        RuleFor(registerUserModel => registerUserModel.Password)
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

        RuleFor(registerUserModel => registerUserModel.ConfirmPassword)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithStatusCode(StatusCode.ConfirmPasswordRequired)
            .Equal(registerUserModel => registerUserModel.Password)
            .WithStatusCode(StatusCode.PasswordsDoNotMatch);
    }
}