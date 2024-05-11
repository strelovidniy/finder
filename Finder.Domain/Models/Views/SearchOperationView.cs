namespace Finder.Domain.Models.Views;

public record SearchOperationView(
    Guid Id,
    string Title,
    string Description,
    IEnumerable<string> Tags,
    ContactInfoView? ContactInfo,
    string? IssuerName,
    string? IssuerImage,
    string? IssuerImageThumbnail,
    IEnumerable<OperationImageView> Images
);