using AutoMapper;
using Finder.Data.Entities;
using Finder.Domain.Mapper.Converters.OperationLocation;
using Finder.Domain.Models.Views;

namespace Finder.Domain.Mapper.Profiles;

internal class OperationLocationMapperProfile : Profile
{
    public OperationLocationMapperProfile()
    {
        CreateMap<OperationLocation, OperationLocationView>()
            .ConvertUsing(new OperationLocationToOperationLocationViewConverter());
    }
}