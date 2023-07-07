namespace ConsoleEF.SearchFramework.PropertySearchExpressions;

using System.Linq.Expressions;

using ConsoleEF.SearchFramework.SearchCriteria;

public class EqualToSearchExpression<TMember> : ISearchExpression
{
    private TMember? Value { get; init; }

    Expression ISearchExpression.GetExpression(MemberExpression memberExpression)
    {
        Expression constantExpression = Expression.Constant(this.Value);

        return Expression.Equal(memberExpression, constantExpression);
    }

    public static implicit operator EqualToSearchExpression<TMember>(SearchValue<TMember> searchValue)
    {
        return new EqualToSearchExpression<TMember> { Value = searchValue.Value };
    }
}