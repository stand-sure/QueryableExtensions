#nullable enable

namespace SearchFramework.PropertySearchExpressions;

using System.Linq.Expressions;
using System.Reflection;

using SearchFramework.SearchCriteria;

public class InSearchExpression<TSource> : ISearchExpression
{
    private IEnumerable<TSource?>? Values { get; init; }

    Expression ISearchExpression.GetExpression(MemberExpression memberExpression)
    {
        MethodInfo method = typeof(Enumerable).GetMethods().First(m => m.Name == "Contains" && m.GetParameters().Length == 2)
            .MakeGenericMethod(typeof(TSource));

        ConstantExpression constant = Expression.Constant(this.Values);
        MethodCallExpression methodCall = Expression.Call(null, method, constant, memberExpression);

        return methodCall;
    }

    public static implicit operator InSearchExpression<TSource>(SearchValues<TSource> searchValues)
    {
        return new InSearchExpression<TSource> { Values = searchValues.Values };
    }
}