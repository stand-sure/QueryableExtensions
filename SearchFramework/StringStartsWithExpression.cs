namespace ConsoleEF.SearchFramework;

using System.Linq.Expressions;
using System.Reflection;

public class StringStartsWithExpression : ISearchExpression
{
    public string? Value { get; init; } = string.Empty;

    Expression ISearchExpression.GetExpression(MemberExpression memberExpression)
    {
        MethodInfo method = typeof(string).GetMethod(nameof(string.StartsWith), new[] { typeof(string) })!;

        ConstantExpression constant = Expression.Constant(this.Value);
        MethodCallExpression methodCall = Expression.Call(memberExpression, method, constant);

        return methodCall;
    }

    public static implicit operator StringStartsWithExpression(SearchValue<string> searchValue)
    {
        return new StringStartsWithExpression { Value = searchValue.Value };
    }
}