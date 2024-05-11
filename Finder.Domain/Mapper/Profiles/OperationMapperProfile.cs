using AutoMapper;
using Finder.Data.Entities;
using Finder.Domain.Mapper.Converters.SearchOperation;
using Finder.Domain.Models.Views;

namespace Finder.Domain.Mapper.Profiles;

internal class OperationMapperProfile : Profile
{
    public OperationMapperProfile()
    {
        CreateMap<SearchOperation, SearchOperationView>().ConvertUsing(new SearchOperationToSearchOperationViewConverter());
    }
}