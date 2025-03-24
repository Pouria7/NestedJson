using System.Diagnostics;
using System.Text.Json.Serialization;

namespace NestedJson;

/// <summary>
/// A dictionary that supports nested key-value pairs with an optional terminal (T) value.
/// </summary>
[JsonConverter(typeof(NestedDictionaryConverterFactory))]
[DebuggerDisplay("Count = {Count}, LastValue = {LastValue}")]
public class NestedDictionary<T> : Dictionary<string, NestedDictionary<T>?>
{
    public const string DefaultKey = "DEFAULT";

    public NestedDictionary()
    {
    }

    public NestedDictionary(IDictionary<string, NestedDictionary<T>> dictionary) : base(dictionary!)
    {
    }

    public NestedDictionary(NestedDictionary<T> dictionary) : base(dictionary!)
    {
    }

    /// <summary>
    /// Gets or sets the terminal value of this dictionary node.
    /// </summary>
    [JsonIgnore]
    public T? LastValue { get; set; }

    public NestedDictionary<T>? this[string key]
    {
        get => this.GetValueOrDefault(key, null!);
        set
        {
            if (ContainsKey(key))
            {
                this[key] = value;
            }
            else
            {
                this.Add(key, value);
            }
        }
    }

    public static NestedDictionary<T> Create(T value) => new() { LastValue = value };
}

