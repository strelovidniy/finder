﻿using Finder.Data.Enums;
using Finder.Domain.Attributes;
using Finder.Domain.Models;
using Finder.Domain.Models.Create;
using Finder.Domain.Models.Update;
using Finder.Domain.Services.Abstraction;
using Finder.Server.Controllers.Base;
using Microsoft.AspNetCore.Mvc;

namespace Finder.Server.Controllers.V1;

[RouteV1("search-operations")]
public class SearchOperationsController(
    IServiceProvider services,
    IUserAccessService userAccessService,
    ISearchOperationService searchOperationService
) : BaseController(services)
{
    [HttpGet("get")]
    public async Task<IActionResult> GetSearchOperationAsync(
        [FromQuery] Guid id,
        CancellationToken cancellationToken = default
    ) => Ok(
        await searchOperationService.GetSearchOperationAsync(
            id,
            cancellationToken
        )
    );


    [HttpPost("create")]
    public async Task<IActionResult> CreateSearchOperationAsync(
        CancellationToken cancellationToken = default
    )
    {
        var model = ParseFormDataToCreateHelpRequestModel(Request.Form);

        await userAccessService.CheckIfUserCanCreateHelpRequestsAsync(cancellationToken);

        await ValidateAsync(model, cancellationToken);

        await searchOperationService.CreateSearchOperationAsync(model, cancellationToken);

        return Ok();
    }

    
    [HttpGet]
    public async Task<IActionResult> GetSearchOperationsAsync(
        [FromQuery] QueryParametersModel queryParametersModel,
        CancellationToken cancellationToken = default
    ) => Ok(
        await searchOperationService.GetSearchOperationsAsync(
            queryParametersModel,
            cancellationToken
        )
    );
    
   

    [HttpDelete("delete")]
    public async Task<IActionResult> DeleteSearchOperationAsync(
        [FromQuery] Guid id,
        CancellationToken cancellationToken = default
    )
    {
        await userAccessService.CheckIfUserCanCreateHelpRequestsAsync(cancellationToken);

        await searchOperationService.DeleteSearchOperationAsync(id, cancellationToken);

        return Ok();
    }

    
    
    [HttpPut("update")]
    public async Task<IActionResult> UpdateSearchOperationAsync(
        CancellationToken cancellationToken = default
    )
    {
        var model = ParseFormDataToUpdateHelpRequestModel(Request.Form);

        await userAccessService.CheckIfUserCanCreateHelpRequestsAsync(cancellationToken);

        await ValidateAsync(model, cancellationToken);

        await searchOperationService.UpdateSearchOperationAsync(model, cancellationToken);

        return Ok();
    }
    
    
    private static CreateSearchOperationRequestModel ParseFormDataToCreateHelpRequestModel(IFormCollection form)
    {
        // Parse basic fields
        var title = form["title"].ToString();
        var description = form["description"].ToString();

        var tags = form["tags"]
            .ToString()
            .Split(',', StringSplitOptions.RemoveEmptyEntries)
            .Select(tag => tag.Trim())
            .Where(tag => !string.IsNullOrWhiteSpace(tag));

        var operationTypeString = form["operationType"].ToString();
        var operationType = Enum.TryParse<SearchOperationType>(operationTypeString, out var parsedOperationType)
            ? parsedOperationType
            : default;

        var showInfo = bool.TryParse(form["showContactInfo"].ToString(), out var show) && show;

        var images = form.Files.GetFiles("images");

        return new CreateSearchOperationRequestModel(
            title,
            description,
            tags,
            showInfo,
            operationType,
            images
        );
    }

    private static UpdateSearchOperationRequestModel ParseFormDataToUpdateHelpRequestModel(IFormCollection form)
    {
        // Parse basic fields
        var id = Guid.Parse(form["id"].ToString());
        var title = form["title"].ToString();
        var description = form["description"].ToString();

        var tags = form["tags"]
            .ToString()
            .Split(',', StringSplitOptions.RemoveEmptyEntries)
            .Select(tag => tag.Trim())
            .Where(tag => !string.IsNullOrWhiteSpace(tag));

        var showInfo = bool.TryParse(form["showContactInfo"].ToString(), out var show) && show;

        var images = form.Files.GetFiles("images");

        var imageToDeleteIds = new List<Guid>();

        for (var i = 0;; i++)
        {
            var key = $"imagesToDelete[{i}]";

            if (!form.ContainsKey(key))
            {
                break;
            }

            var imageId = form[key];

            if (Guid.TryParse(imageId, out var imageToDeleteId))
            {
                imageToDeleteIds.Add(imageToDeleteId);
            }
        }

        var operationTypeString = form["operationType"].ToString();
        var operationType = Enum.TryParse<SearchOperationType>(operationTypeString, out var parsedOperationType)
            ? parsedOperationType
            : default;

        return new UpdateSearchOperationRequestModel(
            id,
            title,
            description,
            tags,
            operationType,
            showInfo,
            images,
            imageToDeleteIds
        );
    }
}