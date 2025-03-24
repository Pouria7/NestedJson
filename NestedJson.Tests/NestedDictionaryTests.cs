
namespace NestedJson.Tests;

    [TestFixture]
    public class NestedDictionaryTests
    {
        [Test]
        public void DefaultConstructor_CreatesEmptyDictionary()
        {
            // Arrange & Act
            var dictionary = new NestedDictionary<string>();
            
            // Assert
            Assert.AreEqual(0, dictionary.Count);
            Assert.IsNull(dictionary.LastValue);
        }

        [Test]
        public void Constructor_WithDictionary_CopiesEntries()
        {
            // Arrange
            var innerDictionary = new NestedDictionary<int>();
            var sourceDictionary = new Dictionary<string, NestedDictionary<int>?>
            {
                { "key1", innerDictionary },
                { "key2", null }
            };
            
            // Act
            var dictionary = new NestedDictionary<int>(sourceDictionary);
            
            // Assert
            Assert.AreEqual(2, dictionary.Count);
            Assert.AreSame(innerDictionary, dictionary["key1"]);
            Assert.IsNull(dictionary["key2"]);
        }

        [Test]
        public void LastValue_SetAndGet_WorksCorrectly()
        {
            // Arrange
            var dictionary = new NestedDictionary<string>();
            
            // Act
            dictionary.LastValue = "test value";
            
            // Assert
            Assert.AreEqual("test value", dictionary.LastValue);
        }

        [Test]
        public void Create_WithValue_ReturnsNestedDictionaryWithLastValue()
        {
            // Arrange & Act
            var value = 42;
            var dictionary = NestedDictionary<int>.Create(value);
            
            // Assert
            Assert.AreEqual(0, dictionary.Count);
            Assert.AreEqual(value, dictionary.LastValue);
        }

        [Test]
        public void NestedDictionary_InheritsFromDictionary_SupportsAllDictionaryOperations()
        {
            // Arrange
            var dictionary = new NestedDictionary<string>();
            var nestedDict1 = new NestedDictionary<string>();
            var nestedDict2 = new NestedDictionary<string>();
            
            // Act
            dictionary.Add("level1", nestedDict1);
            dictionary["level2"] = nestedDict2;
            
            // Assert
            Assert.AreEqual(2, dictionary.Count);
            Assert.IsTrue(dictionary.ContainsKey("level1"));
            Assert.AreSame(nestedDict1, dictionary["level1"]);
            Assert.AreSame(nestedDict2, dictionary["level2"]);
        }

        [Test]
        public void DefaultKey_HasCorrectValue()
        {
            // Assert
            Assert.AreEqual("DEFAULT", NestedDictionary<string>.DefaultKey);
        }

        [Test]
        public void NestedDictionary_SupportsNullValues()
        {
            // Arrange
            var dictionary = new NestedDictionary<string>();
            
            // Act
            dictionary["nullKey"] = null;
            
            // Assert
            Assert.IsTrue(dictionary.ContainsKey("nullKey"));
            Assert.IsNull(dictionary["nullKey"]);
        }

        [Test]
        public void NestedDictionary_SupportsNestedStructure()
        {
            // Arrange
            var rootDict = new NestedDictionary<string>();
            var level1Dict = new NestedDictionary<string>();
            var level2Dict = NestedDictionary<string>.Create("leaf value");
            
            // Act
            rootDict["level1"] = level1Dict;
            level1Dict["level2"] = level2Dict;
            
            // Assert
            Assert.IsNotNull(rootDict["level1"]);
            Assert.IsNotNull(rootDict["level1"]!["level2"]);
            Assert.AreEqual("leaf value", rootDict["level1"]!["level2"]!.LastValue);
        }
    }

