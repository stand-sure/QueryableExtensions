namespace ConsoleEF.QueryableExtensions;

using System.Linq.Expressions;

public static partial class OrderByExtensions
{
    public static IOrderedQueryable<TSource> OrderByEnumKeyDescendingNullsFirst<TSource, TKey>(
        this IQueryable<TSource> source,
        Expression<Func<TSource, TKey?>> keySelector)
        where TKey : struct, Enum
    {
        IEnumerable<string> keys = Enum.GetNames(typeof(TKey)).OrderBy(key => key);

        return OrderByEnumKeyImplementation(source, keySelector, keys, true);
    }

    public static IOrderedQueryable<TSource> OrderByEnumKeyDescendingNullsLast<TSource, TKey>(
        this IQueryable<TSource> source,
        Expression<Func<TSource, TKey?>> keySelector)
        where TKey : struct, Enum
    {
        IEnumerable<string> keys = Enum.GetNames(typeof(TKey)).OrderBy(key => key).Reverse();

        return OrderByEnumKeyImplementation(source, keySelector, keys, false);
    }

    public static IOrderedQueryable<TSource> OrderByEnumKeyNullsFirst<TSource, TKey>(
        this IQueryable<TSource> source,
        Expression<Func<TSource, TKey?>> keySelector)
        where TKey : struct, Enum
    {
        IEnumerable<string> keys = Enum.GetNames(typeof(TKey)).OrderBy(key => key).Reverse();

        return OrderByEnumKeyImplementation(source, keySelector, keys, true);
    }

    public static IOrderedQueryable<TSource> OrderByEnumKeyNullsLast<TSource, TKey>(
        this IQueryable<TSource> source,
        Expression<Func<TSource, TKey?>> keySelector)
        where TKey : struct, Enum
    {
        IEnumerable<string> keys = Enum.GetNames(typeof(TKey)).OrderBy(key => key);

        return OrderByEnumKeyImplementation(source, keySelector, keys, false);
    }

    private static IOrderedQueryable<TSource> OrderByEnumKeyImplementation<TSource, TKey>(
        IQueryable<TSource> source,
        Expression<Func<TSource, TKey?>> keySelector,
        IEnumerable<string> keys,
        bool nullsFirst)
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

            BinaryExpression binaryExpression =
                nullsFirst ? Expression.Equal(memberExpression, valueExpression) : Expression.NotEqual(memberExpression, valueExpression);

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
}