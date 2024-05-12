using Finder.Domain.Models;
using Finder.Domain.Models.Create;
using Finder.Domain.Models.Update;
using Finder.Domain.Models.Views;

namespace Finder.Domain.Services.Abstraction;

public interface ISearchOperationService
{
    public Task<SearchOperationView> GetSearchOperationAsync(
        Guid id,
        CancellationToken cancellationToken = default
    );

    public Task CreateSearchOperationAsync(
        CreateSearchOperationRequestModel createSearchOperationModel,
        CancellationToken cancellationToken = default
    );

    public Task UpdateSearchOperationAsync(
        UpdateSearchOperationRequestModel updateSearchOperationRequestModel,
        CancellationToken cancellationToken = default
    );

    public Task<PagedCollectionView<SearchOperationView>> GetSearchOperationsAsync(
        QueryParametersModel queryParametersModel,
        CancellationToken cancellationToken = default
    );

    public Task DeleteSearchOperationAsync(
        Guid id,
        CancellationToken cancellationToken = default
    );

    public byte[] GenerateSearchOperationQr(
        Guid id,
        CancellationToken cancellationToken = default
    );

    Task AddLocationsToSearchOperationAsync(
        Guid searchOperationId,
        IEnumerable<CreateSearchLocationRequestModel> locationRequests,
        CancellationToken cancellationToken = default
    );

    Task ApplyForSearchOperationAsync(Guid operationId, CancellationToken cancellationToken = default);

    public Task<byte[]> GetSearchOperationPdfAsync(
        Guid id,
        CancellationToken cancellationToken = default
    );

    Task<string> CreateChatBySearchOperationAsync(Guid searchOperationId, CancellationToken cancellationToken = default);

    Task ConfirmSearchOperationAsync(Guid searchOperationId, CancellationToken cancellationToken = default);
}