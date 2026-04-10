namespace Sachssoft.Sasodoc.Naming.Cases
{
    public sealed class SentenceCase : NamingConventionBase
    {
        public override string? SpecialCharacters => " ";

        public override string? Convert(string? value, NamingOptions? options)
        {
            var words = GetWords(value, options, (i) => i == 0 ? CharacterCasing.Word : CharacterCasing.Lower);
            return string.Join("", words);
        }
    }
}