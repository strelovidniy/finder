namespace Finder.Domain.Models.Create;

public record CreateSearchLocationRequestModel(
    string Title,
    string Description,
    double Latitude,
    double Longitude
) : IValidatableModel;