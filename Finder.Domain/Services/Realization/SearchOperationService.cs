using AutoMapper;
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
        var helpRequest = await searchOperationRepository
            .Query()
            .Include(request => request.Images)
            .Include(request => request.Creator!.Details!.ContactInfo)
            .FirstOrDefaultAsync(
                request => request.Id == id,
                cancellationToken
            );

        RuntimeValidator.Assert(helpRequest is not null, StatusCode.HelpRequestNotFound);
        RuntimeValidator.Assert(helpRequest!.DeletedAt is null, StatusCode.HelpRequestRemoved);

        return mapper.Map<SearchOperationView>(helpRequest);
    }

    public async Task CreateSearchOperationAsync(
        CreateSearchOperationRequestModel createHelpRequestModel,
        CancellationToken cancellationToken = default
    )
    {
        var currentUser = await currentUserService.GetCurrentUserAsync(cancellationToken);

        var addedHelpRequest = await searchOperationRepository.AddAsync(
            new SearchOperation()
            {
                Description = createHelpRequestModel.Description,
                CreatorUserId = currentUser.Id,
                ShowContactInfo = createHelpRequestModel.ShowContactInfo,
                Tags = createHelpRequestModel.Tags,
                OperationType = createHelpRequestModel.OperationType,
                Title = createHelpRequestModel.Title
            },
            cancellationToken
        );

        await searchOperationRepository.SaveChangesAsync(cancellationToken);

        await AddHelpRequestImagesAsync(
            createHelpRequestModel.Images,
            addedHelpRequest.Id,
            cancellationToken
        );
        
        await searchOperationNotificationService.NotifyAboutCreatingSearchOperationAsync(addedHelpRequest, cancellationToken);
    }

    public async Task UpdateSearchOperationAsync(
        UpdateSearchOperationRequestModel updateHelpRequestRequestModel,
        CancellationToken cancellationToken = default
    )
    {
        var currentUser = await currentUserService.GetCurrentUserAsync(cancellationToken);

        var helpRequest = await searchOperationRepository
            .Query()
            .Include(helpRequest => helpRequest.Images)
            .FirstOrDefaultAsync(
                helpRequest => helpRequest.Id == updateHelpRequestRequestModel.Id,
                cancellationToken
            );

        RuntimeValidator.Assert(helpRequest is not null, StatusCode.HelpRequestNotFound);
        RuntimeValidator.Assert(helpRequest!.CreatorUserId == currentUser.Id, StatusCode.Forbidden);

        if (helpRequest.ShowContactInfo != updateHelpRequestRequestModel.ShowContactInfo)
        {
            helpRequest.ShowContactInfo = updateHelpRequestRequestModel.ShowContactInfo;
            helpRequest.UpdatedAt = DateTime.UtcNow;
        }

        if (helpRequest.Title != updateHelpRequestRequestModel.Title)
        {
            helpRequest.Title = updateHelpRequestRequestModel.Title;
            helpRequest.UpdatedAt = DateTime.UtcNow;
        }

        if (helpRequest.OperationType != updateHelpRequestRequestModel.OperationType)
        {
            helpRequest.OperationType = updateHelpRequestRequestModel.OperationType;
            helpRequest.UpdatedAt = DateTime.UtcNow;
        }
        
        if (helpRequest.OperationStatus != updateHelpRequestRequestModel.OperationStatus)
        {
            helpRequest.OperationStatus = updateHelpRequestRequestModel.OperationStatus;
            helpRequest.UpdatedAt = DateTime.UtcNow;
        }

        if (!helpRequest.Tags?.SequenceEqual(updateHelpRequestRequestModel.Tags) is true)
        {
            helpRequest.Tags = updateHelpRequestRequestModel.Tags.ToList();
            helpRequest.UpdatedAt = DateTime.UtcNow;
        }

        await searchOperationRepository.SaveChangesAsync(cancellationToken);

        if (updateHelpRequestRequestModel.ImagesToDelete is not null)
        {
            var imagesToDelete = await operationImageRepository
                .Query()
                .Where(
                    helpRequestImage => helpRequestImage.OperationId == helpRequest.Id
                        && updateHelpRequestRequestModel.ImagesToDelete.Contains(helpRequestImage.Id)
                )
                .ToListAsync(cancellationToken);

            await operationImageRepository.DeleteRangeAsync(imagesToDelete, cancellationToken);
        }

        if (helpRequest.Images is not null && updateHelpRequestRequestModel.ImagesToDelete is not null)
        {
            var idsToFixPosition = helpRequest
                .Images
                .Select(image => image.Id)
                .Except(updateHelpRequestRequestModel.ImagesToDelete);

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

        if (updateHelpRequestRequestModel.Images is not null)
        {
            await AddHelpRequestImagesAsync(
                updateHelpRequestRequestModel.Images,
                helpRequest.Id,
                cancellationToken
            );
        }
        
        await searchOperationNotificationService.NotifyAboutUpdatingSearchOperationAsync(helpRequest, cancellationToken);
    }

    public async Task<PagedCollectionView<SearchOperationView>> GetSearchOperationsAsync(
        QueryParametersModel queryParametersModel,
        CancellationToken cancellationToken = default
    )
    {
        var helpRequests = searchOperationRepository
            .Query()
            .AsNoTracking();

        var currentUser = await currentUserService.GetCurrentUserAsync(cancellationToken);

        helpRequests = helpRequests.Where(helpRequest => helpRequest.DeletedAt == null);

        if (currentUser.Role?.CanSeeHelpRequests is not true)
        {
            helpRequests = helpRequests.Where(helpRequest => helpRequest.CreatorUserId == currentUser.Id);
        }

        if (!string.IsNullOrWhiteSpace(queryParametersModel.SearchQuery))
        {
            var names = queryParametersModel.SearchQuery.Split(' ', StringSplitOptions.RemoveEmptyEntries);

            helpRequests = helpRequests.Where(helpRequest =>
                (helpRequest.Tags as object as string)!.Contains(queryParametersModel.SearchQuery)
                || helpRequest.Title.Contains(queryParametersModel.SearchQuery)
                || helpRequest.Description.Contains(queryParametersModel.SearchQuery)
                || (names.Length == 2
                    && helpRequest.Creator!.FirstName == names[0]
                    && helpRequest.Creator.LastName == names[1])
            );
        }

        helpRequests = helpRequests.ExpandAndSort(queryParametersModel, logger);

        try
        {
            return new PagedCollectionView<SearchOperationView>(
                mapper.Map<IEnumerable<SearchOperationView>>(
                    await helpRequests
                        .Skip(queryParametersModel.PageSize * (queryParametersModel.PageNumber - 1))
                        .Take(queryParametersModel.PageSize)
                        .ToListAsync(cancellationToken)
                ),
                await helpRequests.CountAsync(cancellationToken)
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
        var helpRequest = await searchOperationRepository
            .Query()
            .FirstOrDefaultAsync(
                request => request.Id == id,
                cancellationToken
            );

        RuntimeValidator.Assert(helpRequest is not null, StatusCode.HelpRequestNotFound);

        helpRequest!.DeletedAt = DateTime.UtcNow;

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

        var alreadyApplied = operation.UserApplications.Any(a => a.UserId == currentUser.Id);
        RuntimeValidator.Assert(!alreadyApplied, StatusCode.AlreadyApplied);

        var application = new UserSearchOperation
        {
            UserId = currentUser.Id,
            SearchOperationId = operationId
        };

        operation.UserApplications.Add(application);
    
        await searchOperationRepository.SaveChangesAsync(cancellationToken);

        // UNTESTED!!!!!!!!!!!!!!!!!!!!!!!!
        try
        {
            await searchOperationNotificationService.NotifyAboutApplicationReceivedAsync(operation, currentUser, cancellationToken);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
    }
    
    private async Task AddHelpRequestImagesAsync(
        IEnumerable<IFormFile> images,
        Guid helpRequestId,
        CancellationToken cancellationToken = default
    )
    {
        var lastImage = await operationImageRepository
            .Query()
            .Where(image => image.OperationId == helpRequestId)
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
                    OperationId = helpRequestId
                },
                cancellationToken
            );
        }

        await operationImageRepository.SaveChangesAsync(cancellationToken);
    }
}