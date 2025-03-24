namespace NestedJson.Extensions;

public static class DotNotationExtensions
{
    public static NestedStringDictionary ToNested(this Dictionary<string, string> values)
    {
        var result = new NestedStringDictionary();
        foreach (var (key, value) in values)
        {
            SetNestedValue(result, key.Split('.'), value);
        }

        return result;
    }

    public static NestedDictionary<T> ToNested<T>(this Dictionary<string, T> values)
    {
        var result = new NestedDictionary<T>();
        foreach (var (key, value) in values)
        {
            SetNestedValue(result, key.Split('.'), value);
        }

        return result;
    }

    private static void SetNestedValue<T>(NestedDictionary<T> current, string[] keyParts, T value)
    {
        for (int i = 0; i < keyParts.Length; i++)
        {
            var keyPart = keyParts[i];
            if (i == keyParts.Length - 1)
            {
                current![keyPart] = NestedDictionary<T>.Create(value);
            }
            else
            {
                if (!current!.ContainsKey(keyPart))
                {
                    current[keyPart] = new NestedDictionary<T>();
                }

                current = current[keyPart]!;
            }
        }
    }

    public static Dictionary<string, T> ToDotNotation<T>(this NestedDictionary<T> nested)
    {
        var result = new Dictionary<string, T>();
        ConvertToDotNotation(nested, "", result);
        return result;
    }

    private static void ConvertToDotNotation<T>(NestedDictionary<T> dict, string prefix, Dictionary<string, T> result)
    {
        foreach (var key in dict.Keys)
        {
            var newKey = string.IsNullOrEmpty(prefix) ? key : $"{prefix}.{key}";
            var value = dict[key];

            if (value == null || value.LastValue == null)
            {
                ConvertToDotNotation(value!, newKey, result);
            }
            else
            {
                if (key != NestedDictionary<T>.DefaultKey || dict.All(k => k.Key == NestedDictionary<T>.DefaultKey))
                    result[newKey] = value.LastValue!;
            }
        }
    }
}