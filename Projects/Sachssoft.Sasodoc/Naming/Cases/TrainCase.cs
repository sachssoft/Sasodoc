namespace Sachssoft.Documents.Naming.Cases
{
    /// <summary>
    /// Converts names to train case, using capitalized words separated by hyphens.
    /// </summary>
    public sealed class TrainCase : NamingConventionBase
    {
        /// <inheritdoc/>
        public override string? SpecialCharacters => "-";

        /// <inheritdoc/>
        public override string? Convert(string? value, NamingOptions? options)
        {
            var words = GetWords(value, options, CharacterCasing.Word);
            return string.Join("-", words);
        }
    }
}
