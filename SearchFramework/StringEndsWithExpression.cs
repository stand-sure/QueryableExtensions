namespace ConsoleEF.SearchFramework;

using System.Linq.Expressions;
using System.Reflection;

public class StringEndsWithExpression : ISearchExpression
{
    public string? Value { get; init; } = string.Empty;

    Expression ISearchExpression.GetExpression(MemberExpression memberExpression)
    {
        MethodInfo method = typeof(string).GetMethod(nameof(string.EndsWith), new[] { typeof(string) })!;

        ConstantExpression constant = Expression.Constant(this.Value);
        MethodCallExpression methodCall = Expression.Call(memberExpression, method, constant);

        return methodCall;
    }

    public static implicit operator StringEndsWithExpression(SearchValue<string> searchValue)
    {
        return new StringEndsWithExpression { Value = searchValue.Value };
    }
}