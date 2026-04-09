namespace Sachssoft.Sasofly.Documents.Naming.Cases;

public sealed class PascalCase : NamingConventionBase
{
    public override string? SpecialCharacters => null;

    public override string Convert(string value, NamingOptions options)
    {
        var words = GetWords(value, options, CharacterCasing.Word);
        return string.Join("", words);
    }
}
