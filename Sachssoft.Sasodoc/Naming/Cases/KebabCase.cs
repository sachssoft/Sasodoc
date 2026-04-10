namespace Sachssoft.Sasodoc.Naming.Cases
{
    public sealed class KebabCase : NamingConventionBase
    {
        public override string? SpecialCharacters => "-";

        public override string? Convert(string? value, NamingOptions? options)
        {
            var words = GetWords(value, options, CharacterCasing.Lower);
            return string.Join("-", words);
        }
    }
}
