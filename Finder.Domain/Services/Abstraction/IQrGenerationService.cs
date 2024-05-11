namespace Finder.Domain.Services.Abstraction
{
    public interface IQrGenerationService
    {
        public byte[] GenerateQr(
            string searchOperationUrl,
            CancellationToken cancellationToken = default
        );
    }
}