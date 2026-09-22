namespace Sachssoft.Documents.Formats.Binary;

/// <summary>Provides predefined magic values for commonly used binary document file extensions and Sasodoc Binary documents.</summary>
public static class Magics
{
    /// <summary>Magic value intended for <c>.sdb</c> Sasodoc Binary files.</summary>
    public static readonly ReadOnlyMemory<byte> SDB = "SDB"u8.ToArray();
    /// <summary>Magic value intended for generic <c>.bin</c> files.</summary>
    public static readonly ReadOnlyMemory<byte> BIN = "BIN"u8.ToArray();
    /// <summary>Magic value intended for generic <c>.dat</c> files.</summary>
    public static readonly ReadOnlyMemory<byte> DAT = "DAT"u8.ToArray();
    /// <summary>Extended Sasodoc Binary magic value.</summary>
    public static readonly ReadOnlyMemory<byte> SASODOCBIN = "SASODOCBIN"u8.ToArray();
    /// <summary>Extended Sasodoc data magic value.</summary>
    public static readonly ReadOnlyMemory<byte> SASODOCDAT = "SASODOCDAT"u8.ToArray();
}
