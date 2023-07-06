namespace ConsoleEF.SearchFramework;

using System.Linq.Expressions;

public class NotEqualToSearchExpression<TMember> : ISearchExpression
{
    public TMember? Value { get; set; }

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