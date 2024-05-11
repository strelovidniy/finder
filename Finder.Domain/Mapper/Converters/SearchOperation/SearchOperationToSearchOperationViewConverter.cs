﻿using AutoMapper;
using Finder.Domain.Models.Views;

namespace Finder.Domain.Mapper.Converters.SearchOperation;

internal class SearchOperationToSearchOperationViewConverter : ITypeConverter<Data.Entities.SearchOperation, SearchOperationView>
{
    public SearchOperationView Convert(
        Data.Entities.SearchOperation helpRequest,
        SearchOperationView helpRequestView,
        ResolutionContext context
    ) => new(
        helpRequest.Id,
        helpRequest.Title,
        helpRequest.Description,
        helpRequest.Tags ?? [],
        helpRequest.ShowContactInfo
            ? context.Mapper.Map<ContactInfoView>(helpRequest.User?.Details?.ContactInfo)
            : null,
        helpRequest.User?.FullName,
        helpRequest.User?.Details?.ImageUrl,
        helpRequest.User?.Details?.ImageThumbnailUrl,
        context.Mapper.Map<IEnumerable<OperationImageView>>(helpRequest.Images?.OrderBy(image => image.Position))
    );
}