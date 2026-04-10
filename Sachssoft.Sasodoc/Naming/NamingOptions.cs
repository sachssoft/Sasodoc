using System.Globalization;

namespace Sachssoft.Sasodoc.Naming.Case
{
    public class NamingOptions
    {
        public CultureInfo? Culture { get; init; } = CultureInfo.InvariantCulture;

        public bool AsciiOnly { get; init; } = false;

        public bool KeepSymbols { get; init; } = false;

        public bool KeepPunctuations { get; init; } = false;

        public bool IgnoreIfInvalidCharacters { get; init; } = true;

        public bool SeparateIfUpperCase { get; init; } = true;

        public bool KeepUpperCaseWord { get; init; } = true;
    }
}