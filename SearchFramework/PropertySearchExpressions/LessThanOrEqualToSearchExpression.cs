#nullable enable

namespace SearchFramework.PropertySearchExpressions;

using System.Linq.Expressions;

using SearchFramework.SearchCriteria;

public class LessThanOrEqualToSearchExpression<TMember> : ISearchExpression
{
    private TMember? Value { get; init; }

    Expression ISearchExpression.GetExpression(MemberExpression memberExpression)
    {
        Expression constantExpression = Expression.Constant(this.Value);

        return Expression.LessThanOrEqual(memberExpression, constantExpression);
    }

    public static implicit operator LessThanOrEqualToSearchExpression<TMember>(SearchValue<TMember> searchValue)
    {
        return new LessThanOrEqualToSearchExpression<TMember> { Value = searchValue.Value };
    }
}