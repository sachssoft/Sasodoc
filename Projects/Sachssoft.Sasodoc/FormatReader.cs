using System;

namespace Sachssoft.Documents
{
    /// <summary>
    /// Provides a strongly typed format reader base for reading objects and nested data.
    /// </summary>
    /// <typeparam name="TReader">The concrete format reader type.</typeparam>
    public abstract class FormatReader<TReader> : FormatReaderBase
        where TReader : FormatReader<TReader>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FormatReader{TReader}"/> class.
        /// </summary>
        protected FormatReader() { }

        /// <summary>
        /// Creates a new strongly typed format reader.
        /// </summary>
        /// <returns>A new instance of the concrete format reader.</returns>
        public abstract override TReader CreateReader();

        /// <summary>
        /// Reads an object using the specified object reader generator.
        /// </summary>
        /// <param name="key">The key identifying the object.</param>
        /// <param name="generator">The callback used to generate the object reader.</param>
        /// <param name="fallback">The value returned when the object cannot be read.</param>
        /// <returns>The read object or <paramref name="fallback"/>.</returns>
        public IObjectReader<TReader>? ReadObject(string key, ObjectReaderGeneratorCallback<TReader> generator, IObjectReader<TReader>? fallback = default) => ReadObject(new DataIdentifier(key), generator, fallback);

        /// <summary>
        /// Reads an object using the specified object reader generator.
        /// </summary>
        /// <param name="identifier">The identifier of the object.</param>
        /// <param name="generator">The callback used to generate the object reader.</param>
        /// <param name="fallback">The value returned when the object cannot be read.</param>
        /// <returns>The read object or <paramref name="fallback"/>.</returns>
        public abstract IObjectReader<TReader>? ReadObject(DataIdentifier identifier, ObjectReaderGeneratorCallback<TReader> generator, IObjectReader<TReader>? fallback = default);

        /// <summary>
        /// Reads nested data and returns a strongly typed reader for it.
        /// </summary>
        /// <param name="identifier">The identifier of the nested data.</param>
        /// <returns>A strongly typed reader for the nested data.</returns>
        public abstract override TReader Read(DataIdentifier identifier);

        /// <summary>
        /// Reads an array of objects using the specified object reader generator.
        /// </summary>
        /// <param name="key">The key identifying the array.</param>
        /// <param name="generator">The callback used to generate object readers.</param>
        /// <param name="fallback">The value returned when the array cannot be read.</param>
        /// <returns>The read array or <paramref name="fallback"/>.</returns>
        public IObjectReader<TReader>[] ReadObjectArray(string key, ObjectReaderGeneratorCallback<TReader> generator, IObjectReader<TReader>[]? fallback = default) => ReadObjectArray(new DataIdentifier(key), generator, fallback);

        /// <summary>
        /// Reads an array of objects using the specified object reader generator.
        /// </summary>
        /// <param name="identifier">The identifier of the array.</param>
        /// <param name="generator">The callback used to generate object readers.</param>
        /// <param name="fallback">The value returned when the array cannot be read.</param>
        /// <returns>The read array or <paramref name="fallback"/>.</returns>
        public abstract IObjectReader<TReader>[] ReadObjectArray(DataIdentifier identifier, ObjectReaderGeneratorCallback<TReader> generator, IObjectReader<TReader>[]? fallback = default);

        /// <summary>
        /// Reads an array of nested data and returns strongly typed readers for its elements.
        /// </summary>
        /// <param name="identifier">The identifier of the array.</param>
        /// <returns>An array containing strongly typed readers for the nested data.</returns>
        public abstract override TReader[] ReadArray(DataIdentifier identifier);

        /// <inheritdoc />
        public sealed override object? ReadObject(DataIdentifier identifier, ObjectReaderGeneratorCallback generator, object? fallback = default)
        {
            ArgumentNullException.ThrowIfNull(generator);

            return ReadObject(identifier, reader => (IObjectReader<TReader>?)generator(reader), (IObjectReader<TReader>?)fallback);
        }

        /// <inheritdoc />
        public sealed override object[] ReadObjectArray(DataIdentifier identifier, ObjectReaderGeneratorCallback generator, object[]? fallback = default)
        {
            ArgumentNullException.ThrowIfNull(generator);

            return ReadObjectArray(identifier, reader => (IObjectReader<TReader>?)generator(reader), ConvertObjectReaders(fallback));
        }

        /// <summary>
        /// Converts non-generic object readers to strongly typed object readers.
        /// </summary>
        /// <param name="values">The values to convert.</param>
        /// <returns>The converted values, or <see langword="null"/> if <paramref name="values"/> is <see langword="null"/>.</returns>
        private static IObjectReader<TReader>[]? ConvertObjectReaders(object[]? values)
        {
            if (values == null) return null;

            var result = new IObjectReader<TReader>[values.Length];

            for (int i = 0; i < values.Length; i++)
            {
                result[i] = (IObjectReader<TReader>)values[i];
            }

            return result;
        }
    }
}