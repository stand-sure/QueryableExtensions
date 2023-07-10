#nullable enable
namespace SearchFramework.JsonConverters;

using System.Text.Json;
using System.Text.Json.Serialization;

using SearchFramework.SortOrder;

public class SortOrderDirectiveJsonConverter : JsonConverterFactory
{
    public override bool CanConvert(Type typeToConvert)
    {
        return typeToConvert.IsGenericType && typeToConvert.GetGenericTypeDefinition() == typeof(SortOrderDirective<>);
    }
    
    public override JsonConverter CreateConverter(Type typeToConvert, JsonSerializerOptions options)
    {
        Type paramType = typeToConvert.GetGenericArguments().Single();
        Type converterType = typeof(ConverterInternal<>).MakeGenericType(paramType);

        var converter = Activator.CreateInstance(converterType) as JsonConverter;

        return converter!;
    }

    private sealed class ConverterInternal<T> : JsonConverter<SortOrderDirective<T>>
    {
        public override SortOrderDirective<T> Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            string? value = reader.GetString();

            SortOrderDirection direction = Enum.TryParse(value, true, out SortOrderDirection temp) ? temp : SortOrderDirection.Ascending;

            return new SortOrderDirective<T> { Direction = direction };
        }

        public override void Write(Utf8JsonWriter writer, SortOrderDirective<T> value, JsonSerializerOptions options)
        {
            JsonSerializer.Serialize(writer, value.Direction.ToString(), typeof(string), options);
        }
    }
}