namespace ConsoleEF.SearchFramework;

using System.Linq.Expressions;

public class LessThanOrEqualToSearchExpression<TMember> : ISearchExpression
{
    public TMember? Value { get; set; }

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