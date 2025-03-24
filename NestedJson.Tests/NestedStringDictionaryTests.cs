using NestedJson.Extensions;

namespace NestedJson.Tests;

[TestFixture]
public class NestedStringDictionaryTests
{

    [Test]
    public void ImplicitOperator_FromString_CreatesDictionaryWithLastValue()
    {
        // Arrange
        string input = "test-value";

        // Act
        NestedStringDictionary dict = input; // using implicit operator

        // Assert
        Assert.That(dict, Is.Not.Null);
        Assert.That(dict.LastValue, Is.EqualTo(input));
        Assert.That(dict.Count, Is.EqualTo(0)); // should not have any other keys
    }
    

    [Test]
    public void ToNested_ConvertsFlatDictionaryToNested()
    {
        // Arrange
        var flat = new Dictionary<string, string>
        {
            { "a.b.c", "value" }
        };
        
        // Act
        var nested = flat.ToNested();
        
        // Assert
        Assert.That(nested["a"]!["b"]!["c"]!.LastValue, Is.EqualTo("value"));
    }
}