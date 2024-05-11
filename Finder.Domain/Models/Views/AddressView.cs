namespace Finder.Domain.Models.Views;

public record AddressView(
    string AddressLine1,
    string? AddressLine2,
    string City,
    string State,
    string PostalCode,
    string Country
) : IValidatableModel;