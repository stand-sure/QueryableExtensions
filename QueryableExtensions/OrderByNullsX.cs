namespace ConsoleEF.QueryableExtensions;

using System.Linq.Expressions;

public static class OrderByNullsX
{
    public static IOrderedQueryable<TSource> OrderByNullsLast<TSource, TKey>(
        this IQueryable<TSource> source,
        Expression<Func<TSource, TKey>> keySelector)
    {
        BinaryExpression equal = Expression.Equal(keySelector.Body, Expression.Constant(null));
        Expression<Func<TSource, bool>> lambda = Expression.Lambda<Func<TSource, bool>>(equal, keySelector.Parameters);

        return source.OrderBy(lambda).ThenBy(keySelector);
    }

    public static IOrderedQueryable<TSource> OrderByNullsFirst<TSource, TKey>(
        this IQueryable<TSource> source,
        Expression<Func<TSource, TKey>> keySelector)
    {
        BinaryExpression notEqual = Expression.NotEqual(keySelector.Body, Expression.Constant(null));
        Expression<Func<TSource, bool>> lambda = Expression.Lambda<Func<TSource, bool>>(notEqual, keySelector.Parameters);

        return source.OrderBy(lambda).ThenBy(keySelector);
    }

    public static IOrderedQueryable<TSource> OrderByDescendingNullsLast<TSource, TKey>(
        this IQueryable<TSource> source,
        Expression<Func<TSource, TKey>> keySelector)
    {
        BinaryExpression equal = Expression.Equal(keySelector.Body, Expression.Constant(null));
        Expression<Func<TSource, bool>> lambda = Expression.Lambda<Func<TSource, bool>>(equal, keySelector.Parameters);

        return source.OrderBy(lambda).ThenByDescending(keySelector);
    }

    public static IOrderedQueryable<TSource> OrderByDescendingNullsFirst<TSource, TKey>(
        this IQueryable<TSource> source,
        Expression<Func<TSource, TKey>> keySelector)
    {
        BinaryExpression notEqual = Expression.NotEqual(keySelector.Body, Expression.Constant(null));
        Expression<Func<TSource, bool>> lambda = Expression.Lambda<Func<TSource, bool>>(notEqual, keySelector.Parameters);

        return source.OrderBy(lambda).ThenByDescending(keySelector);
    }
}