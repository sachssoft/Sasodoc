using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Sachssoft.Documents
{
    /// <summary>
    /// Provides a registry for value converters and type converters used by document serialization.
    /// </summary>
    /// <remarks>
    /// Built-in converters are registered explicitly so conversion does not depend on runtime
    /// converter discovery. Additional converters can be registered for application-specific types.
    /// </remarks>
    public static class ConverterRegistry
    {
        private static readonly object _syncRoot = new();
        private static readonly Dictionary<Type, IValueConverter> _valueConverters = new();
        private static readonly Dictionary<Type, TypeConverter> _typeConverters = new()
        {
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
            { typeof(DateTime), new DateTimeConverter() },
            { typeof(TimeSpan), new TimeSpanConverter() },
            { typeof(Guid), new GuidConverter() },
            { typeof(char), new CharConverter() },
            { typeof(Uri), new UriTypeConverter() },
            { typeof(Version), new VersionConverter() }
        };

        /// <summary>
        /// Registers a type converter for the specified type.
        /// </summary>
        /// <param name="type">The type for which the converter is registered.</param>
        /// <param name="converter">The converter to register.</param>
        /// <exception cref="ArgumentNullException"><paramref name="type"/> or <paramref name="converter"/> is <see langword="null"/>.</exception>
        public static void Register(Type type, TypeConverter converter)
        {
            ArgumentNullException.ThrowIfNull(type);
            ArgumentNullException.ThrowIfNull(converter);

            lock (_syncRoot)
            {
                _typeConverters[type] = converter;
            }
        }

        /// <summary>
        /// Registers a type converter for the specified type.
        /// </summary>
        /// <typeparam name="T">The type for which the converter is registered.</typeparam>
        /// <param name="converter">The converter to register.</param>
        /// <exception cref="ArgumentNullException"><paramref name="converter"/> is <see langword="null"/>.</exception>
        public static void Register<T>(TypeConverter converter) => Register(typeof(T), converter);

        /// <summary>
        /// Registers a value converter for the specified type.
        /// </summary>
        /// <param name="type">The type for which the converter is registered.</param>
        /// <param name="converter">The converter to register.</param>
        /// <exception cref="ArgumentNullException"><paramref name="type"/> or <paramref name="converter"/> is <see langword="null"/>.</exception>
        public static void Register(Type type, IValueConverter converter)
        {
            ArgumentNullException.ThrowIfNull(type);
            ArgumentNullException.ThrowIfNull(converter);

            lock (_syncRoot)
            {
                _valueConverters[type] = converter;
            }
        }

        /// <summary>
        /// Registers a value converter for the specified type.
        /// </summary>
        /// <typeparam name="T">The type for which the converter is registered.</typeparam>
        /// <param name="converter">The converter to register.</param>
        /// <exception cref="ArgumentNullException"><paramref name="converter"/> is <see langword="null"/>.</exception>
        public static void Register<T>(IValueConverter converter) => Register(typeof(T), converter);

        /// <summary>
        /// Determines whether a converter is registered for the specified type.
        /// </summary>
        /// <param name="targetType">The type to check.</param>
        /// <returns><see langword="true"/> if a converter is registered; otherwise, <see langword="false"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="targetType"/> is <see langword="null"/>.</exception>
        public static bool HasConverter(Type targetType)
        {
            ArgumentNullException.ThrowIfNull(targetType);

            lock (_syncRoot)
            {
                return _valueConverters.ContainsKey(targetType) || _typeConverters.ContainsKey(targetType);
            }
        }

        /// <summary>
        /// Converts the specified value to its string representation using a registered converter when available.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <returns>The converted string, or <see langword="null"/> if <paramref name="value"/> is <see langword="null"/>.</returns>
        public static string? ConvertTo(object? value)
        {
            if (value == null) return null;

            Type type = value.GetType();
            IValueConverter? valueConverter;
            TypeConverter? typeConverter;

            lock (_syncRoot)
            {
                _valueConverters.TryGetValue(type, out valueConverter);
                _typeConverters.TryGetValue(type, out typeConverter);
            }

            if (valueConverter != null) return valueConverter.ConvertTo(value, type);
            if (typeConverter != null) return typeConverter.ConvertToString(value);

            return value.ToString();
        }

        /// <summary>
        /// Converts the specified value to the requested target type using a registered converter.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <param name="targetType">The type to which the value is converted.</param>
        /// <returns>The converted value, or <see langword="null"/> when the converter accepts a null value.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="targetType"/> is <see langword="null"/>.</exception>
        /// <exception cref="InvalidOperationException">A registered converter fails to convert the value.</exception>
        /// <exception cref="NotSupportedException">No converter is registered for <paramref name="targetType"/>.</exception>
        public static object? ConvertFrom(object? value, Type targetType)
        {
            ArgumentNullException.ThrowIfNull(targetType);

            IValueConverter? valueConverter;
            TypeConverter? typeConverter;

            lock (_syncRoot)
            {
                _valueConverters.TryGetValue(targetType, out valueConverter);
                _typeConverters.TryGetValue(targetType, out typeConverter);
            }

            if (valueConverter != null)
            {
                try
                {
                    return valueConverter.ConvertFrom(value?.ToString(), targetType);
                }
                catch (Exception ex)
                {
                    throw new InvalidOperationException($"Conversion with IValueConverter for {targetType} failed.", ex);
                }
            }

            if (typeConverter != null)
            {
                try
                {
                    if (value == null) return null;
                    return typeConverter.ConvertFrom(value);
                }
                catch (Exception ex)
                {
                    throw new InvalidOperationException($"Conversion with TypeConverter for {targetType} failed.", ex);
                }
            }

            throw new NotSupportedException($"No converter is registered for type {targetType}.");
        }
    }
}
