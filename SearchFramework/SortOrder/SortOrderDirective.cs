#nullable enable
namespace SearchFramework.SortOrder;

using System.Linq.Expressions;
using System.Text.Json.Serialization;

using SearchFramework.JsonConverters;

[JsonConverter(typeof(SortOrderDirectiveJsonConverter))]
public class SortOrderDirective<TProperty> : ISortOrderDirective
{
    public SortOrderDirection Direction { get; set; } = SortOrderDirection.Ascending;

    public static implicit operator SortOrderDirective<TProperty>(SortOrderDirection direction)
    {
        return new SortOrderDirective<TProperty> { Direction = direction };
    }

    private static Expression<Func<TSource, TProperty>> GetSelector<TSource>(string propertyName)
    {
        ParameterExpression parameter = Expression.Parameter(typeof(TSource));
        MemberExpression property = Expression.Property(parameter, propertyName);
        Expression<Func<TSource, TProperty>> lambda = Expression.Lambda<Func<TSource, TProperty>>(property, parameter);

        return lambda;
    }

    IOrderedQueryable<TSource> ISortOrderDirective.Apply<TSource>(IQueryable<TSource> queryable, string propertyName)
    {
        if (queryable.Expression.Type == typeof(IOrderedQueryable<TSource>))
        {
            return this.Direction == SortOrderDirection.Ascending ? (queryable as IOrderedQueryable<TSource>)!.ThenBy(GetSelector<TSource>(propertyName)) : (queryable as IOrderedQueryable<TSource>)!.ThenByDescending(GetSelector<TSource>(propertyName));
        }

        return this.Direction == SortOrderDirection.Ascending ? queryable.OrderBy(GetSelector<TSource>(propertyName)) : queryable.OrderByDescending(GetSelector<TSource>(propertyName));
    }
}