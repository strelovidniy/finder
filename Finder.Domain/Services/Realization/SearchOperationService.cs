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
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

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
        var searchOperation = await searchOperationRepository
            .Query()
            .Include(request => request.Images)
            .Include(request => request.Creator!.Details!.ContactInfo)
            .Include(op => op.UserApplications) 
            .FirstOrDefaultAsync(
                request => request.Id == id,
                cancellationToken
            );

        RuntimeValidator.Assert(searchOperation is not null, StatusCode.SearchOperationNotFound);
        RuntimeValidator.Assert(searchOperation!.DeletedAt is null, StatusCode.SearchOperationRemoved);

        return mapper.Map<SearchOperationView>(searchOperation);
    }

    public async Task CreateSearchOperationAsync(
        CreateSearchOperationRequestModel createsearchOperationModel,
        CancellationToken cancellationToken = default
    )
    {
        var currentUser = await currentUserService.GetCurrentUserAsync(cancellationToken);

        var addedSearchOperation = await searchOperationRepository.AddAsync(
            new SearchOperation()
            {
                Description = createsearchOperationModel.Description,
                CreatorUserId = currentUser.Id,
                ShowContactInfo = createsearchOperationModel.ShowContactInfo,
                Tags = createsearchOperationModel.Tags,
                OperationType = createsearchOperationModel.OperationType,
                Title = createsearchOperationModel.Title
            },
            cancellationToken
        );

        await searchOperationRepository.SaveChangesAsync(cancellationToken);

        await AddsearchOperationImagesAsync(
            createsearchOperationModel.Images,
            addedSearchOperation.Id,
            cancellationToken
        );
        
        await searchOperationNotificationService.NotifyAboutCreatingSearchOperationAsync(addedSearchOperation, cancellationToken);
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
                        && updatesearchOperationRequestModel.ImagesToDelete.Contains(searchOperationImage.Id)
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
            await AddsearchOperationImagesAsync(
                updatesearchOperationRequestModel.Images,
                searchOperation.Id,
                cancellationToken
            );
        }
        
        await searchOperationNotificationService.NotifyAboutUpdatingSearchOperationAsync(searchOperation, cancellationToken);
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
            searchOperations = searchOperations.Where(searchOperation => searchOperation.CreatorUserId == currentUser.Id);
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
            await searchOperationNotificationService.NotifyAboutApplicationReceivedAsync(operation, currentUser, cancellationToken);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
    }
    
    private async Task AddsearchOperationImagesAsync(
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

    public async Task<FileStreamResult> GetSearchOperationPdfAsync(
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

        if (searchOperation.Images?.FirstOrDefault() != null)
        {
            file = File.ReadAllBytes(searchOperation.Images.FirstOrDefault()!.ImageUrl);
        }

        QuestPDF.Settings.License = LicenseType.Community;

        string title = "Увага! Пошукова операція";
        string documentTitle = searchOperation.Title;
        string description = searchOperation.Description;
        string footer = "Не будьте байдужими!";


        var pdfBytes = Document.Create(container =>
        {
            container.Page(page =>
            {
                page.Size(PageSizes.A4);
                page.Margin(2, Unit.Centimetre);
                page.PageColor(Colors.Red.Darken4);
                page.DefaultTextStyle(x => x.FontSize(20));
                page.DefaultTextStyle(x => x.FontColor(Colors.White));

                page.Header()
                    .Text(title)
                    .SemiBold().FontSize(36);

                page.Content()
                    .PaddingVertical(1, Unit.Centimetre)
                    .Column(x =>
                    {
                        x.Spacing(20);

                        if (file is not null)
                        {
                            x.Item().Image(file);
                        }   
                        x.Item().Text(documentTitle);
                        x.Item().Text(description);
                    });

                page.Footer()
                    .AlignCenter()
                    .Text(x =>
                    {
                        x.Span(footer);
                    });
            });
        })
        .GeneratePdf();


        MemoryStream ms = new MemoryStream(pdfBytes);
        return new FileStreamResult(ms, "application/pdf");
    }
}