namespace Sachssoft.Documents.Naming.Cases
{
    /// <summary>
    /// Converts names to sentence case, using the first word capitalized and subsequent words in lowercase.
    /// </summary>
    public sealed class SentenceCase : NamingConventionBase
    {
        /// <inheritdoc/>
        public override string? SpecialCharacters => " ";

        /// <inheritdoc/>
        public override string? Convert(string? value, NamingOptions? options)
        {
            var words = GetWords(value, options, (i) => i == 0 ? CharacterCasing.Word : CharacterCasing.Lower);
            return string.Join(" ", words);
        }
    }
}
