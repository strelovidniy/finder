using Finder.Data.Enums;
using Finder.Domain.Models.Update;
using Finder.Domain.Validators.Extensions;
using FluentValidation;

namespace Finder.Domain.Validators;

internal class UpdateAddressModelValidator : AbstractValidator<UpdateAddressModel>
{
    public UpdateAddressModelValidator()
    {
        RuleFor(updateAddressModel => updateAddressModel.AddressLine1)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithStatusCode(StatusCode.AddressLine1Required)
            .MaximumLength(200)
            .WithStatusCode(StatusCode.AddressLine1TooLong);

        RuleFor(updateAddressModel => updateAddressModel.AddressLine2)
            .Cascade(CascadeMode.Stop)
            .MaximumLength(200)
            .WithStatusCode(StatusCode.AddressLine2TooLong);

        RuleFor(updateAddressModel => updateAddressModel.City)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithStatusCode(StatusCode.CityRequired)
            .MaximumLength(200)
            .WithStatusCode(StatusCode.CityTooLong);

        RuleFor(updateAddressModel => updateAddressModel.State)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithStatusCode(StatusCode.StateRequired)
            .MaximumLength(200)
            .WithStatusCode(StatusCode.StateTooLong);

        RuleFor(updateAddressModel => updateAddressModel.PostalCode)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithStatusCode(StatusCode.PostalCodeRequired)
            .MaximumLength(200)
            .WithStatusCode(StatusCode.PostalCodeTooLong);

        RuleFor(updateAddressModel => updateAddressModel.Country)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithStatusCode(StatusCode.CountryRequired)
            .MaximumLength(200)
            .WithStatusCode(StatusCode.CountryTooLong);
    }
}