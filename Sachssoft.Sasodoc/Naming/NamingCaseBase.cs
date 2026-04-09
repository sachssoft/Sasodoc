using System;
using System.Collections.Generic;
using System.Globalization;

namespace Sachssoft.Sasofly.Documents.Naming;

/// <summary>
/// Base class for naming conventions, providing common word extraction and transformation logic.
/// </summary>
public abstract class NamingConventionBase : INamingConvention
{
    internal protected NamingConventionBase()
    {
    }

    /// <summary>
    /// Converts the name according to the string case transformation.
    /// </summary>
    public abstract string Convert(string value, NamingOptions? options);

    /// <summary>
    /// Special characters that are part of the string case.
    /// </summary>
    public abstract string? SpecialCharacters { get; }

    /// <summary>
    /// Extracts the words from this value.
    /// </summary>
    /// <remarks>
    /// Finde die Wörter aus diesem Wert heraus.
    /// </remarks>
    internal protected string[] GetWords(string? value, NamingOptions? options, CharacterCasing casing)
        => GetWords(value, options, (i) => casing);

    /// <summary>
    /// Extracts the words from this value and applies a casing function per word.
    /// </summary>
    /// <remarks>
    /// Finde die Wörter aus diesem Wert heraus und wende eine Groß-/Kleinschreibung pro Wort an.
    /// </remarks>
    internal protected string[] GetWords(string? value, NamingOptions? options, Func<int, CharacterCasing> casing_action)
    {
        if (string.IsNullOrWhiteSpace(value)) return new string[0];

        var words = new List<string>();
        var current_word = string.Empty;
        options ??= new();

        for (int i = 0; i < value.Length; i++)
        {
            var current = value[i];

            if (!KeepCharacter(current, SpecialCharacters, options, false)) continue;

            // Füge ein Zeichen hinzu
            current_word += current;

            // Weiter mit der Prüfung eines nächsten Zeichens
            if (i < value.Length - 1)
            {
                var next = value[i + 1];
                var condition = !KeepCharacter(next, SpecialCharacters, options, true);

                // Prüfe, ob das aktuelle Zeichen klein ist
                if (char.IsLower(current))
                {
                    if (char.IsUpper(next))
                    {
                        condition |= options.SeparateIfUpperCase;
                    }
                }
                // Prüfe, ob das aktuelle Zeichen groß ist
                else if (char.IsUpper(current))
                {
                    if (char.IsUpper(next))
                    {
                        condition |= !options.KeepUpperCaseWord;
                    }
                }

                // Wenn es eine Zeichensetzung oder ein Leerzeichen ist
                if (condition && current_word.Length > 0)
                {
                    words.Add(TransformCase(current_word, options, casing_action(words.Count)));
                    current_word = string.Empty;
                }
            }
            else if (i == value.Length - 1)
            {
                words.Add(TransformCase(current_word, options, casing_action(words.Count)));
            }
        }

        return words.ToArray();
    }

    /// <summary>
    /// Transforms a word according to the specified character casing.
    /// </summary>
    private string TransformCase(string word, NamingOptions options, CharacterCasing casing)
    {
        var culture = options.Culture ?? CultureInfo.InvariantCulture;

        switch (casing)
        {
            case CharacterCasing.Lower:
                return culture.TextInfo.ToLower(word);
            case CharacterCasing.Upper:
                return culture.TextInfo.ToUpper(word);
            case CharacterCasing.Word:
                return culture.TextInfo.ToTitleCase(word);
            default:
                return word;
        }
    }

    /// <summary>
    /// Determines whether a character should be kept when splitting the string into words.
    /// </summary>
    /// <remarks>
    /// Überprüft, ob ein Zeichen beim Aufteilen des Strings in Wörter beibehalten werden soll.
    /// </remarks>
    private bool KeepCharacter(char character, string? special_chars, NamingOptions options, bool ignore_condition)
    {
        // Ist es eine Zeichensetzung oder ein Leerzeichen?
        if (char.IsWhiteSpace(character)) return false;

        // Ist es ein Trennzeichen?
        if (char.IsSeparator(character)) return false;

        // Ist es ein Spezialzeichen?
        if (special_chars != null && special_chars.Contains(character)) return false;

        // Setze die Bedingung beim Ignorieren der ungültigen Zeichen
        var result = ignore_condition && options.IgnoreIfInvalidCharacters;

        // Option: Ist es ein ASCII-Zeichen?
        if (options.AsciiOnly && !char.IsAscii(character)) return result;

        // Option: Werden die Zeichensetzungen behalten?
        if (!options.KeepPunctuations && char.IsPunctuation(character)) return false;

        // Option: Werden die Symbole behalten?
        if (!options.KeepSymbols && char.IsSymbol(character)) return result;

        return true;
    }
}
