using EntityFrameworkCore.RepositoryInfrastructure;
using Finder.Data.Entities;
using Finder.Data.Enums;
using Finder.Data.Enums.RichEnums;
using Finder.Domain.Models.Set;
using Finder.Domain.Models.Update;
using Finder.Domain.Services.Abstraction;
using Finder.Domain.Validators.Runtime;
using Microsoft.EntityFrameworkCore;

namespace Finder.Domain.Services.Realization;

internal class UserDetailsService(
    ICurrentUserService currentUserService,
    IRepository<UserDetails> userDetailsRepository,
    IRepository<Address> addressRepository,
    IRepository<ContactInfo> contactInfoRepository,
    IImageService imageService,
    IStorageService storageService
) : IUserDetailsService
{
    public async Task UpdateAddressesAsync(
        UpdateAddressModel updateAddressModel,
        CancellationToken cancellationToken = default
    )
    {
        var userId = (await currentUserService.GetCurrentUserAsync(cancellationToken)).Id;

        var userDetails = await userDetailsRepository
            .Query()
            .Include(details => details.Address)
            .FirstOrDefaultAsync(
                details => details.UserId == userId,
                cancellationToken
            );

        if (userDetails is null)
        {
            userDetails = await userDetailsRepository.AddAsync(
                new UserDetails
                {
                    UserId = userId
                },
                cancellationToken
            );

            await userDetailsRepository.SaveChangesAsync(cancellationToken);
        }

        if (userDetails.Address is null)
        {
            var address = await addressRepository
                .AddAsync(
                    new Address
                    {
                        AddressLine1 = updateAddressModel.AddressLine1,
                        AddressLine2 = updateAddressModel.AddressLine2,
                        City = updateAddressModel.City,
                        State = updateAddressModel.State,
                        PostalCode = updateAddressModel.PostalCode,
                        Country = updateAddressModel.Country
                    },
                    cancellationToken
                );

            await addressRepository.SaveChangesAsync(cancellationToken);

            userDetails.AddressId = address.Id;
        }
        else
        {
            if (updateAddressModel.AddressLine1 != userDetails.Address.AddressLine1)
            {
                userDetails.Address.AddressLine1 = updateAddressModel.AddressLine1;
                userDetails.Address.UpdatedAt = DateTime.UtcNow;
            }

            if (updateAddressModel.AddressLine2 != userDetails.Address.AddressLine2)
            {
                userDetails.Address.AddressLine2 = updateAddressModel.AddressLine2;
                userDetails.Address.UpdatedAt = DateTime.UtcNow;
            }

            if (updateAddressModel.City != userDetails.Address.City)
            {
                userDetails.Address.City = updateAddressModel.City;
                userDetails.Address.UpdatedAt = DateTime.UtcNow;
            }

            if (updateAddressModel.State != userDetails.Address.State)
            {
                userDetails.Address.State = updateAddressModel.State;
                userDetails.Address.UpdatedAt = DateTime.UtcNow;
            }

            if (updateAddressModel.PostalCode != userDetails.Address.PostalCode)
            {
                userDetails.Address.PostalCode = updateAddressModel.PostalCode;
                userDetails.Address.UpdatedAt = DateTime.UtcNow;
            }

            if (updateAddressModel.Country != userDetails.Address.Country)
            {
                userDetails.Address.Country = updateAddressModel.Country;
                userDetails.Address.UpdatedAt = DateTime.UtcNow;
            }
        }

        await userDetailsRepository.SaveChangesAsync(cancellationToken);
    }

    public async Task SetUserAvatarAsync(
        SetUserAvatarModel model,
        CancellationToken cancellationToken = default
    )
    {
        var userId = (await currentUserService.GetCurrentUserAsync(cancellationToken)).Id;

        var userDetails = await userDetailsRepository
            .Query()
            .Include(details => details.Address)
            .FirstOrDefaultAsync(
                details => details.UserId == userId,
                cancellationToken
            );

        var resizedImageModel = await imageService.ResizeImageAsync(
            model.File,
            needThumbnail: true,
            keepAspectRatio: false,
            cancellationToken: cancellationToken
        );

        RuntimeValidator.Assert(
            resizedImageModel.ThumbnailImage is not null,
            StatusCode.ImageProcessingError
        );

        var imageUrl = await storageService.SaveFileAsync(
            resizedImageModel.ResizedImage,
            FileExtension.Png,
            FolderName.Avatars,
            cancellationToken
        );

        var imageThumbnailUrl = await storageService.SaveFileAsync(
            resizedImageModel.ThumbnailImage!,
            FileExtension.Png,
            FolderName.AvatarThumbnails,
            cancellationToken
        );

        if (userDetails is null)
        {
            userDetails = await userDetailsRepository.AddAsync(
                new UserDetails
                {
                    UserId = userId,
                    ImageUrl = imageUrl,
                    ImageThumbnailUrl = imageThumbnailUrl
                },
                cancellationToken
            );
        }
        else
        {
            if (userDetails.ImageUrl != null)
            {
                storageService.RemoveFile(
                    userDetails.ImageUrl,
                    FolderName.Avatars,
                    cancellationToken
                );
            }

            userDetails.ImageUrl = imageUrl;
            userDetails.UpdatedAt = DateTime.UtcNow;

            if (userDetails.ImageThumbnailUrl != null)
            {
                storageService.RemoveFile(
                    userDetails.ImageThumbnailUrl,
                    FolderName.AvatarThumbnails,
                    cancellationToken
                );
            }

            userDetails.ImageThumbnailUrl = imageThumbnailUrl;
            userDetails.UpdatedAt = DateTime.UtcNow;
        }

        await userDetailsRepository.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateContactInfoAsync(
        UpdateContactInfoModel updateContactInfoModel,
        CancellationToken cancellationToken = default
    )
    {
        var userId = (await currentUserService.GetCurrentUserAsync(cancellationToken)).Id;

        var userDetails = await userDetailsRepository
            .Query()
            .Include(details => details.Address)
            .FirstOrDefaultAsync(
                details => details.UserId == userId,
                cancellationToken
            );

        if (userDetails is null)
        {
            userDetails = await userDetailsRepository.AddAsync(
                new UserDetails
                {
                    UserId = userId
                },
                cancellationToken
            );

            await userDetailsRepository.SaveChangesAsync(cancellationToken);
        }

        if (userDetails.ContactInfo is null)
        {
            var contactInfo = await contactInfoRepository
                .AddAsync(
                    new ContactInfo
                    {
                        Instagram = updateContactInfoModel.Instagram,
                        LinkedIn = updateContactInfoModel.LinkedIn,
                        Other = updateContactInfoModel.Other,
                        PhoneNumber = updateContactInfoModel.PhoneNumber,
                        Skype = updateContactInfoModel.Skype,
                        Telegram = updateContactInfoModel.Telegram
                    },
                    cancellationToken
                );

            await contactInfoRepository.SaveChangesAsync(cancellationToken);

            userDetails.ContactInfoId = contactInfo.Id;
        }
        else
        {
            if (updateContactInfoModel.Instagram != userDetails.ContactInfo.Instagram)
            {
                userDetails.ContactInfo.Instagram = updateContactInfoModel.Instagram;
                userDetails.ContactInfo.UpdatedAt = DateTime.UtcNow;
            }

            if (updateContactInfoModel.LinkedIn != userDetails.ContactInfo.LinkedIn)
            {
                userDetails.ContactInfo.LinkedIn = updateContactInfoModel.LinkedIn;
                userDetails.ContactInfo.UpdatedAt = DateTime.UtcNow;
            }

            if (updateContactInfoModel.Other != userDetails.ContactInfo.Other)
            {
                userDetails.ContactInfo.Other = updateContactInfoModel.Other;
                userDetails.ContactInfo.UpdatedAt = DateTime.UtcNow;
            }

            if (updateContactInfoModel.PhoneNumber != userDetails.ContactInfo.PhoneNumber)
            {
                userDetails.ContactInfo.PhoneNumber = updateContactInfoModel.PhoneNumber;
                userDetails.ContactInfo.UpdatedAt = DateTime.UtcNow;
            }

            if (updateContactInfoModel.Skype != userDetails.ContactInfo.Skype)
            {
                userDetails.ContactInfo.Skype = updateContactInfoModel.Skype;
                userDetails.ContactInfo.UpdatedAt = DateTime.UtcNow;
            }

            if (updateContactInfoModel.Telegram != userDetails.ContactInfo.Telegram)
            {
                userDetails.ContactInfo.Telegram = updateContactInfoModel.Telegram;
                userDetails.ContactInfo.UpdatedAt = DateTime.UtcNow;
            }
        }

        await addressRepository.SaveChangesAsync(cancellationToken);
    }
}