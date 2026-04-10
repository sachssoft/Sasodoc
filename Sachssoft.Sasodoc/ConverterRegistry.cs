using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Sachssoft.Sasodoc
{
    public static class ConverterRegistry
    {
        private static readonly Dictionary<Type, IValueConverter> _valueConverters = new();
        private static readonly Dictionary<Type, TypeConverter> _typeConverters = new()
    {
        // Basis-Typen
        { typeof(string), new StringConverter() },
        { typeof(bool), new BooleanConverter() },
        { typeof(byte), new ByteConverter() },
        { typeof(sbyte), new SByteConverter() },
        { typeof(short), new Int16Converter() },
        { typeof(ushort), new UInt16Converter() },
        { typeof(int), new Int32Converter() },
        { typeof(uint), new UInt32Converter() },
        { typeof(long), new Int64Converter() },
        { typeof(ulong), new UInt64Converter() },
        { typeof(float), new SingleConverter() },
        { typeof(double), new DoubleConverter() },
        { typeof(decimal), new DecimalConverter() },

        // Datum / Zeit
        { typeof(DateTime), new DateTimeConverter() },
        { typeof(TimeSpan), new TimeSpanConverter() },

        // Andere Standardtypen
        { typeof(Guid), new GuidConverter() },

        // Char
        { typeof(char), new CharConverter() },

        // Optional: Uri
        { typeof(Uri), new UriTypeConverter() },

        // Optional: Version
        { typeof(Version), new VersionConverter() }

        // Enum-Konverter müssen pro Enum-Typ registriert werden
        // Beispiel: { typeof(MyEnum), new EnumConverter(typeof(MyEnum)) }
    };

        // Registrierung für TypeConverter (AOT)
        public static void Register(Type type, TypeConverter converter)
        {
            _typeConverters[type] = converter;
        }

        public static void Register<T>(TypeConverter converter) => Register(typeof(T), converter);

        // Registrierung für IValueConverter
        public static void Register(Type type, IValueConverter converter)
        {
            _valueConverters[type] = converter;
        }

        public static void Register<T>(IValueConverter converter) => Register(typeof(T), converter);

        public static bool HasConverter(Type targetType)
        {
            return _valueConverters.ContainsKey(targetType)
                || _typeConverters.ContainsKey(targetType);
        }

        // Konvertierung nach string
        public static string? ConvertTo(object? value)
        {
            if (value == null) return null;
            Type type = value.GetType();

            if (_valueConverters.TryGetValue(type, out var valConv))
            {
                return valConv.ConvertTo(value, type);
            }

            if (_typeConverters.TryGetValue(type, out var typeConv))
            {
                return typeConv.ConvertToString(value);
            }

            return value.ToString();
        }

        public static object? ConvertFrom(object? value, Type targetType)
        {
            // 1. zuerst IValueConverter prüfen
            if (_valueConverters.TryGetValue(targetType, out var valConv))
            {
                try
                {
                    return valConv.ConvertFrom(value?.ToString(), targetType);
                }
                catch (Exception ex)
                {
                    throw new InvalidOperationException($"Konvertierung mit IValueConverter für {targetType} fehlgeschlagen.", ex);
                }
            }

            // 2. dann TypeConverter prüfen
            if (_typeConverters.TryGetValue(targetType, out var typeConv))
            {
                try
                {
                    return typeConv.ConvertFrom(value);
                }
                catch (Exception ex)
                {
                    throw new InvalidOperationException($"Konvertierung mit TypeConverter für {targetType} fehlgeschlagen.", ex);
                }
            }

            // 3. Wenn alles fehlschlägt
            throw new NotSupportedException($"Keine Konvertierung für den Typ {targetType} registriert.");
        }
    }
}
