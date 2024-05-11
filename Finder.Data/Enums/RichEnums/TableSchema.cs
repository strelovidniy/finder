using RichEnum;

namespace Finder.Data.Enums.RichEnums;

public class TableSchema(string value) : RichEnum<string>(value)
{
    public static TableSchema Dbo = new("dbo");
}