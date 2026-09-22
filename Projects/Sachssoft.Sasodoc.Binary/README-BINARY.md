# Sachssoft.Sasodoc.Binary

Initial Sasodoc Binary formatter implementation.

## File extensions

Binary documents may use `.bin` or `.dat`. `.sdb` is recommended for Sasodoc Binary documents.

## Magic

A non-empty magic value is mandatory. `BinaryOptions.Magic` defaults to `Magics.SDB` and throws for an empty value.
Predefined values: `SDB`, `BIN`, `DAT`, `SASODOCBIN`, `SASODOCDAT`.

## Identifier modes

- `None`: identifiers are omitted; values are consumed sequentially in write order.
- `Inline`: each entry stores its identifier directly.
- `Registry`: identifiers are stored once in the header and entries reference registry indexes. This is the default.

`DataIdentifier.Kind` is preserved for Inline and Registry modes.

## Length encoding

`BinaryLengthEncoding` supports `Byte`, `UInt16`, `UInt32`, and `Variable` (7-bit variable-length unsigned integer). Identifier string lengths can be configured independently through `IdentifierLengthEncoding`.

## Header v1

1. configured magic bytes
2. format version byte
3. identifier mode byte
4. identifier-length encoding byte
5. general length encoding byte
6. optional identifier registry (Registry mode)
7. root node payload

No external NuGet dependency is used by the Binary formatter.
