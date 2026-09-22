namespace Sachssoft.Documents.Naming
{
    /// <summary>
    /// Defines a naming convention for converting strings to a specific case or format.
    /// </summary>
    public interface INamingConvention
    {
        /// <summary>
        /// Converts the specified value according to this naming convention.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <param name="options">The options that control the conversion.</param>
        /// <returns>The converted value, or <see langword="null"/> when the conversion produces no value.</returns>
        string? Convert(string? value, NamingOptions? options);

        /// <summary>
        /// Gets the special characters used by this naming convention as word separators.
        /// </summary>
        string? SpecialCharacters { get; }
    }
}
