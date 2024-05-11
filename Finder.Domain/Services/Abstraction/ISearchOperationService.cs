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
        CreateSearchOperationRequestModel createHelpRequestModel,
        CancellationToken cancellationToken = default
    );

    public Task UpdateSearchOperationAsync(
        UpdateSearchOperationRequestModel updateHelpRequestRequestModel,
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
}