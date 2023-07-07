namespace ConsoleEF.SearchFramework.TypeSearchExpressions;

using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;
using System.Reflection;

using ConsoleEF.SearchFramework.PropertySearchExpressions;

using JetBrains.Annotations;

public class ComparableSearchExpression<TMember> : ISearchExpression
{
    [SuppressMessage("Major Code Smell",
        "S3011:Reflection should not be used to increase accessibility of classes, methods, or fields",
        Justification = "Props are internal to avoid end-user fatigue.")]
    Expression ISearchExpression.GetExpression(MemberExpression memberExpression)
    {
        IEnumerable<(string name, ISearchExpression expression)> searchExpressions =
            from propertyInfo in this.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
            where propertyInfo.PropertyType.IsAssignableTo(typeof(ISearchExpression))
            let searchExpression = propertyInfo.GetValue(this) as ISearchExpression
            where searchExpression != null
            select (name: propertyInfo.Name, expression: searchExpression);

        searchExpressions = searchExpressions.ToList();

        Expression result = searchExpressions.Select(s => s.expression.GetExpression(memberExpression))
            .Aggregate(Expression.AndAlso);

        return result.Reduce();
    }

    internal AndSearchExpression<TMember>? AndSearchExpression { [UsedImplicitly] get; init; }
    internal EqualToSearchExpression<TMember>? EqualToSearchExpression { [UsedImplicitly] get; init; }

    internal GreaterThanOrEqualToSearchExpression<TMember>? GreaterThanOrEqualToSearchExpression { [UsedImplicitly] get; init; }

    internal GreaterThanSearchExpression<TMember>? GreaterThanSearchExpression { [UsedImplicitly] get; init; }

    internal InSearchExpression<TMember>? InSearchExpression { [UsedImplicitly] get; init; }

    internal LessThanOrEqualToSearchExpression<TMember>? LessThanOrEqualToSearchExpression { [UsedImplicitly] get; init; }

    internal LessThanSearchExpression<TMember>? LessThanSearchExpression { [UsedImplicitly] get; init; }

    internal NotEqualToSearchExpression<TMember>? NotEqualToSearchExpression { [UsedImplicitly] get; init; }

    internal NotInSearchExpression<TMember>? NotInSearchExpression { [UsedImplicitly] get; init; }
    
    internal OrSearchExpression<TMember>? OrSearchExpression { [UsedImplicitly] get; init; }
}