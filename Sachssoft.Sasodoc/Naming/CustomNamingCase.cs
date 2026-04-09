using System;

namespace Sachssoft.Sasofly.Documents.Naming;

/// <summary>
/// Custom naming convention that allows specifying special characters, separators, and casing per word.
/// </summary>
public class CustomNamingCase : NamingConventionBase
{
    /// <summary>
    /// Initializes a new instance of the <see cref="CustomNamingCase"/> class.
    /// </summary>
    /// <param name="special_characters">Optional special characters that should be ignored in word separation.</param>
    /// <param name="separator">Optional separator string used between words.</param>
    /// <param name="casing_action">Function defining the character casing for each word.</param>
    public CustomNamingCase(string? special_characters, string? separator, Func<int, CharacterCasing> casing_action)
    {
        SpecialCharacters = special_characters;
        Separator = separator;
        CasingAction = casing_action;
    }

    /// <summary>
    /// Function defining the character casing for each word.
    /// </summary>
    public Func<int, CharacterCasing> CasingAction { get; }

    /// <summary>
    /// Special characters that are part of the string case.
    /// </summary>
    public override string? SpecialCharacters { get; }

    /// <summary>
    /// Separator string used between words.
    /// </summary>
    public string? Separator { get; }

    /// <summary>
    /// Converts the input value into a custom naming case using the specified options.
    /// </summary>
    /// <remarks>
    /// Trennt den String in Wörter und fügt sie unter Verwendung des Separators zusammen.
    /// </remarks>
    /// <param name="value">The input string to convert.</param>
    /// <param name="options">The naming options to use.</param>
    /// <returns>The converted string in the custom naming case.</returns>
    public override string Convert(string value, NamingOptions? options)
    {
        var words = GetWords(value, options, CasingAction);
        return string.Join(Separator, words);
    }
}
