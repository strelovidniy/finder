using RichEnum;

namespace Finder.Data.Enums.RichEnums;

public class EmailSubject(string value) : RichEnum<string>(value)
{
    public static EmailSubject ResetPassword =>
        new("Reset your password");

    public static EmailSubject CreateAccount =>
        new("Create your account");
}