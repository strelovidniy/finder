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
        var qrGenerator = new QRCodeGenerator();
        var qrCodeData = qrGenerator.CreateQrCode(searchOperationUrl, QRCodeGenerator.ECCLevel.Q);
        var qrCode = new PngByteQRCode(qrCodeData);
        var qrCodeImage = qrCode.GetGraphic(20);

        return qrCodeImage;
    }
}