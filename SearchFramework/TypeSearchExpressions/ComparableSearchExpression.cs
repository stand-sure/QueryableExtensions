#nullable enable

namespace SearchFramework.TypeSearchExpressions;

using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;
using System.Reflection;
using System.Text.Json.Serialization;

using SearchFramework.PropertySearchExpressions;

using JetBrains.Annotations;

using SearchFramework.JsonConverters;

[PublicAPI]
[JsonConverter(typeof(ComparableSearchExpressionJsonConverter))]
public class ComparableSearchExpression<TMember> : ISearchExpression
{
    [SuppressMessage("Major Code Smell",
        "S3011:Reflection should not be used to increase accessibility of classes, methods, or fields",
        Justification = "Props are internal to avoid end-user fatigue.")]
    Expression ISearchExpression.GetExpression(MemberExpression memberExpression)
    {
        IEnumerable<(string name, ISearchExpression expression)> searchExpressions =
            from propertyInfo in this.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
            where typeof(ISearchExpression).IsAssignableFrom(propertyInfo.PropertyType)
            let searchExpression = propertyInfo.GetValue(this) as ISearchExpression
            where searchExpression != null
            select (name: propertyInfo.Name, expression: searchExpression);

        searchExpressions = searchExpressions.ToList();

        if (searchExpressions.Any() is false)
        {
            return Expression.Constant(true);
        }

        Expression result = searchExpressions.Select(s => s.expression.GetExpression(memberExpression))
            .Aggregate(Expression.AndAlso);

        return result.Reduce();
    }

    internal AndSearchExpression<TMember>? AndSearchExpression { [UsedImplicitly] get; set; }
    internal EqualToSearchExpression<TMember>? EqualToSearchExpression { [UsedImplicitly] get; set; }

    internal GreaterThanOrEqualToSearchExpression<TMember>? GreaterThanOrEqualToSearchExpression { [UsedImplicitly] get; set; }

    internal GreaterThanSearchExpression<TMember>? GreaterThanSearchExpression { [UsedImplicitly] get; set; }

    internal InSearchExpression<TMember>? InSearchExpression { [UsedImplicitly] get; set; }

    internal LessThanOrEqualToSearchExpression<TMember>? LessThanOrEqualToSearchExpression { [UsedImplicitly] get; set; }

    internal LessThanSearchExpression<TMember>? LessThanSearchExpression { [UsedImplicitly] get; set; }

    internal NotEqualToSearchExpression<TMember>? NotEqualToSearchExpression { [UsedImplicitly] get; set; }

    internal NotInSearchExpression<TMember>? NotInSearchExpression { [UsedImplicitly] get; set; }

    internal OrSearchExpression<TMember>? OrSearchExpression { [UsedImplicitly] get; set; }
}