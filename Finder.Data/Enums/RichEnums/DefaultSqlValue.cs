using RichEnum;

namespace Finder.Data.Enums.RichEnums;

public class DefaultSqlValue(string value) : RichEnum<string>(value)
{
    public static DefaultSqlValue NewGuid => new("uuid_generate_v4()");

    public static DefaultSqlValue NowUtc => new("now() at time zone('utc')");
}