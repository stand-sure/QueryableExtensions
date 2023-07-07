namespace ConsoleEF.SearchFramework.PropertySearchExpressions;

using System.Linq.Expressions;

using ConsoleEF.SearchFramework.SearchCriteria;

public class GreaterThanSearchExpression<TMember> : ISearchExpression
{
    private TMember? Value { get; init; }

    Expression ISearchExpression.GetExpression(MemberExpression memberExpression)
    {
        Expression constantExpression = Expression.Constant(this.Value);

        return Expression.GreaterThan(memberExpression, constantExpression);
    }

    public static implicit operator GreaterThanSearchExpression<TMember>(SearchValue<TMember> searchValue)
    {
        return new GreaterThanSearchExpression<TMember> { Value = searchValue.Value };
    }
}