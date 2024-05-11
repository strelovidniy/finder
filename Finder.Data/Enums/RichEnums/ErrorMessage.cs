using RichEnum;

namespace Finder.Data.Enums.RichEnums;

public class ErrorMessage(string value) : RichEnum<string>(value)
{
    public static ErrorMessage ApiExceptionOccurred =>
        new("Api Exception occurred");

    public static ErrorMessage ValidationExceptionOccurred =>
        new("Validation Exception occurred");

    public static ErrorMessage InternalServerErrorOccurred =>
        new("Internal Server Error Occurred");

    public static ErrorMessage EmailAttachmentAddingError =>
        new("Error while attaching files to email");

    public static ErrorMessage EmailSendingError =>
        new("Error while sending email");

    public static ErrorMessage NotificationSendingError =>
        new("Error while sending notification");

    public static ErrorMessage ExpandingPropertyError =>
        new("Error while expanding property");

    public static ErrorMessage SortingByPropertyError =>
        new("Error while sorting by property");

    public static ErrorMessage ProgramStopped =>
        new("Stopped program because of exception");

    public static ErrorMessage HelpRequestsGettingError =>
        new("Error while getting help requests");

    public static ErrorMessage UsersGettingError =>
        new("Error while getting users");

    public static ErrorMessage RolesGettingError =>
        new("Error while getting roles");

    public static ErrorMessage EmailNotSent =>
        new("Email was not sent | Subject - {subject} | Receiver - {receiver}");
}