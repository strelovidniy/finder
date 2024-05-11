using Finder.Data.Enums.RichEnums;

namespace Finder.Domain.Models;

public class FileSize : IFormattable
{
    private const int KiloMultiplier = 1024;

    private readonly long _bytes;

    private FileSize(long size) => _bytes = size;

    public string ToString(string? format, IFormatProvider? formatProvider = null)
    {
        if (format == SizeType.Bytes)
        {
            return $"{GetBytes()} B";
        }

        if (format == SizeType.Kilobytes)
        {
            return $"{GetKilobytes()} KB";
        }

        if (format == SizeType.Megabytes)
        {
            return $"{GetMegabytes()} MB";
        }

        return ToString();
    }

    public override string ToString()
    {
        var mb = GetMegabytes();

        if (mb > 0)
        {
            return ToString(SizeType.Megabytes);
        }

        var kb = GetKilobytes();

        if (kb > 0)
        {
            return ToString(SizeType.Kilobytes);
        }

        return ToString(SizeType.Bytes);
    }

    public static FileSize FromBytes(long size) => new(size);

    public static FileSize FromKilobytes(double size) => FromBytes((long) (size * KiloMultiplier));

    public static FileSize FromMegabytes(double size) => FromKilobytes(size * KiloMultiplier);

    private long GetBytes() => _bytes;

    private long GetKilobytes() => GetBytes() / KiloMultiplier;

    private long GetMegabytes() => GetKilobytes() / KiloMultiplier;


    public static implicit operator long(FileSize size) => size._bytes;
}