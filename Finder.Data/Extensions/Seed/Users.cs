using Finder.Data.Entities;
using Finder.Data.Enums;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Finder.Data.Extensions.Seed;

internal static class Users
{
    public static void Seed(EntityTypeBuilder<User> builder)
    {
        builder.HasData(
            new User
            {
                Id = Guid.Parse("00a68a21-7b01-2211-abbc-0022480a1c03"),
                FirstName = "Roman",
                LastName = "Danylevych",
                Email = "roma.dan2001@gmail.com",
                Status = UserStatus.Active,
                RoleId = Guid.Parse("00000000-0000-0000-0000-000000000001"),
                PasswordHash
                    = "2576c639ea2309626fee6232e624ba921afada44537b9fa6592f03d5a1da7dd375fbd17b2af56655323327e8fd75a46d4932d54c4df61595844bc95fd5979c56"
            },
            new User
            {
                Id = Guid.Parse("6ebd3c1c-4a07-4c53-8a71-493386f28261"),
                FirstName = "Nazariy",
                LastName = "Chetvertukha",
                Email = "nazariy.chetvertukha@gmail.com",
                Status = UserStatus.Active,
                RoleId = Guid.Parse("a0a80c03-abbc-eb11-cabb-0022480a1c0a"),
                PasswordHash
                    = "2576c639ea2309626fee6232e624ba921afada44537b9fa6592f03d5a1da7dd375fbd17b2af56655323327e8fd75a46d4932d54c4df61595844bc95fd5979c56"
            },
            new User
            {
                Id = Guid.Parse("bafee45c-1874-4c40-b781-5221133b4c30"),
                FirstName = "Ann",
                LastName = "Stepaniuk",
                Email = "annstepaniuk12@gmail.com",
                Status = UserStatus.Active,
                RoleId = Guid.Parse("a0a80c03-abbc-eb11-cabb-0022480a1c0a"),
                PasswordHash
                    = "2576c639ea2309626fee6232e624ba921afada44537b9fa6592f03d5a1da7dd375fbd17b2af56655323327e8fd75a46d4932d54c4df61595844bc95fd5979c56"
            },
            new User
            {
                Id = Guid.Parse("e823f508-ce4e-498f-a401-dbf40c892dbb"),
                FirstName = "Roman",
                LastName = "Sadokha",
                Email = "roman.sadokha01@gmail.com",
                Status = UserStatus.Active,
                RoleId = Guid.Parse("a0a80c03-abbc-eb11-cabb-0022480a1c0a"),
                PasswordHash
                    = "2576c639ea2309626fee6232e624ba921afada44537b9fa6592f03d5a1da7dd375fbd17b2af56655323327e8fd75a46d4932d54c4df61595844bc95fd5979c56"
            },
            new User
            {
                Id = Guid.Parse("07afd050-0126-4610-8dae-854efa9fcfde"),
                FirstName = "Arthur",
                LastName = "Zavolovych",
                Email = "boredarthur@gmail.com",
                Status = UserStatus.Active,
                RoleId = Guid.Parse("a0a80c03-abbc-eb11-cabb-0022480a1c0a"),
                PasswordHash
                    = "2576c639ea2309626fee6232e624ba921afada44537b9fa6592f03d5a1da7dd375fbd17b2af56655323327e8fd75a46d4932d54c4df61595844bc95fd5979c56"
            });
    }
}