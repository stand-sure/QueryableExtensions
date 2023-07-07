#nullable enable

namespace SearchFramework.SearchCriteria;

using JetBrains.Annotations;

[PublicAPI]
public record SearchValues<TMember>
{
    public IEnumerable<TMember?>? Values { get; init; }
}