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
using Finder.Domain.Settings.Abstraction;
using Finder.Domain.Validators.Runtime;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace Finder.Domain.Services.Realization;

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

        var searchOperationStatus = SearchOperationStatus.Pending;

        if (currentUser.Role!.Type == RoleType.Admin)
        {
            searchOperationStatus = SearchOperationStatus.Active;
        }

        var addedSearchOperation = await searchOperationRepository.AddAsync(
            new SearchOperation
            {
                Description = createSearchOperationModel.Description,
                CreatorUserId = currentUser.Id,
                ShowContactInfo = createSearchOperationModel.ShowContactInfo,
                Tags = createSearchOperationModel.Tags,
                OperationType = createSearchOperationModel.OperationType,
                Title = createSearchOperationModel.Title,
                OperationStatus = searchOperationStatus
            },
            cancellationToken
        );

        await searchOperationRepository.SaveChangesAsync(cancellationToken);

        await AddSearchOperationImagesAsync(
            createSearchOperationModel.Images,
            addedSearchOperation.Id,
            cancellationToken
        );

        await searchOperationNotificationService.NotifyAboutCreatingSearchOperationAsync(
            addedSearchOperation,
            cancellationToken
        );
    }

    public async Task UpdateSearchOperationAsync(
        UpdateSearchOperationRequestModel updateSearchOperationRequestModel,
        CancellationToken cancellationToken = default
    )
    {
        var currentUser = await currentUserService.GetCurrentUserAsync(cancellationToken);

        var searchOperation = await searchOperationRepository
            .Query()
            .Include(searchOperation => searchOperation.Images)
            .FirstOrDefaultAsync(
                searchOperation => searchOperation.Id == updateSearchOperationRequestModel.Id,
                cancellationToken
            );

        RuntimeValidator.Assert(searchOperation is not null, StatusCode.SearchOperationNotFound);
        RuntimeValidator.Assert(searchOperation!.CreatorUserId == currentUser.Id, StatusCode.Forbidden);

        if (searchOperation.ShowContactInfo != updateSearchOperationRequestModel.ShowContactInfo)
        {
            searchOperation.ShowContactInfo = updateSearchOperationRequestModel.ShowContactInfo;
            searchOperation.UpdatedAt = DateTime.UtcNow;
        }

        if (searchOperation.Title != updateSearchOperationRequestModel.Title)
        {
            searchOperation.Title = updateSearchOperationRequestModel.Title;
            searchOperation.UpdatedAt = DateTime.UtcNow;
        }

        if (searchOperation.OperationType != updateSearchOperationRequestModel.OperationType)
        {
            searchOperation.OperationType = updateSearchOperationRequestModel.OperationType;
            searchOperation.UpdatedAt = DateTime.UtcNow;
        }

        if (searchOperation.OperationStatus != updateSearchOperationRequestModel.OperationStatus)
        {
            searchOperation.OperationStatus = updateSearchOperationRequestModel.OperationStatus;
            searchOperation.UpdatedAt = DateTime.UtcNow;
        }

        if (!searchOperation.Tags?.SequenceEqual(updateSearchOperationRequestModel.Tags) is true)
        {
            searchOperation.Tags = updateSearchOperationRequestModel.Tags.ToList();
            searchOperation.UpdatedAt = DateTime.UtcNow;
        }

        await searchOperationRepository.SaveChangesAsync(cancellationToken);

        if (updateSearchOperationRequestModel.ImagesToDelete is not null)
        {
            var imagesToDelete = await operationImageRepository
                .Query()
                .Where(
                    searchOperationImage => searchOperationImage.OperationId == searchOperation.Id
                        && updateSearchOperationRequestModel.ImagesToDelete.Contains(
                            searchOperationImage.Id)
                )
                .ToListAsync(cancellationToken);

            await operationImageRepository.DeleteRangeAsync(imagesToDelete, cancellationToken);
        }

        if (searchOperation.Images is not null && updateSearchOperationRequestModel.ImagesToDelete is not null)
        {
            var idsToFixPosition = searchOperation
                .Images
                .Select(image => image.Id)
                .Except(updateSearchOperationRequestModel.ImagesToDelete);

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

        if (updateSearchOperationRequestModel.Images is not null)
        {
            await AddSearchOperationImagesAsync(
                updateSearchOperationRequestModel.Images,
                searchOperation.Id,
                cancellationToken
            );
        }

        await searchOperationNotificationService.NotifyAboutUpdatingSearchOperationAsync(
            searchOperation,
            cancellationToken
        );
    }

    public async Task ConfirmSearchOperationAsync(
        Guid searchOperationId,
        CancellationToken cancellationToken = default
    )
    {
        var currentUser = await currentUserService.GetCurrentUserAsync(cancellationToken);
        RuntimeValidator.Assert(currentUser.Role!.Type == RoleType.Admin, StatusCode.Forbidden);

        var searchOperation = await searchOperationRepository
            .Query()
            .Include(searchOperation => searchOperation.Images)
            .FirstOrDefaultAsync(
                searchOperation => searchOperation.Id == searchOperationId,
                cancellationToken
            );

        RuntimeValidator.Assert(searchOperation is not null, StatusCode.SearchOperationNotFound);

        if (searchOperation!.OperationStatus == SearchOperationStatus.Pending)
        {
            searchOperation.OperationStatus = SearchOperationStatus.Active;
            searchOperation.UpdatedAt = DateTime.UtcNow;
        }

        await searchOperationRepository.SaveChangesAsync(cancellationToken);

        await searchOperationNotificationService.NotifyAboutUpdatingSearchOperationAsync(searchOperation,
            cancellationToken);
    }

    public async Task DeclineSearchOperationAsync(
        Guid searchOperationId,
        CancellationToken cancellationToken = default
    )
    {
        var currentUser = await currentUserService.GetCurrentUserAsync(cancellationToken);

        RuntimeValidator.Assert(currentUser.Role!.Type == RoleType.Admin, StatusCode.Forbidden);

        var searchOperation = await searchOperationRepository
            .Query()
            .Include(searchOperation => searchOperation.Images)
            .FirstOrDefaultAsync(
                searchOperation => searchOperation.Id == searchOperationId,
                cancellationToken
            );

        RuntimeValidator.Assert(searchOperation is not null, StatusCode.SearchOperationNotFound);

        if (searchOperation!.OperationStatus == SearchOperationStatus.Pending)
        {
            searchOperation.OperationStatus = SearchOperationStatus.Rejected;
            searchOperation.UpdatedAt = DateTime.UtcNow;
            searchOperation.DeletedAt = DateTime.UtcNow;
        }

        await searchOperationRepository.SaveChangesAsync(cancellationToken);

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

        if (currentUser.Role?.Type is not RoleType.Admin)
        {
            searchOperations = searchOperations.Where(searchOperation =>
                searchOperation.OperationStatus == SearchOperationStatus.Active);
        }

        if (currentUser.Role?.CanCreateSearchOperation is not true)
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
        var url = $"{urlSettings.AppUrl.TrimEnd('/')}/search-operations/details?id={id}";

        return qrGenerationService.GenerateQr(url);
    }

    public async Task ApplyForSearchOperationAsync(Guid operationId, CancellationToken cancellationToken = default)
    {
        var currentUser = await currentUserService.GetCurrentUserAsync(cancellationToken);

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

        try
        {
            await searchOperationNotificationService.NotifyAboutApplicationReceivedAsync(
                operation,
                currentUser,
                cancellationToken
            );
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
    }

    public async Task AddLocationsToSearchOperationAsync(
        Guid searchOperationId,
        IEnumerable<CreateSearchLocationRequestModel> locationRequests,
        CancellationToken cancellationToken = default
    )
    {
        var searchOperation = await searchOperationRepository.Query()
            .Include(operation => operation.OperationLocations)
            .FirstOrDefaultAsync(
                operation => operation.Id == searchOperationId,
                cancellationToken
            );

        RuntimeValidator.Assert(searchOperation != null, StatusCode.OperationNotFound);

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

            searchOperation!.OperationLocations.Add(location);
        }

        await searchOperationRepository.SaveChangesAsync(cancellationToken);
    }

    public async Task<string> CreateChatBySearchOperationAsync(
        Guid searchOperationId,
        CancellationToken cancellationToken = default
    )
    {
        var searchOperation = await searchOperationRepository.Query()
            .Include(op => op.OperationLocations)
            .FirstOrDefaultAsync(op => op.Id == searchOperationId, cancellationToken);

        RuntimeValidator.Assert(searchOperation != null, StatusCode.OperationNotFound);

        var result = await chatService.CreateChannelAsync(searchOperation!.Title);

        if (!string.IsNullOrEmpty(result.InviteLink))
        {
            searchOperation.ChatLink = result.InviteLink;
            await searchOperationRepository.SaveChangesAsync(cancellationToken);
        }

        return result.InviteLink;
    }

    public async Task<byte[]> GetSearchOperationPdfAsync(
        Guid id,
        CancellationToken cancellationToken = default
    )
    {
        var searchOperation = await searchOperationRepository
            .Query()
            .Include(request => request.Images)
            .FirstOrDefaultAsync(
                request => request.Id == id,
                cancellationToken
            );

        RuntimeValidator.Assert(searchOperation is not null, StatusCode.SearchOperationNotFound);
        RuntimeValidator.Assert(searchOperation!.DeletedAt is null, StatusCode.SearchOperationRemoved);

        byte[]? file = null;

        if (searchOperation.Images?.FirstOrDefault()?.ImageUrl is { } imageUrl)
        {
            var res = await new HttpClient().GetAsync(imageUrl, cancellationToken);

            file = await res.Content.ReadAsByteArrayAsync(cancellationToken);
        }

        var qrUrl = $"{urlSettings.AppUrl.TrimEnd('/')}/search-operations/details?id={id}";
        var bytesQR = qrGenerationService.GenerateQr(qrUrl);

        QuestPDF.Settings.License = LicenseType.Community;

        var title = "WARNING! Search operation";
        var documentTitle = searchOperation.Title;
        var description = searchOperation.Description;
        var footer = "Do not be indifferent!";

        var pdfBytes = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(2, Unit.Centimetre);
                    page.PageColor(Colors.White);
                    page.DefaultTextStyle(textStyle => textStyle.FontSize(20));
                    page.DefaultTextStyle(textStyle => textStyle.FontColor(Colors.Red.Darken4));

                    page.Header()
                        .Text(title)
                        .SemiBold().FontSize(36);

                    page.Content()
                        .PaddingVertical(1, Unit.Centimetre)
                        .Column(columnDescriptor =>
                        {
                            columnDescriptor.Spacing(20);

                            if (file is not null)
                            {
                                columnDescriptor.Item().Image(file);
                            }

                            columnDescriptor.Item().Text(documentTitle).FontSize(24).FontColor(Colors.Black);
                            columnDescriptor.Item().Text(description).FontColor(Colors.Black);
                            columnDescriptor.Item().Image(bytesQR);
                        });

                    page.Footer()
                        .AlignCenter()
                        .Text(textDescriptor => { textDescriptor.Span(footer); });
                });
            })
            .GeneratePdf();

        return pdfBytes;
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