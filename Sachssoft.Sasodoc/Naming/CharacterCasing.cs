namespace Sachssoft.Sasofly.Documents.Naming;

/// <summary>
/// Specifies the character casing to apply to words when converting naming conventions.
/// </summary>
public enum CharacterCasing
{
    /// <summary>
    /// No transformation; use the original casing.
    /// </summary>
    Normal = 0,

    /// <summary>
    /// Convert all characters to lowercase.
    /// </summary>
    Lower = 1,

    /// <summary>
    /// Convert all characters to uppercase.
    /// </summary>
    Upper = 2,

    /// <summary>
    /// Capitalize the first character of each word (Title Case).
    /// </summary>
    Word = 3
}
