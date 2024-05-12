using AutoMapper;
using Finder.Domain.Models.Views;

namespace Finder.Domain.Mapper.Converters.SearchOperation;

internal class SearchOperationToSearchOperationViewConverter : ITypeConverter<Data.Entities.SearchOperation, SearchOperationView>
{
    public SearchOperationView Convert(
        Data.Entities.SearchOperation searchOperation,
        SearchOperationView searchOperationView,
        ResolutionContext context
    ) => new(
        searchOperation.Id,
        searchOperation.Title,
        searchOperation.Description,
        searchOperation.Tags ?? [],
        searchOperation.ShowContactInfo
            ? context.Mapper.Map<ContactInfoView>(searchOperation.Creator?.Details?.ContactInfo)
            : null,
        searchOperation.Creator?.FullName,
        searchOperation.Creator?.Details?.ImageUrl,
        searchOperation.Creator?.Details?.ImageThumbnailUrl,
        searchOperation.UserApplications.Count,
        searchOperation.CreatedAt,
        searchOperation.UpdatedAt,
        context.Mapper.Map<IEnumerable<OperationImageView>>(searchOperation.Images?.OrderBy(image => image.Position)),
        context.Mapper.Map<IEnumerable<OperationLocationView>>(searchOperation.OperationLocations)
    );
}