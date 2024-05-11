using RichEnum;

namespace Finder.Data.Enums.RichEnums;

public class NotificationTitle(string value) : RichEnum<string>(value)
{
    public static NotificationTitle SearchOperationRequest => new("New Help Request!");

    public static NotificationTitle SearchOperationUpdated => new("Help Request Updated!");
    
    public static NotificationTitle ApplicationReceived => new("Application Received!");
}