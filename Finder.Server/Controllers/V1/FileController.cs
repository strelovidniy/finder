using Finder.Data.Enums.RichEnums;
using Finder.Domain.Attributes;
using Finder.Domain.Helpers;
using Finder.Domain.Services.Abstraction;
using Finder.Server.Controllers.Base;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using FileInfo = Finder.Domain.Models.FileInfo;

namespace Finder.Server.Controllers.V1;

[RouteV1("files")]
public class FileController(
    IServiceProvider services,
    IStorageService storageService
) : BaseController(services)
{
    [AllowAnonymous]
    [HttpGet("{encodedInfo}")]
    public async Task<IActionResult> GetFileInfo(
        [FromRoute] string encodedInfo,
        CancellationToken cancellationToken = default
    )
    {
        var fileInfo = JsonBase64Helper.Decode<FileInfo>(encodedInfo);

        if (fileInfo == null)
        {
            return NotFound();
        }

        var bytes = await storageService.GetFileAsync(fileInfo, cancellationToken);

        return File(
            bytes,
            fileInfo.FolderName == FolderName.Avatars || fileInfo.FolderName == FolderName.AvatarThumbnails
                ? ContentType.ImagePng
                : ContentType.ApplicationOctetStream,
            fileInfo.FileName
        );
    }
}