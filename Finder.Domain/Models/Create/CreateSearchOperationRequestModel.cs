using Finder.Data.Enums;
using Microsoft.AspNetCore.Http;

namespace Finder.Domain.Models.Create;

public record CreateSearchOperationRequestModel(
    string Title,
    string Description,
    IEnumerable<string> Tags,
    bool ShowContactInfo,
    SearchOperationType OperationType,
    IEnumerable<IFormFile> Images
) : IValidatableModel;