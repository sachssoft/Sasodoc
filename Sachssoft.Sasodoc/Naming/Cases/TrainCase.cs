namespace Sachssoft.Sasodoc.Naming.Case
{
    public sealed class TrainCase : NamingConventionBase
    {
        public override string? SpecialCharacters => "-";

        public override string Convert(string value, NamingOptions options)
        {
            var words = GetWords(value, options, CharacterCasing.Word);
            return string.Join("-", words);
        }
    }
}