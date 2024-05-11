using RichEnum;

namespace Finder.Data.Enums.RichEnums;

public class ImageLocation(string value) : RichEnum<string>(value)
{
    private const string Base = "Finder.Server.Views.Images.";

    public static ImageLocation Icon => new($"{Base}Logo.png");

    public static ImageLocation TwitterIcon => new($"{Base}Twitter.png");

    public static ImageLocation FacebookIcon => new($"{Base}Facebook.png");

    public static ImageLocation InstagramIcon => new($"{Base}Instagram.png");

    public static ImageLocation WhiteBackground => new($"{Base}WhiteBackground.png");

    public static ImageLocation Divider => new($"{Base}Divider.png");
}