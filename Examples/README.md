# Sasodoc Examples

This collection contains small, focused Sasodoc example projects. Each example is its own `.csproj`, so it can be opened and run independently.

## General format examples

- `01.Json.Basic` — primitive values and arrays with JSON.
- `02.Xml.Basic` — primitive values and arrays with XML.
- `03.Ini.Basic` — primitive values and arrays with INI.
- `04.Yaml.Basic` — primitive values and arrays with YAML.
- `05.Binary.Basic` — primitive values and arrays with Sasodoc Binary.

## Binary examples

- `06.Binary.Identifier.None` — sequential binary data without stored identifiers.
- `07.Binary.Identifier.Inline` — identifiers stored inline.
- `08.Binary.Identifier.Registry` — identifiers stored in the document registry/header.
- `09.Binary.Magics` — predefined `SDB`, `BIN`, `DAT`, `SASODOCBIN`, and `SASODOCDAT` magic values.
- `10.Binary.LengthEncodings` — available length encodings.
- `11.Binary.Async` — asynchronous save/load.
- `12.Binary.Arrays` — primitive array serialization.

## Other examples

- `13.Json.NamingConventions` — `FormatOptions` with kebab-case naming.

The project references assume the repository layout `Examples/<example>` and `Projects/<Sasodoc projects>`.
