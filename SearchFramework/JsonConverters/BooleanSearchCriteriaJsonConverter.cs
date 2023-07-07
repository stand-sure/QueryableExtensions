#nullable enable
namespace SearchFramework.JsonConverters;

using System.Text.Json;
using System.Text.Json.Serialization;

using SearchFramework.SearchCriteria;

public class BooleanSearchCriteriaJsonConverter : JsonConverter<BooleanSearchCriteria>
{
    public override BooleanSearchCriteria? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        throw new NotImplementedException();
    }

    public override void Write(Utf8JsonWriter writer, BooleanSearchCriteria value, JsonSerializerOptions options)
    {
        JsonSerializer.Serialize(writer, value.Value, value.Value?.GetType() ?? typeof(object), options);
    }
}