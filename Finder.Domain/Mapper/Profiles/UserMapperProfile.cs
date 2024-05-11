using AutoMapper;
using Finder.Data.Entities;
using Finder.Domain.Mapper.Converters.User;
using Finder.Domain.Models.Views;

namespace Finder.Domain.Mapper.Profiles;

internal class UserMapperProfile : Profile
{
    public UserMapperProfile()
    {
        CreateMap<User, UserView>().ConvertUsing(new UserToUserViewConverter());
    }
}