namespace ConsoleEF.QueryableExtensions;

using System.Linq.Expressions;

using JetBrains.Annotations;

public static partial class OrderByExtensions
{
    [MustUseReturnValue]
    public static IOrderedQueryable<TSource> OrderByDescendingNullsFirst<TSource, TKey>(
        this IQueryable<TSource> source,
        Expression<Func<TSource, TKey>> keySelector)
    {
        BinaryExpression notEqual = keySelector.NotEqualToNull();

        Expression<Func<TSource, bool>> lambda = notEqual.ToLambda(keySelector);

        return source.OrderBy(lambda).ThenByDescending(keySelector);
    }

    [MustUseReturnValue]
    public static IOrderedQueryable<TSource> OrderByDescendingNullsLast<TSource, TKey>(
        this IQueryable<TSource> source,
        Expression<Func<TSource, TKey>> keySelector)
    {
        BinaryExpression equal = keySelector.EqualToNull();

        Expression<Func<TSource, bool>> lambda = equal.ToLambda(keySelector);

        return source.OrderBy(lambda).ThenByDescending(keySelector);
    }

    [MustUseReturnValue]
    public static IOrderedQueryable<TSource> OrderByNullsFirst<TSource, TKey>(
        this IQueryable<TSource> source,
        Expression<Func<TSource, TKey>> keySelector)
    {
        BinaryExpression notEqual = keySelector.NotEqualToNull();

        Expression<Func<TSource, bool>> lambda = notEqual.ToLambda(keySelector);

        return source.OrderBy(lambda).ThenBy(keySelector);
    }

    [MustUseReturnValue]
    public static IOrderedQueryable<TSource> OrderByNullsLast<TSource, TKey>(
        this IQueryable<TSource> source,
        Expression<Func<TSource, TKey>> keySelector)
    {
        BinaryExpression equal = keySelector.EqualToNull();

        Expression<Func<TSource, bool>> lambda = equal.ToLambda(keySelector);

        return source.OrderBy(lambda).ThenBy(keySelector);
    }
}