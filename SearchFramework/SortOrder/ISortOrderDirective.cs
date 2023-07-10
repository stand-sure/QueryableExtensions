#nullable enable
namespace SearchFramework.SortOrder;

public interface ISortOrderDirective
{
    public SortOrderDirection Direction { get; set; }

    IOrderedQueryable<TSource> Apply<TSource>(IQueryable<TSource> queryable, string propertyName);
}