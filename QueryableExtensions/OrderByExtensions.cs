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
        BinaryExpression notEqual = NotEqualToNull(keySelector);

        Expression<Func<TSource, bool>> lambda = notEqual.ToLambda(keySelector);

        return source.OrderBy(lambda).ThenByDescending(keySelector);
    }

    [MustUseReturnValue]
    public static IOrderedQueryable<TSource> OrderByDescendingNullsLast<TSource, TKey>(
        this IQueryable<TSource> source,
        Expression<Func<TSource, TKey>> keySelector)
    {
        BinaryExpression equal = EqualToNull(keySelector);

        Expression<Func<TSource, bool>> lambda = equal.ToLambda(keySelector);

        return source.OrderBy(lambda).ThenByDescending(keySelector);
    }

    [MustUseReturnValue]
    public static IOrderedQueryable<TSource> OrderByNullsFirst<TSource, TKey>(
        this IQueryable<TSource> source,
        Expression<Func<TSource, TKey>> keySelector)
    {
        BinaryExpression notEqual = NotEqualToNull(keySelector);

        Expression<Func<TSource, bool>> lambda = notEqual.ToLambda(keySelector);

        return source.OrderBy(lambda).ThenBy(keySelector);
    }

    [MustUseReturnValue]
    public static IOrderedQueryable<TSource> OrderByNullsLast<TSource, TKey>(
        this IQueryable<TSource> source,
        Expression<Func<TSource, TKey>> keySelector)
    {
        BinaryExpression equal = EqualToNull(keySelector);

        Expression<Func<TSource, bool>> lambda = equal.ToLambda(keySelector);

        return source.OrderBy(lambda).ThenBy(keySelector);
    }

    private static BinaryExpression EqualToNull<TSource, TKey>(Expression<Func<TSource, TKey>> keySelector)
    {
        return Expression.Equal(keySelector.Body, Expression.Constant(null));
    }

    private static BinaryExpression NotEqualToNull<TSource, TKey>(Expression<Func<TSource, TKey>> keySelector)
    {
        return Expression.NotEqual(keySelector.Body, Expression.Constant(null));
    }

    private static Expression<Func<TSource, bool>> ToLambda<TSource, TKey>(this Expression expression, Expression<Func<TSource, TKey>> keySelector)
    {
        return Expression.Lambda<Func<TSource, bool>>(expression, keySelector.Parameters);
    }
}