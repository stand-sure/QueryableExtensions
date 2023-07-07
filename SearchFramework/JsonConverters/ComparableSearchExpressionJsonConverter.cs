#nullable enable

namespace SearchFramework.JsonConverters;

using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;

using SearchFramework.SearchCriteria;
using SearchFramework.TypeSearchExpressions;

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
public class ComparableSearchExpressionJsonConverter : JsonConverterFactory
{
    public override bool CanConvert(Type typeToConvert)
    {
        bool isComparableSearchExpression = typeToConvert.IsGenericType && typeToConvert.GetGenericTypeDefinition() == typeof(ComparableSearchExpression<>);

        return isComparableSearchExpression;
    }

    public override JsonConverter CreateConverter(Type typeToConvert, JsonSerializerOptions options)
    {
        Type paramType = typeToConvert.GetGenericArguments().Single();
        Type converterType = typeof(ConverterInternal<>).MakeGenericType(paramType);

        var converter = Activator.CreateInstance(converterType) as JsonConverter;

        return converter!;
    }

    private sealed class ConverterInternal<T> : JsonConverter<ComparableSearchExpression<T>>
    {
        public override ComparableSearchExpression<T> Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            throw new NotImplementedException();
        }

        public override void Write(Utf8JsonWriter writer, ComparableSearchExpression<T> value, JsonSerializerOptions options)
        {
            writer.WriteStartObject();

            foreach (PropertyInfo propertyInfo in value.GetType().GetProperties())
            {
                object? propValue = propertyInfo.GetValue(value);

                if (propValue is null)
                {
                    continue;
                }

                string propName = propertyInfo.Name;
                writer.WritePropertyName(options.PropertyNamingPolicy?.ConvertName(propName) ?? propName);

                JsonSerializer.Serialize(writer, propValue, propertyInfo.PropertyType, options);
            }

            writer.WriteEndObject();
        }
    }
}