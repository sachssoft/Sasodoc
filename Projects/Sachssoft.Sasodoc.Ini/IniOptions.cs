using System;

namespace Sachssoft.Documents.Formats.Ini;

/// <summary>
/// Defines format-specific options used by the INI reader and writer.
/// </summary>
public sealed class IniOptions
{
    private char _arraySeparator = ',';
    private char _sectionSeparator = '.';

    /// <summary>
    /// Gets or sets the character used to separate primitive array values.
    /// </summary>
    public char ArraySeparator
    {
        get => _arraySeparator;
        set
        {
            ValidateSeparator(value, nameof(ArraySeparator));
            if (value == _sectionSeparator) throw new ArgumentException("The array separator and section separator must be different.", nameof(value));
            _arraySeparator = value;
        }
    }

    /// <summary>
    /// Gets or sets the character used to separate nested section names.
    /// </summary>
    public char SectionSeparator
    {
        get => _sectionSeparator;
        set
        {
            ValidateSeparator(value, nameof(SectionSeparator));
            if (value == _arraySeparator) throw new ArgumentException("The section separator and array separator must be different.", nameof(value));
            _sectionSeparator = value;
        }
    }

    private static void ValidateSeparator(char value, string parameterName)
    {
        if (char.IsWhiteSpace(value) || value is '[' or ']' or '=' or '\r' or '\n' or '"') throw new ArgumentException("The specified character cannot be used as an INI separator.", parameterName);
    }
}
