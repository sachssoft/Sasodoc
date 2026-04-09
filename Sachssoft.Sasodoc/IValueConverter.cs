using System;
using System.Globalization;

namespace Sachssoft.Sasodoc
{
    public interface IValueConverter
    {

        string? ConvertTo(object? value, Type targetType, object? parameter = null, CultureInfo? culture = null);

        object? ConvertFrom(string? value, Type targetType, object? parameter = null, CultureInfo? culture = null);

    }
}
