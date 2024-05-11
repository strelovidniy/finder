using RichEnum;

namespace Finder.Data.Enums.RichEnums;

public class NotificationContent(string value) : RichEnum<string>(value)
{
    public static NotificationContent NewHelpRequest(
        string title
    ) => new($"New request \"{title}\" is waiting for your help!");

    public static NotificationContent HelpRequestUpdated(
        string title
    ) => new($"\"{title}\" was updated by Issuer. Check it again!");
}