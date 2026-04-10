namespace Sachssoft.Sasodoc.Naming
{
    /// <summary>
    /// Defines a naming convention for converting strings to a specific case or format.
    /// </summary>
    public interface INamingConvention
    {
        /// <summary>
        /// Converts the specified string to the target naming convention.
        /// </summary>
        /// <param name="value">The input string to convert.</param>
        /// <param name="options">The naming options to apply during conversion.</param>
        /// <returns>The converted string according to the naming convention.</returns>
        string? Convert(string? value, NamingOptions? options);

        /// <summary>
        /// Special characters that are part of the string case and may affect word separation.
        /// </summary>
        string? SpecialCharacters { get; }
    }
}
