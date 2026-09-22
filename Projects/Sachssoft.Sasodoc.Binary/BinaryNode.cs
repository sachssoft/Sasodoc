namespace Sachssoft.Documents.Formats.Binary;

internal sealed class BinaryNode
{
    public List<BinaryEntry> Entries { get; } = new();
}

internal sealed record BinaryEntry(DataIdentifier Identifier, BinaryValueType Type, object? Value);
