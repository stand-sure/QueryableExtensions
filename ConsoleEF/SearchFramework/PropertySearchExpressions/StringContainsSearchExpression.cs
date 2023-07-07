namespace ConsoleEF.SearchFramework.PropertySearchExpressions;

using System.Linq.Expressions;
using System.Reflection;

using ConsoleEF.SearchFramework.SearchCriteria;

public class StringContainsSearchExpression : ISearchExpression
{
    private string? Value { get; init; } = string.Empty;

    Expression ISearchExpression.GetExpression(MemberExpression memberExpression)
    {
        MethodInfo method = typeof(string).GetMethod(nameof(string.Contains), new[] { typeof(string) })!;

        ConstantExpression constant = Expression.Constant(this.Value);
        MethodCallExpression methodCall = Expression.Call(memberExpression, method, constant);

        return methodCall;
    }

    public static implicit operator StringContainsSearchExpression(SearchValue<string> searchValue)
    {
        return new StringContainsSearchExpression { Value = searchValue.Value };
    }
}