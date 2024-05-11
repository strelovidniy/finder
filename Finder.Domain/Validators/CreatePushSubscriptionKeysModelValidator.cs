using Finder.Data.Enums;
using Finder.Domain.Models.Create;
using Finder.Domain.Validators.Extensions;
using FluentValidation;

namespace Finder.Domain.Validators;

internal class CreatePushSubscriptionKeysModelValidator : AbstractValidator<CreatePushSubscriptionKeysModel>
{
    public CreatePushSubscriptionKeysModelValidator()
    {
        RuleFor(createPushSubscriptionKeysModel => createPushSubscriptionKeysModel.Auth)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithStatusCode(StatusCode.AuthRequired)
            .MaximumLength(500)
            .WithStatusCode(StatusCode.AuthTooLong);

        RuleFor(createPushSubscriptionKeysModel => createPushSubscriptionKeysModel.P256dh)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithStatusCode(StatusCode.P256dhRequired)
            .MaximumLength(500)
            .WithStatusCode(StatusCode.P256dhTooLong);
    }
}