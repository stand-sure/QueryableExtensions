#nullable enable

namespace SearchFramework.SearchCriteria;

using System.Text.Json.Serialization;

using JetBrains.Annotations;

using SearchFramework.JsonConverters;

[PublicAPI]
[JsonConverter(typeof(SearchValueJsonConverter))]
public record SearchValue<TMember>
{
    public TMember? Value { get; set; }

    public static implicit operator SearchValue<TMember>(TMember value)
    {
        return new SearchValue<TMember> { Value = value };
    }
}