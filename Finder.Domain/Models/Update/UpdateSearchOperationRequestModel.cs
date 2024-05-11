using Finder.Data.Enums;
using Microsoft.AspNetCore.Http;

namespace Finder.Domain.Models.Update;

public record UpdateSearchOperationRequestModel(
    Guid Id,
    string Title,
    string Description,
    IEnumerable<string> Tags,
    SearchOperationType OperationType,
    bool ShowContactInfo = false,
    IEnumerable<IFormFile>? Images = null,
    IEnumerable<Guid>? ImagesToDelete = null
) : IValidatableModel;