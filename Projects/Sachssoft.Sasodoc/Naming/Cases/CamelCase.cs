namespace Sachssoft.Documents.Naming.Cases
{
    /// <summary>
    /// Converts names to camel case, using the first word in lowercase and each subsequent word capitalized.
    /// </summary>
    public sealed class CamelCase : NamingConventionBase
    {
        /// <inheritdoc/>
        public override string? SpecialCharacters => null;

        /// <inheritdoc/>
        public override string? Convert(string? value, NamingOptions? options)
        {
            var words = GetWords(value, options, (i) => i == 0 ? CharacterCasing.Lower : CharacterCasing.Word);
            return string.Join("", words);
        }
    }
}
