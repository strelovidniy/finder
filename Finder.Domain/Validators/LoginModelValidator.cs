using Finder.Data.Enums;
using Finder.Domain.Models;
using Finder.Domain.Services.Abstraction;
using Finder.Domain.Validators.Extensions;
using FluentValidation;

namespace Finder.Domain.Validators;

internal class LoginModelValidator : AbstractValidator<LoginModel>
{
    public LoginModelValidator(IValidationService validationService)
    {
        RuleFor(loginModel => loginModel.Email)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithStatusCode(StatusCode.EmailRequired)
            .EmailAddress()
            .WithStatusCode(StatusCode.InvalidEmailFormat)
            .MustAsync(validationService.IsUserExistAsync)
            .WithStatusCode(StatusCode.UserNotFound);

        RuleFor(loginModel => loginModel.Password)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithStatusCode(StatusCode.PasswordRequired)
            .MinimumLength(8)
            .WithStatusCode(StatusCode.PasswordLengthExceeded);
    }
}