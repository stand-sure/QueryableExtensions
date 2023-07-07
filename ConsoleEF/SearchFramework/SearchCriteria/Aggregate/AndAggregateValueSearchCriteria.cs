namespace ConsoleEF.SearchFramework.SearchCriteria.Aggregate;

using System.Linq.Expressions;

public class AndAggregateValueSearchCriteria<TSearchCriteria, TSource>
    where TSearchCriteria : SearchCriteriaBase<TSource>
{
    public IEnumerable<TSearchCriteria>? Criteria { get; init; }

    public static implicit operator Expression<Func<TSource, bool>>(AndAggregateValueSearchCriteria<TSearchCriteria, TSource> searchCriteria)
    {
        return searchCriteria.GetPredicate();
    }

    public static implicit operator AndAggregateValueSearchCriteria<TSearchCriteria, TSource>(TSearchCriteria[] criteria)
    {
        return new AndAggregateValueSearchCriteria<TSearchCriteria, TSource> { Criteria = criteria };
    }

    private Expression<Func<TSource, bool>> GetPredicate()
    {
        if (this.Criteria == null || this.Criteria.Any() is false)
        {
            return source => true;
        }

        ParameterExpression parameter = Expression.Parameter(typeof(TSource), "s");

        IEnumerable<Expression<Func<TSource, bool>>> lambdas = this.GetExpressionsAndReplaceParameter(parameter);

        Expression union = lambdas.Select(expression => expression.Body)
            .Aggregate((agg, next) => Expression.AndAlso(agg, next).Reduce());

        return Expression.Lambda<Func<TSource, bool>>(union, parameter);
    }

    private IEnumerable<Expression<Func<TSource, bool>>> GetExpressionsAndReplaceParameter(ParameterExpression parameter)
    {
        IEnumerable<Expression<Func<TSource, bool>>> lambdas = this.Criteria!.Select(criteria => (Expression<Func<TSource, bool>>)criteria);

        var replacer = new ParameterReplacingExpressionVisitor(parameter);

        return lambdas.Select(expression => replacer.VisitAndConvert(expression, null));
    }
}