namespace Finder.Domain.Models.Views;

public record OperationLocationView(
    Guid Id,
    string Title,
    string Description,
    double Latitude,
    double Longitude
);