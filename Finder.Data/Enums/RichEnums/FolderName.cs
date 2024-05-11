using RichEnum;

namespace Finder.Data.Enums.RichEnums;

public class FolderName(string value) : RichEnum<string>(value)
{
    public static FolderName Avatars => new("Avatars");

    public static FolderName AvatarThumbnails => new("AvatarThumbnails");

    public static FolderName Files => new("Files");
}