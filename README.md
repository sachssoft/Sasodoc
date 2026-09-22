# Sachssoft Sasodoc

Sasodoc is a lightweight, extensible and AOT-friendly document formatting and serialization library for .NET.

It provides a common API for reading and writing different document formats while keeping format-specific implementations separated into individual packages.

> [!WARNING]
> Sasodoc 2.0 introduces significant API and architectural changes compared to previous versions.
> Some format packages are still under active development and may receive breaking changes.

## Documentation

* **Package Build & Publish Guide**: See [PackageBuildInstructions.md](PackageBuildInstructions.md)
* **License**: See [LICENSE.md](LICENSE.md)
* **Changelog**: See [CHANGELOG.md](CHANGELOG.md)
* **Issues & Development**: See [GitHub Issues](https://github.com/sachssoft/Sasodoc/issues)

## Requirements

* .NET 8.0
* .NET 9.0
* .NET 10.0
* Trimming-compatible
* Native AOT-compatible

## Packages

> [!IMPORTANT]
> **Availability** and **stability** are separate.
>
> **Available** means that the package is published and can be used.
>
> **Stable** means that the implementation is considered sufficiently mature for regular use.
>
> **Unstable** packages are available for use but may still contain incomplete functionality, known issues, or undergo significant changes.
>
> See the [GitHub Issues](https://github.com/sachssoft/Sasodoc/issues) for known issues, planned improvements, and ongoing development.

| Package            | Usage                  | Availability | Stability | NuGet                                                                                                                            |
| ------------------ | ---------------------- | ------------ | --------- | -------------------------------------------------------------------------------------------------------------------------------- |
| **Sasodoc**        | Core document API      | Available    | Stable    | [![NuGet](https://img.shields.io/nuget/v/Sachssoft.Sasodoc.svg)](https://www.nuget.org/packages/Sachssoft.Sasodoc)               |
| **Sasodoc.Json**   | JSON document format   | Available    | Stable    | [![NuGet](https://img.shields.io/nuget/v/Sachssoft.Sasodoc.Json.svg)](https://www.nuget.org/packages/Sachssoft.Sasodoc.Json)     |
| **Sasodoc.Xml**    | XML document format    | Available    | Unstable  | [![NuGet](https://img.shields.io/nuget/v/Sachssoft.Sasodoc.Xml.svg)](https://www.nuget.org/packages/Sachssoft.Sasodoc.Xml)       |
| **Sasodoc.Ini**    | INI document format    | Available    | Unstable  | [![NuGet](https://img.shields.io/nuget/v/Sachssoft.Sasodoc.Ini.svg)](https://www.nuget.org/packages/Sachssoft.Sasodoc.Ini)       |
| **Sasodoc.Yaml**   | YAML document format   | Available    | Unstable  | [![NuGet](https://img.shields.io/nuget/v/Sachssoft.Sasodoc.Yaml.svg)](https://www.nuget.org/packages/Sachssoft.Sasodoc.Yaml)     |
| **Sasodoc.Binary** | Binary document format | Available    | Unstable  | [![NuGet](https://img.shields.io/nuget/v/Sachssoft.Sasodoc.Binary.svg)](https://www.nuget.org/packages/Sachssoft.Sasodoc.Binary) |

> [!NOTE]
> The stability status describes the current maturity of each package and may change independently of package availability.
> Check the [GitHub Issues](https://github.com/sachssoft/Sasodoc/issues) before production use, especially for packages marked **Unstable**.

## Features

* Common format-independent reader and writer API
* JSON document support
* XML document support
* INI document support
* YAML document support
* Binary document support
* Primitive values and arrays
* Nested objects
* Object arrays
* Custom value converters
* Configurable naming conventions
* Format-independent data identifiers
* Synchronous stream I/O
* Asynchronous stream I/O
* Trimming support
* Native AOT support
* Multi-targeting for .NET 8, .NET 9 and .NET 10
* Separate packages for individual document formats
* Low dependency overhead

## Document Formats

### JSON

JSON support is provided by `Sachssoft.Sasodoc.Json`.

JSON is one of the primary and stable document formats provided by Sasodoc.

```csharp
using Sachssoft.Documents.Formats.Json;
```

NuGet:

```text
Sachssoft.Sasodoc.Json
```

### XML

XML support is provided by `Sachssoft.Sasodoc.Xml`.

The XML formatter uses XML attributes for primitive properties and child elements for nested objects.

Example:

```xml
<Object name="Test" enabled="true" count="5">
    <transform x="10" y="20" />
</Object>
```

```csharp
using Sachssoft.Documents.Formats.Xml;
```

NuGet:

```text
Sachssoft.Sasodoc.Xml
```

### INI

INI support is provided by `Sachssoft.Sasodoc.Ini`.

Primitive values are represented as key/value pairs while nested objects can be represented through sections.

Example:

```ini
name=Test
enabled=true
count=5

[player]
name=Tobias

[player.position]
x=100
y=200
```

```csharp
using Sachssoft.Documents.Formats.Ini;
```

NuGet:

```text
Sachssoft.Sasodoc.Ini
```

### YAML

YAML support is provided by `Sachssoft.Sasodoc.Yaml`.

Example:

```yaml
name: Test
enabled: true

player:
  name: Tobias
  position:
    x: 100
    y: 200

numbers:
  - 10
  - 20
  - 30
```

```csharp
using Sachssoft.Documents.Formats.Yaml;
```

NuGet:

```text
Sachssoft.Sasodoc.Yaml
```

### Binary

Binary document support is provided by `Sachssoft.Sasodoc.Binary`.

The binary formatter provides multiple strategies for storing document identifiers and allows the binary representation to be configured depending on compatibility and file-size requirements.

Supported identifier modes include:

* No stored identifiers
* Inline identifiers
* Identifier registry

Identifier lengths can use fixed or variable-length encoding.

The binary format also requires a file magic value, allowing applications to identify and validate binary documents.

Common magic values include:

* `SDB`
* `BIN`
* `DAT`
* `SASODOCBIN`
* `SASODOCDAT`

Common file extensions include:

* `.sdb` — Sasodoc Binary
* `.bin`
* `.dat`

```csharp
using Sachssoft.Documents.Formats.Binary;
```

NuGet:

```text
Sachssoft.Sasodoc.Binary
```

## Architecture

Sasodoc separates the format-independent document API from individual document format implementations.

The core package provides the common reader, writer, formatting, naming and serialization infrastructure.

Format-specific implementations are distributed as separate packages.

```text
Sachssoft.Sasodoc
│
├── Sachssoft.Sasodoc.Json
├── Sachssoft.Sasodoc.Xml
├── Sachssoft.Sasodoc.Ini
├── Sachssoft.Sasodoc.Yaml
└── Sachssoft.Sasodoc.Binary
```

Applications only need to reference the formats they actually use.

## Data Identifiers

Sasodoc uses `DataIdentifier` to represent format-independent identifiers.

An identifier consists of a textual value and a semantic identifier kind.

This allows formats to distinguish regular properties from special identifiers without exposing format-specific key types through the common API.

```csharp
DataIdentifier identifier = new("name");
```

Strings can also be converted implicitly:

```csharp
DataIdentifier identifier = "name";
```

Special identifier kinds can be used when an identifier has semantic meaning and should not be handled like a regular property name.

## Naming Conventions

Sasodoc supports configurable naming conventions for properties and fields.

This allows application-side names to be converted into the naming style expected by a document format.

Examples include:

```text
PlayerName
playerName
player_name
player-name
```

Naming conversion is handled by the common Sasodoc infrastructure so individual format implementations can use the same naming behavior.

Semantic identifiers are preserved and are not treated as regular property names.

## Serialization

Sasodoc provides serialization infrastructure independently of individual document formats.

Serialization handlers can read and write objects through the common `FormatReaderBase` and `FormatWriterBase` APIs.

This allows the same serialization implementation to work with different document formats.

The serialization system favors explicit and predictable serialization behavior rather than requiring automatic runtime reflection.

## Native AOT and Trimming

Native AOT and trimming compatibility are important design goals of Sasodoc.

The projects enable the .NET trimming and AOT analyzers to detect incompatible APIs during development.

Sasodoc favors:

* Explicit serialization
* Explicit converters
* Predictable object creation
* Minimal runtime reflection
* AOT-compatible document processing
* Trimming-compatible APIs

Format implementations should avoid APIs that require runtime-generated serialization metadata unless appropriate metadata is explicitly supplied.

## Async I/O

Document formats can provide synchronous and asynchronous stream operations.

```csharp
writer.SaveTo(stream);
```

Asynchronous I/O:

```csharp
await writer.SaveToAsync(stream, cancellationToken);
```

Readers provide corresponding loading operations:

```csharp
reader.LoadFrom(stream);
```

and:

```csharp
await reader.LoadFromAsync(stream, cancellationToken);
```

Asynchronous APIs use actual asynchronous stream operations rather than wrapping synchronous I/O in `Task.Run`.

## Examples

Example projects are available in the [`Examples`](Examples) directory.

The examples demonstrate the individual document formats and important configuration options.

Current examples include:

* JSON basics
* XML basics
* INI basics
* YAML basics
* Binary basics
* Binary without identifiers
* Binary with inline identifiers
* Binary with identifier registry
* Binary magic values
* Binary length encodings
* Binary asynchronous I/O
* Binary arrays
* JSON naming conventions

Each example is provided as a separate project so it can be built and executed independently.

## Issues and Development

Known bugs, planned features and ongoing development are tracked through GitHub Issues:

[GitHub Issues](https://github.com/sachssoft/Sasodoc/issues)

When using a package marked **Unstable**, checking the related issues is recommended because its API or document representation may still change.

Bug reports and feature requests can also be submitted through the issue tracker.

## License

Sasodoc is licensed under the MIT License.

See [LICENSE.md](LICENSE.md) for details.
