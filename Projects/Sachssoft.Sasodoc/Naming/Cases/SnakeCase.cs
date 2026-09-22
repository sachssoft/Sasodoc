namespace Sachssoft.Documents.Naming.Cases
{
    /// <summary>
    /// Converts names to snake case, using lowercase words separated by underscores.
    /// </summary>
    public sealed class SnakeCase : NamingConventionBase
    {
        /// <inheritdoc/>
        public override string? SpecialCharacters => "_";

        /// <inheritdoc/>
        public override string? Convert(string? value, NamingOptions? options)
        {
            var words = GetWords(value, options, CharacterCasing.Lower);
            return string.Join("_", words);
        }
    }
}
