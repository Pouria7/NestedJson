using System.Text.Json;
using System.Text.Json.Serialization;

namespace NestedJson;

public class NestedDictionaryConverter<T> : JsonConverter<NestedDictionary<T>>
{
    public override NestedDictionary<T>? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType == JsonTokenType.Null) return null;

        if (reader.TokenType != JsonTokenType.StartObject)
        {
            var value = JsonSerializer.Deserialize<T>(ref reader, options);
            if (value != null) return NestedDictionary<T>.Create(value);
        }

        var dictionary = JsonSerializer.Deserialize<Dictionary<string, NestedDictionary<T>?>>(ref reader, options);
        return dictionary == null ? null : new NestedDictionary<T>(dictionary!);
    }

    public override void Write(Utf8JsonWriter writer, NestedDictionary<T> value, JsonSerializerOptions options)
    {
        if (value.LastValue == null)
        {
            JsonSerializer.Serialize(writer, value as Dictionary<string, NestedDictionary<T>?>, options);
        }
        else if (!value.Any())
        {
            JsonSerializer.Serialize(writer, value.LastValue, options);
        }
        else
        {
            var temp = new Dictionary<string, NestedDictionary<T>?>(value)
            {
                ["DEFAULT"] = NestedDictionary<T>.Create(value.LastValue)
            };
            JsonSerializer.Serialize(writer, temp, options);
        }
    }
}

public class NestedDictionaryConverterFactory : JsonConverterFactory
{
    public override bool CanConvert(Type typeToConvert)
    {
        if (!typeToConvert.IsGenericType) return false;
        return typeToConvert.GetGenericTypeDefinition() == typeof(NestedDictionary<>);
    }

    public override JsonConverter? CreateConverter(Type typeToConvert, JsonSerializerOptions options)
    {
        Type genericType = typeToConvert.GetGenericArguments()[0];
        Type converterType = typeof(NestedDictionaryConverter<>).MakeGenericType(genericType);
        return (JsonConverter?)Activator.CreateInstance(converterType);
    }
}