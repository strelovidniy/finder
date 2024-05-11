namespace Finder.Domain.Models;

internal record ResizedImageModel(
    byte[] ResizedImage,
    byte[]? ThumbnailImage = null
);