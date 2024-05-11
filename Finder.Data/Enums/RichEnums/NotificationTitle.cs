using RichEnum;

namespace Finder.Data.Enums.RichEnums;

public class NotificationTitle(string value) : RichEnum<string>(value)
{
    public static NotificationTitle NewHelpRequest => new("New Help Request!");

    public static NotificationTitle HelpRequestUpdated => new("Help Request Updated!");
}