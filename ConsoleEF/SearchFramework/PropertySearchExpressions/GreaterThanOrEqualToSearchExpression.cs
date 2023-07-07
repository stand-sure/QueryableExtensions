namespace ConsoleEF.SearchFramework.PropertySearchExpressions;

using System.Linq.Expressions;

using ConsoleEF.SearchFramework.SearchCriteria;

public class GreaterThanOrEqualToSearchExpression<TMember> : ISearchExpression
{
    private TMember? Value { get; init; }

    Expression ISearchExpression.GetExpression(MemberExpression memberExpression)
    {
        Expression constantExpression = Expression.Constant(this.Value);

        return Expression.GreaterThanOrEqual(memberExpression, constantExpression);
    }

    public static implicit operator GreaterThanOrEqualToSearchExpression<TMember>(SearchValue<TMember> searchValue)
    {
        return new GreaterThanOrEqualToSearchExpression<TMember> { Value = searchValue.Value };
    }
}