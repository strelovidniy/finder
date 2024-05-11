namespace Finder.Domain.Models.Views;

public record UserDetailsView(
    string? ImageUrl,
    string? ImageThumbnailUrl,
    AddressView Address,
    ContactInfoView ContactInfo
);