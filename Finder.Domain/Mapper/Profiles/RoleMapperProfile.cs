using AutoMapper;
using Finder.Data.Entities;
using Finder.Domain.Mapper.Converters.Role;
using Finder.Domain.Models.Views;

namespace Finder.Domain.Mapper.Profiles;

internal class RoleMapperProfile : Profile
{
    public RoleMapperProfile()
    {
        CreateMap<Role, AccessView>().ConvertUsing(new RoleToAccessViewConverter());
    }
}