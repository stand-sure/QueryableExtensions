namespace ConsoleEF.SearchFramework;

using System.Linq.Expressions;

public class GreaterThanSearchExpression<TMember> : ISearchExpression
{
    public TMember? Value { get; set; }

    Expression ISearchExpression.GetExpression(MemberExpression memberExpression)
    {
        Expression constantExpression = Expression.Constant(this.Value);

        return Expression.GreaterThan(memberExpression, constantExpression);
    }

    public static implicit operator GreaterThanSearchExpression<TMember>(SearchValue<TMember> searchValue)
    {
        return new GreaterThanSearchExpression<TMember>() { Value = searchValue.Value };
    }
}