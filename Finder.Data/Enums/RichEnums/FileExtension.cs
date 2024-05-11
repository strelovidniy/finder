using RichEnum;

namespace Finder.Data.Enums.RichEnums;

public class FileExtension(string value) : RichEnum<string>(value)
{
    public static FileExtension Png => new("png");
}