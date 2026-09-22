using System.Globalization;

namespace Sachssoft.Documents.Naming
{
    /// <summary>
    /// Provides options that control how names are split, filtered and transformed.
    /// </summary>
    public class NamingOptions
    {
        /// <summary>
        /// Gets the culture used for casing transformations.
        /// </summary>
        public CultureInfo? Culture { get; init; } = CultureInfo.InvariantCulture;

        /// <summary>
        /// Gets a value indicating whether only ASCII characters are accepted.
        /// </summary>
        public bool AsciiOnly { get; init; } = false;

        /// <summary>
        /// Gets a value indicating whether symbol characters are preserved.
        /// </summary>
        public bool KeepSymbols { get; init; } = false;

        /// <summary>
        /// Gets a value indicating whether punctuation characters are preserved.
        /// </summary>
        public bool KeepPunctuations { get; init; } = false;

        /// <summary>
        /// Gets a value indicating whether invalid characters are ignored when determining word boundaries.
        /// </summary>
        public bool IgnoreIfInvalidCharacters { get; init; } = true;

        /// <summary>
        /// Gets a value indicating whether a transition from lowercase to uppercase starts a new word.
        /// </summary>
        public bool SeparateIfUpperCase { get; init; } = true;

        /// <summary>
        /// Gets a value indicating whether consecutive uppercase characters are kept as one word.
        /// </summary>
        public bool KeepUpperCaseWord { get; init; } = true;
    }
}
