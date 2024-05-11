using Finder.Domain.Constants;
using Finder.Domain.Settings.Abstraction;

namespace Finder.Domain.Settings.Realization;

internal class ImageSettings : IImageSettings
{
    public int MaxImageSize { get; set; } = Defaults.MaxImageSize;

    public int MaxThumbnailSize { get; set; } = Defaults.MaxThumbnailSize;
}