#nullable enable

namespace SearchFramework.PropertySearchExpressions;

using System.Linq.Expressions;

using SearchFramework.SearchCriteria;

public class LessThanSearchExpression<TMember> : ISearchExpression
{
    private TMember? Value { get; init; }

    Expression ISearchExpression.GetExpression(MemberExpression memberExpression)
    {
        Expression constantExpression = Expression.Constant(this.Value);

        return Expression.LessThan(memberExpression, constantExpression);
    }

    public static implicit operator LessThanSearchExpression<TMember>(SearchValue<TMember> searchValue)
    {
        return new LessThanSearchExpression<TMember> { Value = searchValue.Value };
    }
}