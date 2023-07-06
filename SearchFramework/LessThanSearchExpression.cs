namespace ConsoleEF.SearchFramework;

using System.Linq.Expressions;

public class LessThanSearchExpression<TMember> : ISearchExpression
{
    public TMember? Value { get; set; }

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