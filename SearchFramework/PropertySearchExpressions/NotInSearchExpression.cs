namespace ConsoleEF.SearchFramework.PropertySearchExpressions;

using System.Linq.Expressions;
using System.Reflection;

using ConsoleEF.SearchFramework.SearchCriteria;

public class NotInSearchExpression<TSource> : ISearchExpression
{
    public IEnumerable<TSource?>? Values { get; init; }

    Expression ISearchExpression.GetExpression(MemberExpression memberExpression)
    {
        MethodInfo method = typeof(Enumerable).GetMethods().First(m => m.Name == "Contains" && m.GetParameters().Length == 2).MakeGenericMethod(typeof(TSource));

        ConstantExpression constant = Expression.Constant(this.Values);
        MethodCallExpression methodCall = Expression.Call(null, method, constant, memberExpression);

        UnaryExpression not = Expression.Not(methodCall);

        return not;
    }

    public static implicit operator NotInSearchExpression<TSource>(SearchValues<TSource> searchValues)
    {
        return new NotInSearchExpression<TSource> { Values = searchValues.Values };
    }
}