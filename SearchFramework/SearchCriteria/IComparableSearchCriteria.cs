#nullable enable

namespace SearchFramework.SearchCriteria;

using JetBrains.Annotations;

[PublicAPI]
internal interface IComparableSearchCriteria<T>
{
    public SearchValue<T>? EqualTo { get; set; }
    public SearchValue<T>? GreaterThan { get; set; }
    public SearchValue<T>? GreaterThanOrEqualTo { get; set; }
    public SearchValue<T>? LessThan { get; set; }
    public SearchValue<T>? LessThanOrEqualTo { get; set; }

    public SearchValue<T>? NotEqualTo { get; set; }
}