using System.Globalization;

namespace Sachssoft.Sasofly.Documents.Naming;

public class NamingOptions
{
    public CultureInfo? Culture { get; set; } = CultureInfo.InvariantCulture;

    public bool AsciiOnly { get; set; } = false;

    public bool KeepSymbols { get; set; } = false;

    public bool KeepPunctuations { get; set; } = false;

    public bool IgnoreIfInvalidCharacters { get; set; } = true;

    public bool SeparateIfUpperCase { get; set; } = true;

    public bool KeepUpperCaseWord { get; set; } = true;
}
