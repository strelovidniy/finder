namespace Finder.Domain.Models.Views;

public record HelpRequestImageView(
    Guid Id,
    string ImageUrl,
    string ImageThumbnailUrl,
    int Position
);