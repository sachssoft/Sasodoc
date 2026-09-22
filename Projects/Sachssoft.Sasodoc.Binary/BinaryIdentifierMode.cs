namespace Sachssoft.Documents.Formats.Binary;

/// <summary>Specifies how data identifiers are stored in a binary document.</summary>
public enum BinaryIdentifierMode
{
    /// <summary>Identifiers are omitted. Values are read sequentially in their written order.</summary>
    None,
    /// <summary>Each identifier is stored inline with its value.</summary>
    Inline,
    /// <summary>Identifiers are stored once in the document header and referenced by index.</summary>
    Registry
}
