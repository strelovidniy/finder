using Finder.Domain.Models;
using Finder.Domain.Services.Abstraction;
using Finder.Domain.Settings.Abstraction;
using Microsoft.AspNetCore.Http;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;

namespace Finder.Domain.Services.Realization;

internal class ImageService(
    IImageSettings imageSettings
) : IImageService
{
    public async Task<ResizedImageModel> ResizeImageAsync(
        IFormFile imageFile,
        bool keepAspectRatio = false,
        bool needThumbnail = false,
        bool force = false,
        CancellationToken cancellationToken = default
    )
    {
        await using var stream = new MemoryStream();

        await stream.FlushAsync(cancellationToken);
        stream.Position = 0;

        await imageFile.CopyToAsync(stream, cancellationToken);

        stream.Position = 0;

        return await ResizeImageAsync(
            stream,
            keepAspectRatio,
            needThumbnail,
            force,
            cancellationToken
        );
    }

    public async Task<ResizedImageModel> ResizeImageAsync(
        Stream imageStream,
        bool keepAspectRatio = false,
        bool needThumbnail = false,
        bool force = false,
        CancellationToken cancellationToken = default
    )
    {
        await using var stream = new MemoryStream();

        await stream.FlushAsync(cancellationToken);
        stream.Position = 0;

        imageStream.Position = 0;
        await imageStream.CopyToAsync(stream, cancellationToken);
        stream.Position = 0;

        return await ResizeImageAsync(
            stream,
            keepAspectRatio,
            needThumbnail,
            force,
            cancellationToken
        );
    }

    public async Task<ResizedImageModel> ResizeImageAsync(
        byte[] imageBytes,
        bool keepAspectRatio = false,
        bool needThumbnail = false,
        bool force = false,
        CancellationToken cancellationToken = default
    )
    {
        await using var stream = new MemoryStream();

        await stream.FlushAsync(cancellationToken);
        stream.Position = 0;

        await stream.WriteAsync(imageBytes, cancellationToken);

        stream.Position = 0;

        return await ResizeImageAsync(
            stream,
            keepAspectRatio,
            needThumbnail,
            force,
            cancellationToken
        );
    }

    private async Task<ResizedImageModel> ResizeImageAsync(
        MemoryStream stream,
        bool keepAspectRatio = false,
        bool needThumbnail = false,
        bool force = false,
        CancellationToken cancellationToken = default
    )
    {
        var maxSize = imageSettings.MaxImageSize;
        var maxThumbnailSize = imageSettings.MaxThumbnailSize;

        stream.Position = 0;

        using var image = await Image.LoadAsync(stream, cancellationToken);

        Size size;
        Size thumbnailSize;

        if (keepAspectRatio)
        {
            size = image.Height > image.Width
                ? new Size(0, maxSize)
                : new Size(maxSize, 0);

            thumbnailSize = image.Height > image.Width
                ? new Size(0, maxThumbnailSize)
                : new Size(maxThumbnailSize, 0);
        }
        else
        {
            size = new Size(maxSize, maxSize);

            thumbnailSize = new Size(maxThumbnailSize, maxThumbnailSize);
        }

        if (force || image.Height > maxSize || image.Width > maxSize)
        {
            image.Mutate(imageProcessingContext => imageProcessingContext.Resize(
                new ResizeOptions
                {
                    Size = size,
                    Compand = true,
                    Mode = ResizeMode.Stretch,
                    Position = AnchorPositionMode.Center,
                    PadColor = Color.Black,
                    Sampler = KnownResamplers.Bicubic,
                    PremultiplyAlpha = false
                }
            ));
        }

        await stream.FlushAsync(cancellationToken);
        stream.Position = 0;

        await image.SaveAsPngAsync(stream, cancellationToken);

        stream.Position = 0;

        var resizedImage = stream.ToArray();

        stream.Position = 0;

        if (!needThumbnail)
        {
            return new ResizedImageModel(
                resizedImage
            );
        }

        if (force || image.Height > maxThumbnailSize || image.Width > maxThumbnailSize)
        {
            image.Mutate(imageProcessingContext => imageProcessingContext.Resize(
                new ResizeOptions
                {
                    Size = thumbnailSize,
                    Compand = true,
                    Mode = ResizeMode.Stretch,
                    Position = AnchorPositionMode.Center,
                    PadColor = Color.Black,
                    Sampler = KnownResamplers.Bicubic,
                    PremultiplyAlpha = false
                }
            ));
        }

        await stream.FlushAsync(cancellationToken);
        stream.Position = 0;

        await image.SaveAsPngAsync(stream, cancellationToken);
        stream.Position = 0;

        var thumbnailImage = stream.ToArray();

        return new ResizedImageModel(
            resizedImage,
            thumbnailImage
        );
    }
}