#nullable enable

namespace SearchFramework.PropertySearchExpressions;

using System.Linq.Expressions;

using SearchFramework.TypeSearchExpressions;

public class AndSearchExpression<T> : ISearchExpression
{
    Expression ISearchExpression.GetExpression(MemberExpression memberExpression)
    {
        if (this.Expressions is null || !this.Expressions.Any())
        {
            return Expression.Constant(false);
        }

        return this.Expressions.Aggregate((Expression)Expression.Constant(true),
            (agg, next) => Expression.AndAlso(agg, (next as ISearchExpression).GetExpression(memberExpression)));
    }

    private IEnumerable<ComparableSearchExpression<T>>? Expressions { get; init; }

    public static implicit operator AndSearchExpression<T>(ComparableSearchExpression<T>[] clauses)
    {
        return new AndSearchExpression<T>
        {
            Expressions = new List<ComparableSearchExpression<T>>(clauses),
        };
    }
}