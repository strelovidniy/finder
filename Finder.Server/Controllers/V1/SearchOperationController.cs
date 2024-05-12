using Finder.Data.Enums;
using Finder.Data.Enums.RichEnums;
using Finder.Domain.Attributes;
using Finder.Domain.Models;
using Finder.Domain.Models.Create;
using Finder.Domain.Models.Update;
using Finder.Domain.Services.Abstraction;
using Finder.Server.Controllers.Base;
using Microsoft.AspNetCore.Mvc;

namespace Finder.Server.Controllers.V1;

[RouteV1("search-operations")]
public class SearchOperationController(
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

        await ValidateAsync(model, cancellationToken);

        await searchOperationService.CreateSearchOperationAsync(
            model,
            cancellationToken
        );

        return Ok();
    }

    [HttpPost("apply")]
    public async Task<IActionResult> ApplyForSearchOperationAsync(
        [FromQuery] Guid searchOperationId,
        CancellationToken cancellationToken = default
    )
    {
        await searchOperationService.ApplyForSearchOperationAsync(
            searchOperationId,
            cancellationToken
        );

        return Ok();
    }

    [HttpPost("operations-locations")]
    public async Task<IActionResult> AddSearchLocation(
        [FromBody] AddLocationsRequest model,
        CancellationToken cancellationToken = default
    )
    {
        await searchOperationService.AddLocationsToSearchOperationAsync(
            model.SearchOperationId,
            model.LocationRequests,
            cancellationToken
        );

        return Ok();
    }

    [HttpPost("create-chat")]
    public async Task<IActionResult> CreateChat(
        [FromQuery] Guid searchOperationId,
        CancellationToken cancellationToken = default
    )
    {
        await userAccessService.CheckIfUserCanCreateSearchOperationsAsync(cancellationToken);

        return Ok(await searchOperationService.CreateChatBySearchOperationAsync(searchOperationId, cancellationToken));
    }

    [HttpPost("confirm-operation")]
    public async Task<IActionResult> ConfirmOperation(
        [FromQuery] Guid searchOperationId,
        CancellationToken cancellationToken = default
    )
    {
        await userAccessService.CheckIfUserCanCreateSearchOperationsAsync(cancellationToken);

        await searchOperationService.ConfirmSearchOperationAsync(searchOperationId, cancellationToken);

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
        await searchOperationService.DeleteSearchOperationAsync(
            id,
            cancellationToken
        );

        return Ok();
    }

    [HttpPut("update")]
    public async Task<IActionResult> UpdateSearchOperationAsync(
        CancellationToken cancellationToken = default
    )
    {
        var model = ParseFormDataToUpdateHelpRequestModel(Request.Form);

        await ValidateAsync(model, cancellationToken);

        await searchOperationService.UpdateSearchOperationAsync(model, cancellationToken);

        return Ok();
    }

    [HttpGet("generate-qr")]
    public IActionResult GenerateQrCode(
        [FromQuery] Guid id,
        CancellationToken cancellationToken = default
    ) => File(
        searchOperationService.GenerateSearchOperationQr(id, cancellationToken),
        ContentType.ImagePng,
        "qr.png"
    );

    [HttpGet("generate-pdf")]
    public async Task<IActionResult> GeneratePdf(
        [FromQuery] Guid id,
        CancellationToken cancellationToken = default
    ) => File(
        await searchOperationService.GetSearchOperationPdfAsync(id, cancellationToken),
        ContentType.ApplicationPdf
    );


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

        var operationStatusString = form["operationStatus"].ToString();

        var operationStatus = Enum.TryParse<SearchOperationStatus>(operationStatusString, out var parsedStatusType)
            ? parsedStatusType
            : default;

        return new UpdateSearchOperationRequestModel(
            id,
            title,
            description,
            tags,
            operationType,
            operationStatus,
            showInfo,
            images,
            imageToDeleteIds
        );
    }
}