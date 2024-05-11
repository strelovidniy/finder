namespace Finder.Domain.Models;

public record QueryParametersModel(
    string? SearchQuery,
    string? SortBy,
    string? ExpandProperty,
    bool SortAscending = true,
    int PageNumber = 1,
    int PageSize = 10
);