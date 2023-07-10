#nullable enable
namespace SearchFramework.JsonConverters;

using System.Text.Json;
using System.Text.Json.Serialization;

using SearchFramework.SearchCriteria;
using SearchFramework.SortOrder;

public class BooleanSearchCriteriaJsonConverter : JsonConverter<BooleanSearchCriteria>
{
    public override BooleanSearchCriteria? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        bool value = reader.GetBoolean();

        return new BooleanSearchCriteria { Value = value };
    }

    public override void Write(Utf8JsonWriter writer, BooleanSearchCriteria value, JsonSerializerOptions options)
    {
        JsonSerializer.Serialize(writer, value.Value, value.Value?.GetType() ?? typeof(object), options);
    }
}