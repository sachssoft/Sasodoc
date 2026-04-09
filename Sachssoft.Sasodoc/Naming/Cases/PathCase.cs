namespace Sachssoft.Sasofly.Documents.Naming.Cases;

public sealed class PathCase : NamingConventionBase
{
    public override string? SpecialCharacters => "/";

    public override string Convert(string value, NamingOptions options)
    {
        var words = GetWords(value, options, CharacterCasing.Normal);
        return string.Join("/", words);
    }
}
