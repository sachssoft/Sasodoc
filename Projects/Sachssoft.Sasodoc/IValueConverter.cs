using System;
using System.Globalization;

namespace Sachssoft.Documents
{
    /// <summary>
    /// Defines a converter for converting values between object representations and their string representations.
    /// </summary>
    public interface IValueConverter
    {
        /// <summary>
        /// Converts the specified value to its string representation.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <param name="targetType">The target type associated with the conversion.</param>
        /// <param name="parameter">An optional parameter that provides additional conversion information.</param>
        /// <param name="culture">The culture to use during conversion, or <see langword="null"/> to use the converter's default behavior.</param>
        /// <returns>The converted string representation, or <see langword="null"/> if the conversion produces no value.</returns>
        string? ConvertTo(object? value, Type targetType, object? parameter = null, CultureInfo? culture = null);

        /// <summary>
        /// Converts the specified string representation to a value of the specified target type.
        /// </summary>
        /// <param name="value">The string representation to convert.</param>
        /// <param name="targetType">The type to convert the value to.</param>
        /// <param name="parameter">An optional parameter that provides additional conversion information.</param>
        /// <param name="culture">The culture to use during conversion, or <see langword="null"/> to use the converter's default behavior.</param>
        /// <returns>The converted value, or <see langword="null"/> if the conversion produces no value.</returns>
        object? ConvertFrom(string? value, Type targetType, object? parameter = null, CultureInfo? culture = null);
    }
}