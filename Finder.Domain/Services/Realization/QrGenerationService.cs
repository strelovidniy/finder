using Finder.Domain.Services.Abstraction;
using QRCoder;

namespace Finder.Domain.Services.Realization;

internal class QrGenerationService : IQrGenerationService
{
    public byte[] GenerateQrAsync(
        string findEventUrl,
        CancellationToken cancellationToken = default
    )
    {
        QRCodeGenerator qrGenerator = new QRCodeGenerator();
        QRCodeData qrCodeData = qrGenerator.CreateQrCode(findEventUrl, QRCodeGenerator.ECCLevel.Q);
        PngByteQRCode qrCode = new PngByteQRCode(qrCodeData);
        byte[] qrCodeImage = qrCode.GetGraphic(20);

        return qrCodeImage;
    }
}
