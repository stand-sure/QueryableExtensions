namespace ConsoleEF.SearchFramework.PropertySearchExpressions;

using System.Linq.Expressions;

using ConsoleEF.SearchFramework.SearchCriteria;

public class NotEqualToSearchExpression<TMember> : ISearchExpression
{
    private TMember? Value { get; init; }

    Expression ISearchExpression.GetExpression(MemberExpression memberExpression)
    {
        Expression constantExpression = Expression.Constant(this.Value);

        return Expression.NotEqual(memberExpression, constantExpression);
    }

    public static implicit operator NotEqualToSearchExpression<TMember>(SearchValue<TMember> searchValue)
    {
        return new NotEqualToSearchExpression<TMember> { Value = searchValue.Value };
    }
}