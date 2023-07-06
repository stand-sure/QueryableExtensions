namespace ConsoleEF.SearchFramework;

using System.Linq.Expressions;

public class AndSearchExpression<T> : ISearchExpression
{
    public IEnumerable<ComparableSearchExpression<T>>? Expressions { get; init; }

    Expression ISearchExpression.GetExpression(MemberExpression memberExpression)
    {
        if (this.Expressions is null || !this.Expressions.Any())
        {
            return Expression.Constant(false);
        }

        return this.Expressions.Aggregate(((Expression)Expression.Constant(true)),
            (agg, next) => Expression.And(agg, (next as ISearchExpression).GetExpression(memberExpression)));
    }

    public static implicit operator AndSearchExpression<T>(ComparableSearchExpression<T>[] clauses)
    {
        return new AndSearchExpression<T>
        {
            Expressions = new List<ComparableSearchExpression<T>>(clauses),
        };
    }
}