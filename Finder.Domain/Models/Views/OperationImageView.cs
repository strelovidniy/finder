namespace Finder.Domain.Models.Views;

public record OperationImageView(
    Guid Id,
    string ImageUrl,
    string ImageThumbnailUrl,
    int Position
);