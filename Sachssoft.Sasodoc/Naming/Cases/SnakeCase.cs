namespace Sachssoft.Sasodoc.Naming.Cases
{
    public sealed class SnakeCase : NamingConventionBase
    {
        public override string? SpecialCharacters => "_";

        public override string? Convert(string? value, NamingOptions? options)
        {
            var words = GetWords(value, options, CharacterCasing.Lower);
            return string.Join("_", words);
        }
    }
}