namespace Sachssoft.Documents.Naming.Cases
{
    /// <summary>
    /// Converts names to dot case, using lowercase words separated by dots.
    /// </summary>
    public sealed class DotCase : NamingConventionBase
    {
        /// <inheritdoc/>
        public override string? SpecialCharacters => ".";

        /// <inheritdoc/>
        public override string? Convert(string? value, NamingOptions? options)
        {
            var words = GetWords(value, options, CharacterCasing.Lower);
            return string.Join(".", words);
        }
    }
}
