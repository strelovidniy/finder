namespace Finder.Domain.Models.Views;

public record HelpRequestView(
    Guid Id,
    string Title,
    string Description,
    IEnumerable<string> Tags,
    double? Latitude,
    double? Longitude,
    ContactInfoView? ContactInfo,
    string? IssuerName,
    string? IssuerImage,
    string? IssuerImageThumbnail,
    DateTime? Deadline,
    IEnumerable<HelpRequestImageView> Images
);