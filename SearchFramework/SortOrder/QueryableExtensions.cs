#nullable enable
namespace SearchFramework.SortOrder;

public static class QueryableExtensions {
    public static IQueryable<T> ApplySort<T>(this IQueryable<T> queryable, IEnumerable<SortOrderBase<T>> sortOrders)
    {
        return sortOrders.Aggregate(queryable, (current, sortOrder) => sortOrder.Apply(current));
    }
}