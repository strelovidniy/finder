using AutoMapper;
using Finder.Data.Entities;
using Finder.Domain.Mapper.Converters.OperationImage;
using Finder.Domain.Models.Views;

namespace Finder.Domain.Mapper.Profiles;

internal class OperationImageMapperProfile : Profile
{
    public OperationImageMapperProfile()
    {
        CreateMap<OperationImage, OperationImageView>()
            .ConvertUsing(new OperationImageToHelpRequestImageViewConverter());
    }
}