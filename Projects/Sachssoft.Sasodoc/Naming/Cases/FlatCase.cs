namespace Sachssoft.Documents.Naming.Cases
{
    /// <summary>
    /// Converts names to flat case, using lowercase words without separators.
    /// </summary>
    public sealed class FlatCase : NamingConventionBase
    {
        /// <inheritdoc/>
        public override string? SpecialCharacters => null;

        /// <inheritdoc/>
        public override string? Convert(string? value, NamingOptions? options)
        {
            var words = GetWords(value, options, CharacterCasing.Lower);
            return string.Join("", words);
        }
    }
}
