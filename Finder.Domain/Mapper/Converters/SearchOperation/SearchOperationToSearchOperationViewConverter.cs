using AutoMapper;
using Finder.Domain.Models.Views;

namespace Finder.Domain.Mapper.Converters.SearchOperation;

internal class SearchOperationToSearchOperationViewConverter
    : ITypeConverter<Data.Entities.SearchOperation, SearchOperationView>
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
        searchOperation.CreatorUserId,
        searchOperation.UserApplications.Select(x => x.UserId).Any(x => x == searchOperation.CreatorUserId),
        searchOperation.CreatedAt,
        searchOperation.UpdatedAt,
        searchOperation.ChatLink,
        searchOperation.OperationType,
        searchOperation.OperationStatus,
        context.Mapper.Map<IEnumerable<OperationImageView>>(searchOperation.Images?.OrderBy(image => image.Position)),
        context.Mapper.Map<IEnumerable<OperationLocationView>>(searchOperation.OperationLocations)
    );
}