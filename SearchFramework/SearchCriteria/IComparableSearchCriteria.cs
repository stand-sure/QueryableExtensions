#nullable enable

namespace SearchFramework.SearchCriteria;

using JetBrains.Annotations;

[PublicAPI]
internal interface IComparableSearchCriteria<T>
{
    public SearchValue<T>? EqualTo { get; init; }
    public SearchValue<T>? GreaterThan { get; init; }
    public SearchValue<T>? GreaterThanOrEqualTo { get; init; }
    public SearchValue<T>? LessThan { get; init; }
    public SearchValue<T>? LessThanOrEqualTo { get; init; }

    public SearchValue<T>? NotEqualTo { get; init; }
}