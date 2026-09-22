namespace Sachssoft.Documents.Naming.Cases
{
    /// <summary>
    /// Converts names to constant case, using uppercase words separated by underscores.
    /// </summary>
    public sealed class ConstantCase : NamingConventionBase
    {
        /// <inheritdoc/>
        public override string? SpecialCharacters => "_";

        /// <inheritdoc/>
        public override string? Convert(string? value, NamingOptions? options)
        {
            var words = GetWords(value, options, CharacterCasing.Upper);
            return string.Join("_", words);
        }
    }
}
