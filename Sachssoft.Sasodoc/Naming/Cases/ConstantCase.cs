namespace Sachssoft.Sasofly.Documents.Naming.Cases;

public sealed class ConstantCase : NamingConventionBase
{
    public override string? SpecialCharacters => "_";

    public override string Convert(string value, NamingOptions options)
    {
        var words = GetWords(value, options, CharacterCasing.Upper);
        return string.Join("_", words);
    }
}
