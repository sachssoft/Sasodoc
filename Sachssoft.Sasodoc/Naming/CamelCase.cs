
namespace Sachssoft.Sasodoc.Naming.Case
{
    public sealed class CamelCase : NamingConventionBase
    {
        public override string? SpecialCharacters => null;

        public override string? Convert(string value, NamingOptions options)
        {
            var words = GetWords(value, options, (i) => i == 0 ? CharacterCasing.Lower : CharacterCasing.Word);
            return string.Join("", words);
        }
    }
}