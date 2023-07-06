namespace ConsoleEF.SearchFramework;

using System.Linq.Expressions;

public class GreaterThanOrEqualToSearchExpression<TMember> : ISearchExpression
{
    public TMember? Value { get; set; }

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