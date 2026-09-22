namespace Sachssoft.Documents.Formats.Binary;

/// <summary>Specifies how lengths and identifier registry indexes are encoded.</summary>
public enum BinaryLengthEncoding
{
    /// <summary>Uses an unsigned 8-bit value.</summary>
    Byte,
    /// <summary>Uses an unsigned 16-bit value.</summary>
    UInt16,
    /// <summary>Uses an unsigned 32-bit value.</summary>
    UInt32,
    /// <summary>Uses a variable-length 7-bit encoded unsigned integer.</summary>
    Variable
}
