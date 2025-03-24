# NestedJson

A lightweight C# library for working with nested dictionaries that support terminal values, JSON serialization, and optional dot notation conversion.

## Features
- **Nested Dictionaries:** Create hierarchical structures with terminal values.
- **JSON Serialization:** Seamless serialization and deserialization using `System.Text.Json`.
- **String Specialization:** `NestedStringDictionary` with implicit conversion from strings.
- **Generic Support:** `NestedDictionary<T>` for any data type.
- **Dot Notation (Optional):** Convert flat key-value pairs to nested structures and vice versa via extension methods.
- **Debugger Friendly:** Enhanced debugging experience with `DebuggerDisplay` showing count and last value.

## Installation
Clone the repository and build the solution:
```bash
git clone https://github.com/Pouria7/NestedJson.git
cd NestedJson
dotnet build
```

# Usage
## Basic Example
```csharp
using NestedJson;

// Using NestedStringDictionary with implicit conversion
NestedStringDictionary dict = "root";
dict["a"] = NestedStringDictionary.Create("nested");

// Serialize to JSON
string json = JsonSerializer.Serialize(dict);
Console.WriteLine(json); // {"a":"nested","DEFAULT":"root"}
```

## Generic Example
```csharp
using NestedJson;

// Using NestedDictionary<T>
var intDict = NestedDictionary<int>.Create(42);
intDict["x"] = NestedDictionary<int>.Create(100);

string json = JsonSerializer.Serialize(intDict);
Console.WriteLine(json); // {"x":100,"DEFAULT":42}
```

## Dot Notation Conversion
```csharp
using NestedJson.Extensions;

// Convert flat dictionary to nested
var flat = new Dictionary<string, string> { { "a.b.c", "value" } };
var nested = flat.ToNested();
Console.WriteLine(nested["a"]!["b"]!["c"]!.LastValue); // "value"
```

## License
This project is licensed under the MIT License.  
See the [LICENSE](LICENSE) file for more details.

## Author
Made with ❤️ by [Pouria7](https://github.com/Pouria7)  
