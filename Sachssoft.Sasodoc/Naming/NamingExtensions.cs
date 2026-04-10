using System;

namespace Sachssoft.Sasodoc.Naming.Case
{
    /// <summary>
    /// Provides extension methods for converting strings to different naming conventions.
    /// </summary>
    public static class NamingExtensions
    {
        /// <summary>
        /// Converts the string to the specified naming convention type with default options.
        /// </summary>
        public static string ToCase<TNamingCase>(this string value)
            where TNamingCase : class, INamingConvention, new()
            => ToCase<TNamingCase>(value, new NamingOptions());

        /// <summary>
        /// Converts the string to the specified naming convention type using the provided options.
        /// </summary>
        public static string ToCase<TNamingCase>(this string value, NamingOptions? options)
            where TNamingCase : class, INamingConvention, new()
        {
            var naming = new TNamingCase();
            return naming.Convert(value, options);
        }

        /// <summary>
        /// Converts the string using the specified naming convention with default options.
        /// </summary>
        public static string ToCase(this string value, INamingConvention? convention)
            => ToCase(value, convention, new NamingOptions());

        /// <summary>
        /// Converts the string using the specified naming convention and options.
        /// </summary>
        public static string ToCase(this string value, INamingConvention? convention, NamingOptions? options)
        {
            if (convention == null)
                return value;
            return convention.Convert(value, options);
        }

        /// <summary>
        /// Converts the string to a custom naming case with a fixed separator and casing for all words.
        /// </summary>
        public static string ToCustomCase(this string value, NamingOptions? options, string? separator, CharacterCasing casing)
            => ToCustomCase(value, options, null, separator, (i) => casing);

        /// <summary>
        /// Converts the string to a custom naming case with special characters, separator, and fixed casing.
        /// </summary>
        public static string ToCustomCase(this string value, NamingOptions? options, string? special_chars, string? separator, CharacterCasing casing)
            => ToCustomCase(value, options, special_chars, separator, (i) => casing);

        /// <summary>
        /// Converts the string to a custom naming case with a separator and a per-word casing function.
        /// </summary>
        public static string ToCustomCase(this string value, NamingOptions? options, string? separator, Func<int, CharacterCasing> casing_action)
            => ToCustomCase(value, options, null, separator, casing_action);

        /// <summary>
        /// Converts the string to a fully custom naming case with special characters, separator, and per-word casing function.
        /// </summary>
        public static string ToCustomCase(this string value, NamingOptions? options, string? special_chars, string? separator, Func<int, CharacterCasing> casing_action)
        {
            var naming = new CustomNamingCase(special_chars, separator, casing_action);
            return naming.Convert(value, options);
        }
    }
}