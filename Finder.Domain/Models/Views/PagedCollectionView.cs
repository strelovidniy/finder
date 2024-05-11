namespace Finder.Domain.Models.Views;

public record PagedCollectionView<T>(
    IEnumerable<T> Items,
    int TotalCount
);