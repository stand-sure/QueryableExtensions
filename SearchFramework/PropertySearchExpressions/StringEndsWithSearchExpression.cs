namespace ConsoleEF.SearchFramework.PropertySearchExpressions;

using System.Linq.Expressions;
using System.Reflection;

using ConsoleEF.SearchFramework.SearchCriteria;

public class StringEndsWithSearchExpression : ISearchExpression
{
    public string? Value { get; init; } = string.Empty;

    Expression ISearchExpression.GetExpression(MemberExpression memberExpression)
    {
        MethodInfo method = typeof(string).GetMethod(nameof(string.EndsWith), new[] { typeof(string) })!;

        ConstantExpression constant = Expression.Constant(this.Value);
        MethodCallExpression methodCall = Expression.Call(memberExpression, method, constant);

        return methodCall;
    }

    public static implicit operator StringEndsWithSearchExpression(SearchValue<string> searchValue)
    {
        return new StringEndsWithSearchExpression { Value = searchValue.Value };
    }
}