namespace Sachssoft.Documents.Formats.Binary;

/// <summary>Defines options controlling the Sasodoc binary document representation.</summary>
/// <remarks>Binary documents commonly use <c>.bin</c> or <c>.dat</c>. The <c>.sdb</c> extension is recommended for Sasodoc Binary documents.</remarks>
public sealed class BinaryOptions
{
    private ReadOnlyMemory<byte> _magic = Magics.SDB;

    /// <summary>Gets or sets the mandatory magic value written to and validated from the document header.</summary>
    /// <exception cref="ArgumentException">Thrown when the assigned magic value is empty.</exception>
    public ReadOnlyMemory<byte> Magic
    {
        get => _magic;
        set
        {
            if (value.IsEmpty) throw new ArgumentException("The binary magic value cannot be empty.", nameof(value));
            _magic = value;
        }
    }

    /// <summary>Gets or sets how data identifiers are stored.</summary>
    public BinaryIdentifierMode IdentifierMode { get; set; } = BinaryIdentifierMode.Registry;

    /// <summary>Gets or sets how inline identifier string lengths and registry identifier string lengths are encoded.</summary>
    public BinaryLengthEncoding IdentifierLengthEncoding { get; set; } = BinaryLengthEncoding.Variable;

    /// <summary>Gets or sets how collection lengths and registry indexes are encoded.</summary>
    public BinaryLengthEncoding LengthEncoding { get; set; } = BinaryLengthEncoding.Variable;
}
