namespace Finder.Domain.Services.Abstraction
{
    internal interface IQrGenerationService
    {
        public byte[] GenerateQrAsync(
            string findEventUrl,
            CancellationToken cancellationToken = default
        );
    }
}