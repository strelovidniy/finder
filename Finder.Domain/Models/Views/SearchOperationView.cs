using Finder.Data.Enums;

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
    int ApplicantsCount,
    Guid CreatorId,
    bool IsAlreadyApplied,
    DateTime CreatedAt,
    DateTime? UpdatedAt,
    string? ChatUrl,
    SearchOperationType SearchOperationType,
    SearchOperationStatus SearchOperationStatus,
    IEnumerable<OperationImageView> Images,
    IEnumerable<OperationLocationView> Locations
);