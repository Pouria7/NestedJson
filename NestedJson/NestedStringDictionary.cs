
namespace NestedJson;

/// <summary>
/// A dictionary that supports nested key-value pairs with an optional terminal string value.
/// </summary>
public class NestedStringDictionary : NestedDictionary<string?>
{
    public NestedStringDictionary(){}
    public NestedStringDictionary(IDictionary<string, NestedDictionary<string?>> dictionary) : base(dictionary)
    {
    }

    public static implicit operator NestedStringDictionary(string d) =>
        new() { LastValue = d };
}