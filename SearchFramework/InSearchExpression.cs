namespace ConsoleEF.SearchFramework;

using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;

public class InSearchExpression<TSource> : ISearchExpression
{
    public IEnumerable<TSource?>? Values { get; set; }

    Expression ISearchExpression.GetExpression(MemberExpression memberExpression)
    {
        MethodInfo method = typeof(Enumerable).GetMethods().First(m => m.Name == "Contains" && m.GetParameters().Count() == 2).MakeGenericMethod(typeof(TSource));

        ConstantExpression constant = Expression.Constant(this.Values);
        MethodCallExpression methodCall = Expression.Call(null, method, constant, memberExpression);

        return methodCall;
    }

    public static implicit operator InSearchExpression<TSource>?(SearchValues<TSource> searchValues)
    {
        return new InSearchExpression<TSource> { Values = searchValues.Values };
    }
}