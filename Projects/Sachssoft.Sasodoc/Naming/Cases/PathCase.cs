namespace Sachssoft.Documents.Naming.Cases
{
    /// <summary>
    /// Converts names to path case, using words separated by forward slashes while preserving their casing.
    /// </summary>
    public sealed class PathCase : NamingConventionBase
    {
        /// <inheritdoc/>
        public override string? SpecialCharacters => "/";

        /// <inheritdoc/>
        public override string? Convert(string? value, NamingOptions? options)
        {
            var words = GetWords(value, options, CharacterCasing.Normal);
            return string.Join("/", words);
        }
    }
}
