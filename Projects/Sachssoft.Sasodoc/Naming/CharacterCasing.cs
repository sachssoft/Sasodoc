namespace Sachssoft.Documents.Naming
{
    /// <summary>
    /// Specifies the character casing applied to a word during naming conversion.
    /// </summary>
    public enum CharacterCasing
    {
        /// <summary>
        /// Preserves the original character casing.
        /// </summary>
        Normal = 0,

        /// <summary>
        /// Converts all characters to lowercase.
        /// </summary>
        Lower = 1,

        /// <summary>
        /// Converts all characters to uppercase.
        /// </summary>
        Upper = 2,

        /// <summary>
        /// Converts the word to title case.
        /// </summary>
        Word = 3
    }
}
