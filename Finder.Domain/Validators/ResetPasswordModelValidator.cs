using Finder.Data.Enums;
using Finder.Domain.Models;
using Finder.Domain.Services.Abstraction;
using Finder.Domain.Validators.Extensions;
using FluentValidation;

namespace Finder.Domain.Validators;

internal class ResetPasswordModelValidator : AbstractValidator<ResetPasswordModel>
{
    public ResetPasswordModelValidator(IValidationService validationService)
    {
        RuleFor(resetPasswordModel => resetPasswordModel.Email)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithStatusCode(StatusCode.EmailRequired)
            .EmailAddress()
            .WithStatusCode(StatusCode.InvalidEmailFormat)
            .MustAsync(validationService.IsUserExistAsync)
            .WithStatusCode(StatusCode.UserNotFound);
    }
}