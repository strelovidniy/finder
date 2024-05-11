using Finder.Domain.Models;
using Microsoft.AspNetCore.Http;

namespace Finder.Domain.Services.Abstraction;

internal interface IImageService
{
    public Task<ResizedImageModel> ResizeImageAsync(
        IFormFile imageFile,
        bool keepAspectRatio = false,
        bool needThumbnail = false,
        bool force = false,
        CancellationToken cancellationToken = default
    );

    public Task<ResizedImageModel> ResizeImageAsync(
        Stream imageStream,
        bool keepAspectRatio = false,
        bool needThumbnail = false,
        bool force = false,
        CancellationToken cancellationToken = default
    );

    public Task<ResizedImageModel> ResizeImageAsync(
        byte[] imageBytes,
        bool keepAspectRatio = false,
        bool needThumbnail = false,
        bool force = false,
        CancellationToken cancellationToken = default
    );
}