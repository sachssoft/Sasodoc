namespace Sachssoft.Documents.Naming.Cases
{
    /// <summary>
    /// Converts names to Pascal case, using capitalized words without separators.
    /// </summary>
    public sealed class PascalCase : NamingConventionBase
    {
        /// <inheritdoc/>
        public override string? SpecialCharacters => null;

        /// <inheritdoc/>
        public override string? Convert(string? value, NamingOptions? options)
        {
            var words = GetWords(value, options, CharacterCasing.Word);
            return string.Join("", words);
        }
    }
}
