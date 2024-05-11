using AutoMapper;
using Finder.Data.Entities;
using Finder.Domain.Mapper.Converters.Address;
using Finder.Domain.Models.Views;

namespace Finder.Domain.Mapper.Profiles;

internal class AddressMapperProfile : Profile
{
    public AddressMapperProfile()
    {
        CreateMap<Address, AddressView>().ConvertUsing(new AddressToAddressViewConverter());
    }
}