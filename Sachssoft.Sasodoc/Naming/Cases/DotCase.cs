namespace Sachssoft.Sasofly.Documents.Naming.Cases;

public sealed class DotCase : NamingConventionBase
{
    public override string? SpecialCharacters => ".";

    public override string Convert(string value, NamingOptions options)
    {
        var words = GetWords(value, options, CharacterCasing.Lower);
        return string.Join(".", words);
    }
}
