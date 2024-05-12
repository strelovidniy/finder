﻿using AutoMapper;
using EntityFrameworkCore.RepositoryInfrastructure;
using Finder.Data.Entities;
using Finder.Data.Enums;
using Finder.Data.Enums.RichEnums;
using Finder.Domain.Exceptions;
using Finder.Domain.Extensions;
using Finder.Domain.Models;
using Finder.Domain.Models.Create;
using Finder.Domain.Models.Update;
using Finder.Domain.Models.Views;
using Finder.Domain.Services.Abstraction;
using Finder.Domain.Validators.Runtime;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Finder.Domain.Services.Realization;

using Settings.Abstraction;

internal class SearchOperationService(
    IRepository<SearchOperation> searchOperationRepository,
    IRepository<OperationImage> operationImageRepository,
    ICurrentUserService currentUserService,
    IStorageService storageService,
    IImageService imageService,
    ISearchOperationNotificationService searchOperationNotificationService,
    IQrGenerationService qrGenerationService,
    IChatService chatService,
    IMapper mapper,
    ILogger<SearchOperationService> logger,
    IUrlSettings urlSettings
) : ISearchOperationService
{
    public async Task<SearchOperationView> GetSearchOperationAsync(
        Guid id,
        CancellationToken cancellationToken = default
    )
    {
        var searchOperation = await searchOperationRepository
            .Query()
            .Include(request => request.Images)
            .Include(request => request.Creator!.Details!.ContactInfo)
            .Include(op => op.UserApplications)
            .Include(op => op.OperationLocations)
            .FirstOrDefaultAsync(
                request => request.Id == id,
                cancellationToken
            );

        RuntimeValidator.Assert(searchOperation is not null, StatusCode.SearchOperationNotFound);
        RuntimeValidator.Assert(searchOperation!.DeletedAt is null, StatusCode.SearchOperationRemoved);

        return mapper.Map<SearchOperationView>(searchOperation);
    }

    public async Task CreateSearchOperationAsync(
        CreateSearchOperationRequestModel createSearchOperationModel,
        CancellationToken cancellationToken = default
    )
    {
        var currentUser = await currentUserService.GetCurrentUserAsync(cancellationToken);
        
        var addedSearchOperation = await searchOperationRepository.AddAsync(
            new SearchOperation
            {
                Description = createSearchOperationModel.Description,
                CreatorUserId = currentUser.Id,
                ShowContactInfo = createSearchOperationModel.ShowContactInfo,
                Tags = createSearchOperationModel.Tags,
                OperationType = createSearchOperationModel.OperationType,
                Title = createSearchOperationModel.Title
            },
            cancellationToken
        );

        await searchOperationRepository.SaveChangesAsync(cancellationToken);

        await AddSearchOperationImagesAsync(
            createSearchOperationModel.Images,
            addedSearchOperation.Id,
            cancellationToken
        );

        await searchOperationNotificationService.NotifyAboutCreatingSearchOperationAsync(addedSearchOperation,
            cancellationToken);
    }

    public async Task UpdateSearchOperationAsync(
        UpdateSearchOperationRequestModel updatesearchOperationRequestModel,
        CancellationToken cancellationToken = default
    )
    {
        var currentUser = await currentUserService.GetCurrentUserAsync(cancellationToken);

        var searchOperation = await searchOperationRepository
            .Query()
            .Include(searchOperation => searchOperation.Images)
            .FirstOrDefaultAsync(
                searchOperation => searchOperation.Id == updatesearchOperationRequestModel.Id,
                cancellationToken
            );

        RuntimeValidator.Assert(searchOperation is not null, StatusCode.SearchOperationNotFound);
        RuntimeValidator.Assert(searchOperation!.CreatorUserId == currentUser.Id, StatusCode.Forbidden);

        if (searchOperation.ShowContactInfo != updatesearchOperationRequestModel.ShowContactInfo)
        {
            searchOperation.ShowContactInfo = updatesearchOperationRequestModel.ShowContactInfo;
            searchOperation.UpdatedAt = DateTime.UtcNow;
        }

        if (searchOperation.Title != updatesearchOperationRequestModel.Title)
        {
            searchOperation.Title = updatesearchOperationRequestModel.Title;
            searchOperation.UpdatedAt = DateTime.UtcNow;
        }

        if (searchOperation.OperationType != updatesearchOperationRequestModel.OperationType)
        {
            searchOperation.OperationType = updatesearchOperationRequestModel.OperationType;
            searchOperation.UpdatedAt = DateTime.UtcNow;
        }

        if (searchOperation.OperationStatus != updatesearchOperationRequestModel.OperationStatus)
        {
            searchOperation.OperationStatus = updatesearchOperationRequestModel.OperationStatus;
            searchOperation.UpdatedAt = DateTime.UtcNow;
        }

        if (!searchOperation.Tags?.SequenceEqual(updatesearchOperationRequestModel.Tags) is true)
        {
            searchOperation.Tags = updatesearchOperationRequestModel.Tags.ToList();
            searchOperation.UpdatedAt = DateTime.UtcNow;
        }

        await searchOperationRepository.SaveChangesAsync(cancellationToken);

        if (updatesearchOperationRequestModel.ImagesToDelete is not null)
        {
            var imagesToDelete = await operationImageRepository
                .Query()
                .Where(
                    searchOperationImage => searchOperationImage.OperationId == searchOperation.Id
                                            && updatesearchOperationRequestModel.ImagesToDelete.Contains(
                                                searchOperationImage.Id)
                )
                .ToListAsync(cancellationToken);

            await operationImageRepository.DeleteRangeAsync(imagesToDelete, cancellationToken);
        }

        if (searchOperation.Images is not null && updatesearchOperationRequestModel.ImagesToDelete is not null)
        {
            var idsToFixPosition = searchOperation
                .Images
                .Select(image => image.Id)
                .Except(updatesearchOperationRequestModel.ImagesToDelete);

            var images = await operationImageRepository
                .Query()
                .Where(image => idsToFixPosition.Contains(image.Id))
                .OrderBy(image => image.Position)
                .ToListAsync(cancellationToken);

            for (var i = 0; i < images.Count; i++)
            {
                images[i].Position = i + 1;
            }
        }

        await operationImageRepository.SaveChangesAsync(cancellationToken);

        if (updatesearchOperationRequestModel.Images is not null)
        {
            await AddSearchOperationImagesAsync(
                updatesearchOperationRequestModel.Images,
                searchOperation.Id,
                cancellationToken
            );
        }

        await searchOperationNotificationService.NotifyAboutUpdatingSearchOperationAsync(searchOperation,
            cancellationToken);
    }

    public async Task<PagedCollectionView<SearchOperationView>> GetSearchOperationsAsync(
        QueryParametersModel queryParametersModel,
        CancellationToken cancellationToken = default
    )
    {
        var searchOperations = searchOperationRepository
            .Query()
            .Include(op => op.UserApplications)
            .AsNoTracking();

        var currentUser = await currentUserService.GetCurrentUserAsync(cancellationToken);

        searchOperations = searchOperations.Where(searchOperation => searchOperation.DeletedAt == null);

        if (currentUser.Role?.CanCreateHelpRequest is not true)
        {
            searchOperations =
                searchOperations.Where(searchOperation => searchOperation.CreatorUserId == currentUser.Id);
        }

        if (!string.IsNullOrWhiteSpace(queryParametersModel.SearchQuery))
        {
            var names = queryParametersModel.SearchQuery.Split(' ', StringSplitOptions.RemoveEmptyEntries);

            searchOperations = searchOperations.Where(searchOperation =>
                (searchOperation.Tags as object as string)!.Contains(queryParametersModel.SearchQuery)
                || searchOperation.Title.Contains(queryParametersModel.SearchQuery)
                || searchOperation.Description.Contains(queryParametersModel.SearchQuery)
                || (names.Length == 2
                    && searchOperation.Creator!.FirstName == names[0]
                    && searchOperation.Creator.LastName == names[1])
            );
        }

        searchOperations = searchOperations.ExpandAndSort(queryParametersModel, logger);

        try
        {
            return new PagedCollectionView<SearchOperationView>(
                mapper.Map<IEnumerable<SearchOperationView>>(
                    await searchOperations
                        .Skip(queryParametersModel.PageSize * (queryParametersModel.PageNumber - 1))
                        .Take(queryParametersModel.PageSize)
                        .ToListAsync(cancellationToken)
                ),
                await searchOperations.CountAsync(cancellationToken)
            );
        }
        catch (Exception e)
        {
            logger.LogError(e, ErrorMessage.HelpRequestsGettingError);

            throw new ApiException(StatusCode.QueryResultError);
        }
    }

    public async Task DeleteSearchOperationAsync(
        Guid id,
        CancellationToken cancellationToken = default
    )
    {
        var searchOperation = await searchOperationRepository
            .Query()
            .FirstOrDefaultAsync(
                request => request.Id == id,
                cancellationToken
            );

        RuntimeValidator.Assert(searchOperation is not null, StatusCode.HelperRoleNotFound);

        searchOperation!.DeletedAt = DateTime.UtcNow;

        await searchOperationRepository.SaveChangesAsync(cancellationToken);
    }

    public byte[] GenerateSearchOperationQr(
        Guid id,
        CancellationToken cancellationToken = default
    )
    {
        var url = $"{urlSettings.AppUrl.TrimEnd('/')}/search-operations/{id}";

        return qrGenerationService.GenerateQr(url);
    }

    public async Task ApplyForSearchOperationAsync(Guid operationId, CancellationToken cancellationToken = default)
    {
        var currentUser = await currentUserService.GetCurrentUserAsync(cancellationToken);

        RuntimeValidator.Assert(currentUser != null, StatusCode.Unauthorized);

        var operation = await searchOperationRepository
            .Query()
            .Include(op => op.UserApplications)
            .FirstOrDefaultAsync(op => op.Id == operationId, cancellationToken);

        RuntimeValidator.Assert(operation != null, StatusCode.OperationNotFound);

        var alreadyApplied = operation!.UserApplications.Any(a => a.UserId == currentUser!.Id);
        RuntimeValidator.Assert(!alreadyApplied, StatusCode.AlreadyApplied);

        var application = new UserSearchOperation
        {
            UserId = currentUser!.Id,
            SearchOperationId = operationId
        };

        operation.UserApplications.Add(application);

        await searchOperationRepository.SaveChangesAsync(cancellationToken);

        // UNTESTED!!!!!!!!!!!!!!!!!!!!!!!!
        try
        {
            await searchOperationNotificationService.NotifyAboutApplicationReceivedAsync(operation, currentUser,
                cancellationToken);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
    }

    public async Task AddLocationsToSearchOperationAsync(Guid searchOperationId,
        IEnumerable<CreateSearchLocationRequestModel> locationRequests, CancellationToken cancellationToken = default)
    {
        var searchOperation = await searchOperationRepository.Query()
            .Include(op => op.OperationLocations)
            .FirstOrDefaultAsync(op => op.Id == searchOperationId, cancellationToken);

        if (searchOperation == null)
        {
            throw new KeyNotFoundException("Search operation not found.");
        }

        foreach (var locationRequest in locationRequests)
        {
            var location = new OperationLocation
            {
                SearchOperationId = searchOperationId,
                Latitude = locationRequest.Latitude,
                Longitude = locationRequest.Longitude,
                Title = locationRequest.Title,
                Description = locationRequest.Description
            };

            searchOperation.OperationLocations.Add(location);
        }

        await searchOperationRepository.SaveChangesAsync(cancellationToken);
    }

    public async Task CreateChatBySearchOperationAsync(Guid searchOperationId, CancellationToken cancellationToken = default)
    {
        var searchOperation = await searchOperationRepository.Query()
            .Include(op => op.OperationLocations)
            .FirstOrDefaultAsync(op => op.Id == searchOperationId, cancellationToken);

        if (searchOperation == null)
        {
            throw new KeyNotFoundException("Search operation not found.");
        }

        var result = await chatService.CreateChannelAsync(searchOperation.Title);

        if (!string.IsNullOrEmpty(result.InviteLink))
        {
            searchOperation.ChatLink = result.InviteLink;
            await searchOperationRepository.SaveChangesAsync(cancellationToken);
        }
    }
    
    private async Task AddSearchOperationImagesAsync(
        IEnumerable<IFormFile> images,
        Guid searchOperationId,
        CancellationToken cancellationToken = default
    )
    {
        var lastImage = await operationImageRepository
            .Query()
            .Where(image => image.OperationId == searchOperationId)
            .OrderByDescending(image => image.Position)
            .FirstOrDefaultAsync(cancellationToken);

        var position = lastImage?.Position ?? 0;

        foreach (var file in images)
        {
            ++position;

            var resizedImageModel = await imageService.ResizeImageAsync(
                file,
                needThumbnail: true,
                keepAspectRatio: true,
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

            await operationImageRepository.AddAsync(
                new OperationImage
                {
                    ImageUrl = imageUrl,
                    ImageThumbnailUrl = imageThumbnailUrl,
                    Position = position,
                    OperationId = searchOperationId
                },
                cancellationToken
            );
        }

        await operationImageRepository.SaveChangesAsync(cancellationToken);
    }
}