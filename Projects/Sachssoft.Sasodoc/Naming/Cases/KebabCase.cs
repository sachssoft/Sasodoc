namespace Sachssoft.Documents.Naming.Cases
{
    /// <summary>
    /// Converts names to kebab case, using lowercase words separated by hyphens.
    /// </summary>
    public sealed class KebabCase : NamingConventionBase
    {
        /// <inheritdoc/>
        public override string? SpecialCharacters => "-";

        /// <inheritdoc/>
        public override string? Convert(string? value, NamingOptions? options)
        {
            var words = GetWords(value, options, CharacterCasing.Lower);
            return string.Join("-", words);
        }
    }
}
