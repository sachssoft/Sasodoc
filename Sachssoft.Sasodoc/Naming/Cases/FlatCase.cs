namespace Sachssoft.Sasodoc.Naming.Case
{
    public sealed class FlatCase : NamingConventionBase
    {
        public override string? SpecialCharacters => null;

        public override string Convert(string value, NamingOptions options)
        {
            var words = GetWords(value, options, CharacterCasing.Lower);
            return string.Join("", words);
        }
    }
}