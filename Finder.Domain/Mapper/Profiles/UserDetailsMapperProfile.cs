using AutoMapper;
using Finder.Data.Entities;
using Finder.Domain.Mapper.Converters.UserDetails;
using Finder.Domain.Models.Views;

namespace Finder.Domain.Mapper.Profiles;

internal class UserDetailsMapperProfile : Profile
{
    public UserDetailsMapperProfile()
    {
        CreateMap<UserDetails, UserDetailsView>().ConvertUsing(new UserDetailsToUserDetailsViewConverter());
    }
}