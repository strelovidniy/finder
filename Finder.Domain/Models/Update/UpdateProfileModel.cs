namespace Finder.Domain.Models.Update;

public record UpdateProfileModel(
    Guid Id,
    string? FirstName,
    string? LastName,
    string? Email
) : IValidatableModel;