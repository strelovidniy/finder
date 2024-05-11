using RichEnum;

namespace Finder.Data.Enums.RichEnums;

public class TableName(string value) : RichEnum<string>(value)
{
    public static TableName Addresses => new("Addresses");

    public static TableName ContactInfos => new("ContactInfos");

    public static TableName HelpRequestImages => new("HelpRequestImages");

    public static TableName HelpRequests => new("HelpRequests");

    public static TableName NotificationSettings => new("NotificationSettings");

    public static TableName PushSubscriptions => new("PushSubscriptions");

    public static TableName Roles => new("Roles");

    public static TableName UserDetails => new("UserDetails");

    public static TableName Users => new("Users");
}