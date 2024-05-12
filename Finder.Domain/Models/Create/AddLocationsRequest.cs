namespace Finder.Domain.Models.Create;

public class AddLocationsRequest
{
    public Guid SearchOperationId { get; init; }

    public List<CreateSearchLocationRequestModel> LocationRequests { get; init; } = new();
}