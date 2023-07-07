#nullable enable
namespace SearchFramework.SearchCriteria;

using System.Text.Json.Serialization;

using JetBrains.Annotations;

using SearchFramework.JsonConverters;
using SearchFramework.TypeSearchExpressions;

[PublicAPI]
[JsonConverter(typeof(BooleanSearchCriteriaJsonConverter))]
public class BooleanSearchCriteria : ComparableSearchExpression<bool>
{
    private readonly SearchValue<bool>? @value;

    public SearchValue<bool>? Value
    {
        get => this.value;
        init
        {
            this.value = value;

            if (this.value is not null)
            {
                this.EqualToSearchExpression = this.value;
            }
        }
    }

    public static implicit operator BooleanSearchCriteria(bool value)
    {
        return new BooleanSearchCriteria { Value = value };
    }
}