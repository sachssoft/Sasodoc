using System;

namespace Sachssoft.Documents
{
    /// <summary>
    /// Provides a strongly typed format writer base for writing objects and nested data.
    /// </summary>
    /// <typeparam name="TWriter">The concrete format writer type.</typeparam>
    public abstract class FormatWriter<TWriter> : FormatWriterBase
        where TWriter : FormatWriter<TWriter>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FormatWriter{TWriter}"/> class.
        /// </summary>
        protected FormatWriter() { }

        /// <summary>
        /// Creates a new strongly typed format writer.
        /// </summary>
        /// <returns>A new instance of the concrete format writer.</returns>
        public abstract override TWriter CreateWriter();

        /// <summary>
        /// Writes an object using the strongly typed writer.
        /// </summary>
        /// <param name="identifier">The identifier of the object.</param>
        /// <param name="value">The object writer to write.</param>
        /// <param name="writerBefore">An optional callback invoked with the writer before the object is written.</param>
        public abstract void WriteObject(DataIdentifier identifier, IObjectWriter<TWriter>? value, Action<TWriter>? writerBefore = null);

        /// <summary>
        /// Writes nested data from another strongly typed format writer.
        /// </summary>
        /// <param name="identifier">The identifier of the nested data.</param>
        /// <param name="value">The writer containing the nested data to write.</param>
        public abstract void Write(DataIdentifier identifier, TWriter? value);

        /// <summary>
        /// Writes an array of objects using the strongly typed writer.
        /// </summary>
        /// <param name="identifier">The identifier of the array.</param>
        /// <param name="values">The object writers to write.</param>
        /// <param name="writerBefore">An optional callback invoked with the writer before each object is written.</param>
        public abstract void WriteObjectArray(DataIdentifier identifier, IObjectWriter<TWriter>[]? values, Action<TWriter>? writerBefore = null);

        /// <summary>
        /// Writes an array of nested data from strongly typed format writers.
        /// </summary>
        /// <param name="identifier">The identifier of the array.</param>
        /// <param name="values">The writers containing the nested data to write.</param>
        public abstract void WriteArray(DataIdentifier identifier, TWriter[]? values);

        /// <summary>
        /// Writes an object through the non-generic format writer API.
        /// </summary>
        /// <param name="identifier">The identifier of the object.</param>
        /// <param name="value">The object writer to write.</param>
        /// <param name="writerBefore">An optional callback invoked with the writer before the object is written.</param>
        public sealed override void WriteObject(DataIdentifier identifier, object? value, Action<FormatWriterBase>? writerBefore = null) => WriteObject(identifier, (IObjectWriter<TWriter>?)value, writerBefore == null ? null : writer => writerBefore(writer));

        /// <summary>
        /// Writes nested data through the non-generic format writer API.
        /// </summary>
        /// <param name="identifier">The identifier of the nested data.</param>
        /// <param name="value">The writer containing the nested data to write.</param>
        public sealed override void Write(DataIdentifier identifier, FormatWriterBase? value) => Write(identifier, (TWriter?)value);

        /// <summary>
        /// Writes an array of objects through the non-generic format writer API.
        /// </summary>
        /// <param name="identifier">The identifier of the array.</param>
        /// <param name="values">The object writers to write.</param>
        /// <param name="writerBefore">An optional callback invoked with the writer before each object is written.</param>
        public sealed override void WriteObjectArray(DataIdentifier identifier, object[]? values, Action<FormatWriterBase>? writerBefore = null) => WriteObjectArray(identifier, ConvertObjectWriters(values), writerBefore == null ? null : writer => writerBefore(writer));

        /// <summary>
        /// Writes an array of nested data through the non-generic format writer API.
        /// </summary>
        /// <param name="identifier">The identifier of the array.</param>
        /// <param name="values">The writers containing the nested data to write.</param>
        public sealed override void WriteArray(DataIdentifier identifier, FormatWriterBase[]? values) => WriteArray(identifier, ConvertWriters(values));

        /// <summary>
        /// Converts non-generic object writers to strongly typed object writers.
        /// </summary>
        /// <param name="values">The values to convert.</param>
        /// <returns>The converted values, or <see langword="null"/> if <paramref name="values"/> is <see langword="null"/>.</returns>
        private static IObjectWriter<TWriter>[]? ConvertObjectWriters(object[]? values)
        {
            if (values == null) return null;

            var result = new IObjectWriter<TWriter>[values.Length];

            for (int i = 0; i < values.Length; i++)
            {
                result[i] = (IObjectWriter<TWriter>)values[i];
            }

            return result;
        }

        /// <summary>
        /// Converts non-generic format writers to strongly typed format writers.
        /// </summary>
        /// <param name="values">The values to convert.</param>
        /// <returns>The converted values, or <see langword="null"/> if <paramref name="values"/> is <see langword="null"/>.</returns>
        private static TWriter[]? ConvertWriters(FormatWriterBase[]? values)
        {
            if (values == null) return null;

            var result = new TWriter[values.Length];

            for (int i = 0; i < values.Length; i++)
            {
                result[i] = (TWriter)values[i];
            }

            return result;
        }
    }
}