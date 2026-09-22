using System;

namespace Sachssoft.Documents.Naming
{
    /// <summary>
    /// Provides extension methods for converting strings using naming conventions.
    /// </summary>
    public static class NamingExtensions
    {
        /// <summary>
        /// Converts a value using the specified naming convention type and default naming options.
        /// </summary>
        /// <typeparam name="TNamingCase">The naming convention type to use.</typeparam>
        /// <param name="value">The value to convert.</param>
        /// <returns>The converted value, or <see langword="null"/> when the conversion produces no value.</returns>
        public static string? ToCase<TNamingCase>(this string value) where TNamingCase : class, INamingConvention, new() => ToCase<TNamingCase>(value, new NamingOptions());

        /// <summary>
        /// Converts a value using the specified naming convention type and naming options.
        /// </summary>
        /// <typeparam name="TNamingCase">The naming convention type to use.</typeparam>
        /// <param name="value">The value to convert.</param>
        /// <param name="options">The options that control the conversion.</param>
        /// <returns>The converted value, or <see langword="null"/> when the conversion produces no value.</returns>
        public static string? ToCase<TNamingCase>(this string value, NamingOptions? options) where TNamingCase : class, INamingConvention, new()
        {
            var naming = new TNamingCase();
            return naming.Convert(value, options);
        }

        /// <summary>
        /// Converts a value using the specified naming convention and default naming options.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <param name="convention">The naming convention to use, or <see langword="null"/> to return the original value.</param>
        /// <returns>The converted value.</returns>
        public static string? ToCase(this string value, INamingConvention? convention) => ToCase(value, convention, new NamingOptions());

        /// <summary>
        /// Converts a value using the specified naming convention and naming options.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <param name="convention">The naming convention to use, or <see langword="null"/> to return the original value.</param>
        /// <param name="options">The options that control the conversion.</param>
        /// <returns>The converted value.</returns>
        public static string? ToCase(this string value, INamingConvention? convention, NamingOptions? options)
        {
            if (convention == null) return value;
            return convention.Convert(value, options);
        }

        /// <summary>
        /// Converts a value using a custom separator and a fixed casing for all words.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <param name="options">The options that control the conversion.</param>
        /// <param name="separator">The separator inserted between converted words.</param>
        /// <param name="casing">The casing applied to every word.</param>
        /// <returns>The converted value.</returns>
        public static string? ToCustomCase(this string value, NamingOptions? options, string? separator, CharacterCasing casing) => ToCustomCase(value, options, null, separator, (i) => casing);

        /// <summary>
        /// Converts a value using custom special characters, a separator and a fixed casing for all words.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <param name="options">The options that control the conversion.</param>
        /// <param name="specialChars">The special characters treated as word separators.</param>
        /// <param name="separator">The separator inserted between converted words.</param>
        /// <param name="casing">The casing applied to every word.</param>
        /// <returns>The converted value.</returns>
        public static string? ToCustomCase(this string value, NamingOptions? options, string? specialChars, string? separator, CharacterCasing casing) => ToCustomCase(value, options, specialChars, separator, (i) => casing);

        /// <summary>
        /// Converts a value using a custom separator and per-word casing.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <param name="options">The options that control the conversion.</param>
        /// <param name="separator">The separator inserted between converted words.</param>
        /// <param name="casingAction">The function that determines the casing for each word by its zero-based index.</param>
        /// <returns>The converted value.</returns>
        public static string? ToCustomCase(this string value, NamingOptions? options, string? separator, Func<int, CharacterCasing> casingAction) => ToCustomCase(value, options, null, separator, casingAction);

        /// <summary>
        /// Converts a value using custom special characters, a separator and per-word casing.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <param name="options">The options that control the conversion.</param>
        /// <param name="specialChars">The special characters treated as word separators.</param>
        /// <param name="separator">The separator inserted between converted words.</param>
        /// <param name="casingAction">The function that determines the casing for each word by its zero-based index.</param>
        /// <returns>The converted value.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="casingAction"/> is <see langword="null"/>.</exception>
        public static string? ToCustomCase(this string value, NamingOptions? options, string? specialChars, string? separator, Func<int, CharacterCasing> casingAction)
        {
            ArgumentNullException.ThrowIfNull(casingAction);

            var naming = new CustomNamingCase(specialChars, separator, casingAction);
            return naming.Convert(value, options);
        }
    }
}
