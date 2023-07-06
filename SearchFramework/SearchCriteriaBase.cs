namespace ConsoleEF.SearchFramework;

using System.Linq.Expressions;

public abstract class SearchCriteriaBase<TSource>
{
    public static implicit operator Expression<Func<TSource, bool>>(SearchCriteriaBase<TSource> searchCriteria)
    {
        return searchCriteria.GetPredicate();
    }

    private Expression<Func<TSource, bool>> GetPredicate()
    {
        IEnumerable<(string name, ISearchExpression expression)> searchExpressions =
            from propertyInfo in this.GetType().GetProperties()
            where propertyInfo.PropertyType.IsAssignableTo(typeof(ISearchExpression))
            let searchExpression = propertyInfo.GetValue(this) as ISearchExpression
            where searchExpression != null
            select (name: propertyInfo.Name, expression: searchExpression);

        searchExpressions = searchExpressions.ToList();

        if (searchExpressions.Any() is false)
        {
            return source => true;
        }

        ParameterExpression sourceParameter = Expression.Parameter(typeof(TSource), "s");

        IList<Expression> sourcePredicateLambdas = new List<Expression>();

        foreach ((string name, ISearchExpression searchExpression) in searchExpressions)
        {
            MemberExpression sourceProperty = Expression.Property(sourceParameter, name);

            Expression sourcePredicate = searchExpression.GetExpression(sourceProperty).Reduce();

            if (sourcePredicate is ConstantExpression { Value: true })
            {
                continue;
            }

            sourcePredicateLambdas.Add(sourcePredicate);
        }

        Expression intersectionExpression = sourcePredicateLambdas.Aggregate(Expression.AndAlso).Reduce();

        Expression<Func<TSource, bool>> result = Expression.Lambda<Func<TSource, bool>>(intersectionExpression, sourceParameter);

        return result;
    }
}