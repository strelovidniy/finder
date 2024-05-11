using RichEnum;

namespace Finder.Data.Enums.RichEnums;

public class EmailViewLocation(string value) : RichEnum<string>(value)
{
    private const string Base = "/Views/EmailTemplates/";

    public static EmailViewLocation CreateAccountEmail =>
        new($"{Base}CreateAccountEmail/CreateAccountEmail.cshtml");

    public static EmailViewLocation ResetPasswordEmail =>
        new($"{Base}ResetPasswordEmail/ResetPasswordEmail.cshtml");
}