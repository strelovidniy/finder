using RichEnum;

namespace Finder.Data.Enums.RichEnums;

public class NotificationContent(string value) : RichEnum<string>(value)
{
    public static NotificationContent NewSearchOperation(
        string title
    ) => new($"New request \"{title}\" is waiting for your help!");

    public static NotificationContent SearchOperationUpdated(
        string title
    ) => new($"\"{title}\" was updated by Issuer. Check it again!");

    public static NotificationContent ApplicationReceived(
        string title
    ) => new($"\"{title}\" application received! Check it!");
}