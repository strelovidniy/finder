using Finder.Domain.Services.Abstraction;
using QRCoder;

namespace Finder.Domain.Services.Realization;

internal class QrGenerationService : IQrGenerationService
{
    public byte[] GenerateQr(
        string searchOperationUrl,
        CancellationToken cancellationToken = default
    )
    {
        QRCodeGenerator qrGenerator = new QRCodeGenerator();
        QRCodeData qrCodeData = qrGenerator.CreateQrCode(searchOperationUrl, QRCodeGenerator.ECCLevel.Q);
        PngByteQRCode qrCode = new PngByteQRCode(qrCodeData);
        byte[] qrCodeImage = qrCode.GetGraphic(20);

        return qrCodeImage;
    }
}
