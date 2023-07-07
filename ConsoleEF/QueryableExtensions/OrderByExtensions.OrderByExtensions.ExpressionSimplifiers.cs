namespace ConsoleEF.QueryableExtensions;

using System.Linq.Expressions;

public static partial class OrderByExtensions
{
    private static Expression Convert(this Expression expression, Type type)
    {
        return Expression.Convert(expression, type);
    }

    private static BinaryExpression EqualTo(this Expression left, Expression right)
    {
        return Expression.Equal(left, right);
    }

    private static BinaryExpression EqualToNull<TSource, TKey>(this Expression<Func<TSource, TKey>> keySelector)
    {
        return Expression.Equal(keySelector.Body, Expression.Constant(null));
    }

    private static BinaryExpression NotEqualTo(this Expression left, Expression right)
    {
        return Expression.NotEqual(left, right);
    }

    private static BinaryExpression NotEqualToNull<TSource, TKey>(this Expression<Func<TSource, TKey>> keySelector)
    {
        return Expression.NotEqual(keySelector.Body, Expression.Constant(null));
    }

    private static Expression ToConstant(this object? value)
    {
        return Expression.Constant(value);
    }

    private static Expression<Func<TSource, bool>> ToLambda<TSource, TKey>(this Expression expression, Expression<Func<TSource, TKey>> keySelector)
    {
        return Expression.Lambda<Func<TSource, bool>>(expression, keySelector.Parameters);
    }
}