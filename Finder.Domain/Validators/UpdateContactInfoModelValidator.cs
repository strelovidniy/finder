using Finder.Data.Enums;
using Finder.Domain.Models.Update;
using Finder.Domain.Validators.Extensions;
using FluentValidation;

namespace Finder.Domain.Validators;

internal class UpdateContactInfoModelValidator : AbstractValidator<UpdateContactInfoModel>
{
    public UpdateContactInfoModelValidator()
    {
        RuleFor(updateContactInfoModel => updateContactInfoModel.Instagram)
            .Cascade(CascadeMode.Stop)
            .MaximumLength(200)
            .WithStatusCode(StatusCode.InstagramTooLong);

        RuleFor(updateContactInfoModel => updateContactInfoModel.LinkedIn)
            .Cascade(CascadeMode.Stop)
            .MaximumLength(200)
            .WithStatusCode(StatusCode.LinkedInTooLong);

        RuleFor(updateContactInfoModel => updateContactInfoModel.PhoneNumber)
            .Cascade(CascadeMode.Stop)
            .MaximumLength(100)
            .WithStatusCode(StatusCode.PhoneNumberTooLong)
            .Matches(@"\+?([\d|\(][\s|\(\d{3}\)|\.|\-|\d]{4,}\d)")
            .WithStatusCode(StatusCode.InvalidPhoneNumber);

        RuleFor(updateContactInfoModel => updateContactInfoModel.Skype)
            .Cascade(CascadeMode.Stop)
            .MaximumLength(200)
            .WithStatusCode(StatusCode.SkypeTooLong);

        RuleFor(updateContactInfoModel => updateContactInfoModel.Telegram)
            .Cascade(CascadeMode.Stop)
            .MaximumLength(200)
            .WithStatusCode(StatusCode.TelegramTooLong);

        RuleFor(updateContactInfoModel => updateContactInfoModel.Other)
            .Cascade(CascadeMode.Stop)
            .MaximumLength(200)
            .WithStatusCode(StatusCode.OtherTooLong);
    }
}