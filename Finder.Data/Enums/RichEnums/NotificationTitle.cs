using RichEnum;

namespace Finder.Data.Enums.RichEnums;

public class NotificationTitle(string value) : RichEnum<string>(value)
{
    public static NotificationTitle SearchOperationRequest => new("New Search Operation!");

    public static NotificationTitle SearchOperationUpdated => new("Search Operation Updated!");

    public static NotificationTitle ApplicationReceived => new("Application Received!");
}