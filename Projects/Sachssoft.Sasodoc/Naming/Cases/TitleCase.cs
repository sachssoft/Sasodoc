namespace Sachssoft.Documents.Naming.Cases
{
    /// <summary>
    /// Converts names to title case, using capitalized words separated by spaces.
    /// </summary>
    public sealed class TitleCase : NamingConventionBase
    {
        /// <inheritdoc/>
        public override string? SpecialCharacters => " ";

        /// <inheritdoc/>
        public override string? Convert(string? value, NamingOptions? options)
        {
            var words = GetWords(value, options, CharacterCasing.Word);
            return string.Join(" ", words);
        }
    }
}
