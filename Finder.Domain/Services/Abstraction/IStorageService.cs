using Finder.Data.Enums.RichEnums;
using FileInfo = Finder.Domain.Models.FileInfo;

namespace Finder.Domain.Services.Abstraction;

public interface IStorageService
{
    public Task<string> SaveFileAsync(
        byte[] file,
        string fileExtension,
        FolderName folderName,
        CancellationToken cancellationToken = default
    );

    public void RemoveFile(
        string fileUrl,
        FolderName folderName,
        CancellationToken cancellationToken = default
    );

    public Task<byte[]> GetFileAsync(
        FileInfo info,
        CancellationToken cancellationToken = default
    );
}