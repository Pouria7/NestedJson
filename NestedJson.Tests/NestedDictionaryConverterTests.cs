using System.Text.Json;

namespace NestedJson.Tests;

[TestFixture]
public class NestedDictionaryConverterTests
{

    [Test]
    public void Read_SimpleValue_ReturnsNestedDictionaryWithLastValue()
    {
        // Arrange
        string json = "\"test value\"";

        // Act
        var result = JsonSerializer.Deserialize<NestedDictionary<string>>(json);

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual("test value", result.LastValue);
        Assert.AreEqual(0, result.Count);
    }

    [Test]
    public void Read_NestedDictionary_ReturnsNestedDictionary()
    {
        // Arrange
        string json = "{\"key1\":{\"key2\":\"value\"}}";

        // Act
        var result = JsonSerializer.Deserialize<NestedStringDictionary>(json);

        // Assert
        Assert.IsNotNull(result);
        Assert.IsNull(result.LastValue);
        Assert.AreEqual(1, result.Count);
        Assert.IsTrue(result.ContainsKey("key1"));
        Assert.AreEqual("value", result["key1"]["key2"].LastValue);
    }

    [Test]
    public void Read_NullValue_ReturnsNull()
    {
        // Arrange
        string json = "null";

        // Act
        var result = JsonSerializer.Deserialize<NestedDictionary<string>>(json);

        // Assert
        Assert.IsNull(result);
    }

    [Test]
    public void Write_SimpleValue_WritesValueDirectly()
    {
        // Arrange
        var dictionary = new NestedDictionary<string> { LastValue = "test value" };

        // Act
        string json = JsonSerializer.Serialize(dictionary);

        // Assert
        Assert.AreEqual("\"test value\"", json);
    }

    [Test]
    public void Write_NestedDictionary_WritesNestedStructure()
    {
        // Arrange
        var dictionary = new NestedDictionary<string>();
        var nestedDict = new NestedDictionary<string> { LastValue = "nested value" };
        dictionary["key"] = nestedDict;

        // Act
        string json = JsonSerializer.Serialize(dictionary);

        // Assert
        Assert.AreEqual("{\"key\":\"nested value\"}", json);
    }

    [Test]
    public void Write_DictionaryWithLastValue_WritesNestedStructureWithDefault()
    {
        // Arrange
        var dictionary = new NestedDictionary<string>
        {
            LastValue = "default value"
        };
        dictionary["key"] = new NestedDictionary<string> { LastValue = "nested value" };

        // Act
        string json = JsonSerializer.Serialize(dictionary);

        // Assert
        // The converter should add a DEFAULT key with the LastValue
        Assert.AreEqual("{\"key\":\"nested value\",\"DEFAULT\":\"default value\"}", json);
    }

    [Test]
    public void Write_ComplexNestedStructure_MaintainsStructure()
    {
        // Arrange
        var dictionary = new NestedDictionary<string>();
        var level1 = new NestedDictionary<string>();
        var level2 = new NestedDictionary<string> { LastValue = "deep value" };

        level1["level2"] = level2;
        dictionary["level1"] = level1;

        // Act
        string json = JsonSerializer.Serialize(dictionary);
        var deserialized = JsonSerializer.Deserialize<NestedDictionary<string>>(json);

        // Assert
        Assert.IsNotNull(deserialized);
        Assert.IsTrue(deserialized.ContainsKey("level1"));
        Assert.IsTrue(deserialized["level1"].ContainsKey("level2"));
        Assert.AreEqual("deep value", deserialized["level1"]["level2"].LastValue);
    }

    [Test]
    public void ReadWrite_RoundTrip_PreservesData()
    {
        // Arrange
        var original = new NestedDictionary<int>();
        original["a"] = new NestedDictionary<int> { LastValue = 1 };
        original["b"] = new NestedDictionary<int>();
        original["b"]!["c"] = new NestedDictionary<int> { LastValue = 2 };
        original.LastValue = 3;

        // Act
        string json = JsonSerializer.Serialize(original);

        var deserialized = JsonSerializer.Deserialize<NestedDictionary<int>>(json);

        // Assert
        Assert.IsNotNull(deserialized);
        Assert.AreEqual(3, deserialized["DEFAULT"].LastValue);
        Assert.AreEqual(1, deserialized["a"].LastValue);
        Assert.AreEqual(2, deserialized["b"]["c"].LastValue);
    }
}