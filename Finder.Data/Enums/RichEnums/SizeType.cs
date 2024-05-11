using RichEnum;

namespace Finder.Data.Enums.RichEnums;

public class SizeType(string value) : RichEnum<string>(value)
{
    public static SizeType Bytes => new("B");

    public static SizeType Megabytes => new("MB");

    public static SizeType Kilobytes => new("MB");
}