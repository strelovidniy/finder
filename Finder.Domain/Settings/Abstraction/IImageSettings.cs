namespace Finder.Domain.Settings.Abstraction;

internal interface IImageSettings
{
    public int MaxImageSize { get; set; }

    public int MaxThumbnailSize { get; set; }
}