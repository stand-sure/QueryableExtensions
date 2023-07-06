namespace ConsoleEF.SearchFramework;

using System.Linq.Expressions;

public class OrSearchExpression<T> : ISearchExpression
{
    Expression ISearchExpression.GetExpression(MemberExpression memberExpression)
    {
        if (this.Expressions is null || !this.Expressions.Any())
        {
            return Expression.Constant(false);
        }

        return this.Expressions.Select(e => (e as ISearchExpression).GetExpression(memberExpression)).Aggregate(
            (agg, next) => Expression.OrElse(agg, (next)));
    }

    public IEnumerable<ComparableSearchExpression<T>>? Expressions { get; init; }

    public static implicit operator OrSearchExpression<T>(ComparableSearchExpression<T>[] clauses)
    {
        return new OrSearchExpression<T>
        {
            Expressions = new List<ComparableSearchExpression<T>>(clauses),
        };
    }
}