namespace Finder.Domain.Models.Update;

public record UpdateAddressModel(
    string AddressLine1,
    string? AddressLine2,
    string City,
    string State,
    string PostalCode,
    string Country
) : IValidatableModel;