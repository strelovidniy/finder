using RichEnum;

namespace Finder.Data.Enums.RichEnums;

public class ContentType(string value) : RichEnum<string>(value)
{
    public static ContentType ImagePng => new("image/png");

    public static ContentType ImageJpeg => new("image/jpeg");

    public static ContentType ImageJpg => new("image/jpg");

    public static ContentType ImageGif => new("image/gif");

    public static ContentType ImageWebp => new("image/webp");

    public static ContentType ImageBmp => new("image/x-MS-bmp");

    public static ContentType ImagePbm => new("image/x-portable-bitmap");

    public static ContentType ImageTif => new("image/tif");

    public static ContentType ImageTiff => new("image/tiff");

    public static ContentType TextCsv => new("text/csv");

    public static ContentType TextXml => new("text/xml");

    public static ContentType ApplicationProblem => new("application/problem+json");

    public static ContentType ApplicationPdf => new("application/pdf");

    public static ContentType ApplicationJson => new("application/json");

    public static ContentType ApplicationOctetStream => new("application/octet-stream");
}