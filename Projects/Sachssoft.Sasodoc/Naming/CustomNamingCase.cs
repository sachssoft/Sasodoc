using System;

namespace Sachssoft.Documents.Naming
{
    /// <summary>
    /// Provides a configurable naming convention with custom separators and per-word casing.
    /// </summary>
    public class CustomNamingCase : NamingConventionBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CustomNamingCase"/> class.
        /// </summary>
        /// <param name="specialCharacters">The special characters treated as word separators.</param>
        /// <param name="separator">The separator inserted between converted words.</param>
        /// <param name="casingAction">The function that determines the casing for each word by its zero-based index.</param>
        /// <exception cref="ArgumentNullException"><paramref name="casingAction"/> is <see langword="null"/>.</exception>
        public CustomNamingCase(string? specialCharacters, string? separator, Func<int, CharacterCasing> casingAction)
        {
            ArgumentNullException.ThrowIfNull(casingAction);

            SpecialCharacters = specialCharacters;
            Separator = separator;
            CasingAction = casingAction;
        }

        /// <summary>
        /// Gets the function that determines the casing for each word by its zero-based index.
        /// </summary>
        public Func<int, CharacterCasing> CasingAction { get; }

        /// <inheritdoc/>
        public override string? SpecialCharacters { get; }

        /// <summary>
        /// Gets the separator inserted between converted words.
        /// </summary>
        public string? Separator { get; }

        /// <inheritdoc/>
        public override string? Convert(string? value, NamingOptions? options)
        {
            var words = GetWords(value, options, CasingAction);
            return string.Join(Separator, words);
        }
    }
}
