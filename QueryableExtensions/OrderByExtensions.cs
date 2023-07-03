namespace ConsoleEF.QueryableExtensions;

using System.Linq.Expressions;

using JetBrains.Annotations;

public static class OrderByExtensions
{
    public static IOrderedQueryable<TSource> OrderByEnumKey<TSource, TKey>(
        this IQueryable<TSource> source,
        Expression<Func<TSource, TKey?>> keySelector)
        where TKey : struct, Enum
    {
        IEnumerable<string> keys = Enum.GetNames(typeof(TKey)).OrderBy(key => key).Reverse();

        return OrderByEnumKeyImplementation(source, keySelector, keys);
    }

    public static IOrderedQueryable<TSource> OrderByEnumKeyDescending<TSource, TKey>(
        this IQueryable<TSource> source,
        Expression<Func<TSource, TKey?>> keySelector)
        where TKey : struct, Enum
    {
        IEnumerable<string> keys = Enum.GetNames(typeof(TKey)).OrderBy(key => key);

        return OrderByEnumKeyImplementation(source, keySelector, keys);
    }

    private static IOrderedQueryable<TSource> OrderByEnumKeyImplementation<TSource, TKey>(
        IQueryable<TSource> source,
        Expression<Func<TSource, TKey?>> keySelector,
        IEnumerable<string> keys)
        where TKey : struct, Enum
    {
        Expression memberExpression = keySelector.Body;

        IOrderedQueryable<TSource> orderedEnumerable = null!;

        var isFirst = true;
        Type? propType = Nullable.GetUnderlyingType(keySelector.GetType().GenericTypeArguments[0].GenericTypeArguments[1]);
        bool isNullable = propType is not null;
        
        propType = isNullable ? typeof(Nullable<>).MakeGenericType(propType!) : propType;

        foreach (string key in keys)
        {
            TKey? value = Enum.Parse<TKey>(key);

            Expression valueExpression = Expression.Constant(value);

            if (isNullable)
            {
                memberExpression = Expression.Convert(memberExpression, propType!);
                valueExpression = Expression.Convert(valueExpression, propType!);
            }

            BinaryExpression binaryExpression = Expression.Equal(memberExpression, valueExpression);

            Expression<Func<TSource, bool>> lambda = binaryExpression.ToLambda(keySelector);

            if (isFirst)
            {
                orderedEnumerable = source.OrderBy(lambda);
                isFirst = false;
            }
            else
            {
                orderedEnumerable = orderedEnumerable!.ThenBy(lambda);
            }
        }

        return orderedEnumerable;
    }

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