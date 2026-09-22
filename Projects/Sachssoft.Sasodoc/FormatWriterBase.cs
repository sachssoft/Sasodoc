using System;

namespace Sachssoft.Documents
{
    /// <summary>
    /// Provides the base implementation for writing values, objects and arrays to a document format.
    /// </summary>
    public abstract class FormatWriterBase : FormatBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FormatWriterBase"/> class.
        /// </summary>
        protected FormatWriterBase() { }

        /// <summary>
        /// Creates a new writer of the concrete writer type.
        /// </summary>
        /// <returns>A new writer instance.</returns>
        /// <remarks>
        /// This factory is used internally when additional writers are required, for example when writing nested objects.
        /// </remarks>
        public abstract FormatWriterBase CreateWriter();

        /// <summary>
        /// Writes a Boolean value.
        /// </summary>
        /// <param name="key">The key identifying the value.</param>
        /// <param name="value">The value to write.</param>
        public void WriteBoolean(string key, bool value) => WriteBoolean(new DataIdentifier(key), value);

        /// <summary>
        /// Writes a Boolean value.
        /// </summary>
        /// <param name="identifier">The identifier of the value.</param>
        /// <param name="value">The value to write.</param>
        public abstract void WriteBoolean(DataIdentifier identifier, bool value);

        /// <summary>
        /// Writes an unsigned 8-bit integer.
        /// </summary>
        /// <param name="key">The key identifying the value.</param>
        /// <param name="value">The value to write.</param>
        public void WriteByte(string key, byte value) => WriteByte(new DataIdentifier(key), value);

        /// <summary>
        /// Writes an unsigned 8-bit integer.
        /// </summary>
        /// <param name="identifier">The identifier of the value.</param>
        /// <param name="value">The value to write.</param>
        public abstract void WriteByte(DataIdentifier identifier, byte value);

        /// <summary>
        /// Writes a signed 8-bit integer.
        /// </summary>
        /// <param name="key">The key identifying the value.</param>
        /// <param name="value">The value to write.</param>
        public void WriteSByte(string key, sbyte value) => WriteSByte(new DataIdentifier(key), value);

        /// <summary>
        /// Writes a signed 8-bit integer.
        /// </summary>
        /// <param name="identifier">The identifier of the value.</param>
        /// <param name="value">The value to write.</param>
        public abstract void WriteSByte(DataIdentifier identifier, sbyte value);

        /// <summary>
        /// Writes a signed 16-bit integer.
        /// </summary>
        /// <param name="key">The key identifying the value.</param>
        /// <param name="value">The value to write.</param>
        public void WriteInt16(string key, short value) => WriteInt16(new DataIdentifier(key), value);

        /// <summary>
        /// Writes a signed 16-bit integer.
        /// </summary>
        /// <param name="identifier">The identifier of the value.</param>
        /// <param name="value">The value to write.</param>
        public abstract void WriteInt16(DataIdentifier identifier, short value);

        /// <summary>
        /// Writes an unsigned 16-bit integer.
        /// </summary>
        /// <param name="key">The key identifying the value.</param>
        /// <param name="value">The value to write.</param>
        public void WriteUInt16(string key, ushort value) => WriteUInt16(new DataIdentifier(key), value);

        /// <summary>
        /// Writes an unsigned 16-bit integer.
        /// </summary>
        /// <param name="identifier">The identifier of the value.</param>
        /// <param name="value">The value to write.</param>
        public abstract void WriteUInt16(DataIdentifier identifier, ushort value);

        /// <summary>
        /// Writes a signed 32-bit integer.
        /// </summary>
        /// <param name="key">The key identifying the value.</param>
        /// <param name="value">The value to write.</param>
        public void WriteInt32(string key, int value) => WriteInt32(new DataIdentifier(key), value);

        /// <summary>
        /// Writes a signed 32-bit integer.
        /// </summary>
        /// <param name="identifier">The identifier of the value.</param>
        /// <param name="value">The value to write.</param>
        public abstract void WriteInt32(DataIdentifier identifier, int value);

        /// <summary>
        /// Writes an unsigned 32-bit integer.
        /// </summary>
        /// <param name="key">The key identifying the value.</param>
        /// <param name="value">The value to write.</param>
        public void WriteUInt32(string key, uint value) => WriteUInt32(new DataIdentifier(key), value);

        /// <summary>
        /// Writes an unsigned 32-bit integer.
        /// </summary>
        /// <param name="identifier">The identifier of the value.</param>
        /// <param name="value">The value to write.</param>
        public abstract void WriteUInt32(DataIdentifier identifier, uint value);

        /// <summary>
        /// Writes a signed 64-bit integer.
        /// </summary>
        /// <param name="key">The key identifying the value.</param>
        /// <param name="value">The value to write.</param>
        public void WriteInt64(string key, long value) => WriteInt64(new DataIdentifier(key), value);

        /// <summary>
        /// Writes a signed 64-bit integer.
        /// </summary>
        /// <param name="identifier">The identifier of the value.</param>
        /// <param name="value">The value to write.</param>
        public abstract void WriteInt64(DataIdentifier identifier, long value);

        /// <summary>
        /// Writes an unsigned 64-bit integer.
        /// </summary>
        /// <param name="key">The key identifying the value.</param>
        /// <param name="value">The value to write.</param>
        public void WriteUInt64(string key, ulong value) => WriteUInt64(new DataIdentifier(key), value);

        /// <summary>
        /// Writes an unsigned 64-bit integer.
        /// </summary>
        /// <param name="identifier">The identifier of the value.</param>
        /// <param name="value">The value to write.</param>
        public abstract void WriteUInt64(DataIdentifier identifier, ulong value);

        /// <summary>
        /// Writes a single-precision floating-point value.
        /// </summary>
        /// <param name="key">The key identifying the value.</param>
        /// <param name="value">The value to write.</param>
        public void WriteSingle(string key, float value) => WriteSingle(new DataIdentifier(key), value);

        /// <summary>
        /// Writes a single-precision floating-point value.
        /// </summary>
        /// <param name="identifier">The identifier of the value.</param>
        /// <param name="value">The value to write.</param>
        public abstract void WriteSingle(DataIdentifier identifier, float value);

        /// <summary>
        /// Writes a double-precision floating-point value.
        /// </summary>
        /// <param name="key">The key identifying the value.</param>
        /// <param name="value">The value to write.</param>
        public void WriteDouble(string key, double value) => WriteDouble(new DataIdentifier(key), value);

        /// <summary>
        /// Writes a double-precision floating-point value.
        /// </summary>
        /// <param name="identifier">The identifier of the value.</param>
        /// <param name="value">The value to write.</param>
        public abstract void WriteDouble(DataIdentifier identifier, double value);

        /// <summary>
        /// Writes a decimal value.
        /// </summary>
        /// <param name="key">The key identifying the value.</param>
        /// <param name="value">The value to write.</param>
        public void WriteDecimal(string key, decimal value) => WriteDecimal(new DataIdentifier(key), value);

        /// <summary>
        /// Writes a decimal value.
        /// </summary>
        /// <param name="identifier">The identifier of the value.</param>
        /// <param name="value">The value to write.</param>
        public abstract void WriteDecimal(DataIdentifier identifier, decimal value);

        /// <summary>
        /// Writes a character value.
        /// </summary>
        /// <param name="key">The key identifying the value.</param>
        /// <param name="value">The value to write.</param>
        public void WriteChar(string key, char value) => WriteChar(new DataIdentifier(key), value);

        /// <summary>
        /// Writes a character value.
        /// </summary>
        /// <param name="identifier">The identifier of the value.</param>
        /// <param name="value">The value to write.</param>
        public abstract void WriteChar(DataIdentifier identifier, char value);

        /// <summary>
        /// Writes a string value.
        /// </summary>
        /// <param name="key">The key identifying the value.</param>
        /// <param name="value">The value to write.</param>
        public void WriteString(string key, string? value) => WriteString(new DataIdentifier(key), value);

        /// <summary>
        /// Writes a string value.
        /// </summary>
        /// <param name="identifier">The identifier of the value.</param>
        /// <param name="value">The value to write.</param>
        public abstract void WriteString(DataIdentifier identifier, string? value);

        /// <summary>
        /// Writes a time interval.
        /// </summary>
        /// <param name="key">The key identifying the value.</param>
        /// <param name="value">The value to write.</param>
        public void WriteTimeSpan(string key, TimeSpan value) => WriteTimeSpan(new DataIdentifier(key), value);

        /// <summary>
        /// Writes a time interval.
        /// </summary>
        /// <param name="identifier">The identifier of the value.</param>
        /// <param name="value">The value to write.</param>
        public abstract void WriteTimeSpan(DataIdentifier identifier, TimeSpan value);

        /// <summary>
        /// Writes a date and time value.
        /// </summary>
        /// <param name="key">The key identifying the value.</param>
        /// <param name="value">The value to write.</param>
        public void WriteDateTime(string key, DateTime value) => WriteDateTime(new DataIdentifier(key), value);

        /// <summary>
        /// Writes a date and time value.
        /// </summary>
        /// <param name="identifier">The identifier of the value.</param>
        /// <param name="value">The value to write.</param>
        public abstract void WriteDateTime(DataIdentifier identifier, DateTime value);

        /// <summary>
        /// Writes a globally unique identifier.
        /// </summary>
        /// <param name="key">The key identifying the value.</param>
        /// <param name="value">The value to write.</param>
        public void WriteGuid(string key, Guid value) => WriteGuid(new DataIdentifier(key), value);

        /// <summary>
        /// Writes a globally unique identifier.
        /// </summary>
        /// <param name="identifier">The identifier of the value.</param>
        /// <param name="value">The value to write.</param>
        public abstract void WriteGuid(DataIdentifier identifier, Guid value);

        /// <summary>
        /// Writes an enumeration value.
        /// </summary>
        /// <typeparam name="TEnum">The enumeration type.</typeparam>
        /// <param name="key">The key identifying the value.</param>
        /// <param name="value">The value to write.</param>
        public void WriteEnum<TEnum>(string key, TEnum value) where TEnum : struct, Enum => WriteEnum(new DataIdentifier(key), value);

        /// <summary>
        /// Writes an enumeration value.
        /// </summary>
        /// <typeparam name="TEnum">The enumeration type.</typeparam>
        /// <param name="identifier">The identifier of the value.</param>
        /// <param name="value">The value to write.</param>
        public abstract void WriteEnum<TEnum>(DataIdentifier identifier, TEnum value) where TEnum : struct, Enum;

        /// <summary>
        /// Writes an object.
        /// </summary>
        /// <param name="key">The key identifying the object.</param>
        /// <param name="value">The object to write.</param>
        /// <param name="writerBefore">An optional callback invoked with the writer before the object is written.</param>
        public void WriteObject(string key, object? value, Action<FormatWriterBase>? writerBefore = null) => WriteObject(new DataIdentifier(key), value, writerBefore);

        /// <summary>
        /// Writes an object.
        /// </summary>
        /// <param name="identifier">The identifier of the object.</param>
        /// <param name="value">The object to write.</param>
        /// <param name="writerBefore">An optional callback invoked with the writer before the object is written.</param>
        public abstract void WriteObject(DataIdentifier identifier, object? value, Action<FormatWriterBase>? writerBefore = null);

        /// <summary>
        /// Writes nested data from another format writer.
        /// </summary>
        /// <param name="key">The key identifying the nested data.</param>
        /// <param name="value">The writer containing the nested data to write.</param>
        public void Write(string key, FormatWriterBase? value) => Write(new DataIdentifier(key), value);

        /// <summary>
        /// Writes nested data from another format writer.
        /// </summary>
        /// <param name="identifier">The identifier of the nested data.</param>
        /// <param name="value">The writer containing the nested data to write.</param>
        public abstract void Write(DataIdentifier identifier, FormatWriterBase? value);

        /// <summary>
        /// Writes an array of Boolean values.
        /// </summary>
        /// <param name="key">The key identifying the array.</param>
        /// <param name="values">The values to write.</param>
        public void WriteBooleanArray(string key, bool[]? values) => WriteBooleanArray(new DataIdentifier(key), values);

        /// <summary>
        /// Writes an array of Boolean values.
        /// </summary>
        /// <param name="identifier">The identifier of the array.</param>
        /// <param name="values">The values to write.</param>
        public abstract void WriteBooleanArray(DataIdentifier identifier, bool[]? values);

        /// <summary>
        /// Writes an array of unsigned 8-bit integers.
        /// </summary>
        /// <param name="key">The key identifying the array.</param>
        /// <param name="values">The values to write.</param>
        public void WriteByteArray(string key, byte[]? values) => WriteByteArray(new DataIdentifier(key), values);

        /// <summary>
        /// Writes an array of unsigned 8-bit integers.
        /// </summary>
        /// <param name="identifier">The identifier of the array.</param>
        /// <param name="values">The values to write.</param>
        public abstract void WriteByteArray(DataIdentifier identifier, byte[]? values);

        /// <summary>
        /// Writes an array of signed 8-bit integers.
        /// </summary>
        /// <param name="key">The key identifying the array.</param>
        /// <param name="values">The values to write.</param>
        public void WriteSByteArray(string key, sbyte[]? values) => WriteSByteArray(new DataIdentifier(key), values);

        /// <summary>
        /// Writes an array of signed 8-bit integers.
        /// </summary>
        /// <param name="identifier">The identifier of the array.</param>
        /// <param name="values">The values to write.</param>
        public abstract void WriteSByteArray(DataIdentifier identifier, sbyte[]? values);

        /// <summary>
        /// Writes an array of signed 16-bit integers.
        /// </summary>
        /// <param name="key">The key identifying the array.</param>
        /// <param name="values">The values to write.</param>
        public void WriteInt16Array(string key, short[]? values) => WriteInt16Array(new DataIdentifier(key), values);

        /// <summary>
        /// Writes an array of signed 16-bit integers.
        /// </summary>
        /// <param name="identifier">The identifier of the array.</param>
        /// <param name="values">The values to write.</param>
        public abstract void WriteInt16Array(DataIdentifier identifier, short[]? values);

        /// <summary>
        /// Writes an array of unsigned 16-bit integers.
        /// </summary>
        /// <param name="key">The key identifying the array.</param>
        /// <param name="values">The values to write.</param>
        public void WriteUInt16Array(string key, ushort[]? values) => WriteUInt16Array(new DataIdentifier(key), values);

        /// <summary>
        /// Writes an array of unsigned 16-bit integers.
        /// </summary>
        /// <param name="identifier">The identifier of the array.</param>
        /// <param name="values">The values to write.</param>
        public abstract void WriteUInt16Array(DataIdentifier identifier, ushort[]? values);

        /// <summary>
        /// Writes an array of signed 32-bit integers.
        /// </summary>
        /// <param name="key">The key identifying the array.</param>
        /// <param name="values">The values to write.</param>
        public void WriteInt32Array(string key, int[]? values) => WriteInt32Array(new DataIdentifier(key), values);

        /// <summary>
        /// Writes an array of signed 32-bit integers.
        /// </summary>
        /// <param name="identifier">The identifier of the array.</param>
        /// <param name="values">The values to write.</param>
        public abstract void WriteInt32Array(DataIdentifier identifier, int[]? values);

        /// <summary>
        /// Writes an array of unsigned 32-bit integers.
        /// </summary>
        /// <param name="key">The key identifying the array.</param>
        /// <param name="values">The values to write.</param>
        public void WriteUInt32Array(string key, uint[]? values) => WriteUInt32Array(new DataIdentifier(key), values);

        /// <summary>
        /// Writes an array of unsigned 32-bit integers.
        /// </summary>
        /// <param name="identifier">The identifier of the array.</param>
        /// <param name="values">The values to write.</param>
        public abstract void WriteUInt32Array(DataIdentifier identifier, uint[]? values);

        /// <summary>
        /// Writes an array of signed 64-bit integers.
        /// </summary>
        /// <param name="key">The key identifying the array.</param>
        /// <param name="values">The values to write.</param>
        public void WriteInt64Array(string key, long[]? values) => WriteInt64Array(new DataIdentifier(key), values);

        /// <summary>
        /// Writes an array of signed 64-bit integers.
        /// </summary>
        /// <param name="identifier">The identifier of the array.</param>
        /// <param name="values">The values to write.</param>
        public abstract void WriteInt64Array(DataIdentifier identifier, long[]? values);

        /// <summary>
        /// Writes an array of unsigned 64-bit integers.
        /// </summary>
        /// <param name="key">The key identifying the array.</param>
        /// <param name="values">The values to write.</param>
        public void WriteUInt64Array(string key, ulong[]? values) => WriteUInt64Array(new DataIdentifier(key), values);

        /// <summary>
        /// Writes an array of unsigned 64-bit integers.
        /// </summary>
        /// <param name="identifier">The identifier of the array.</param>
        /// <param name="values">The values to write.</param>
        public abstract void WriteUInt64Array(DataIdentifier identifier, ulong[]? values);

        /// <summary>
        /// Writes an array of single-precision floating-point values.
        /// </summary>
        /// <param name="key">The key identifying the array.</param>
        /// <param name="values">The values to write.</param>
        public void WriteSingleArray(string key, float[]? values) => WriteSingleArray(new DataIdentifier(key), values);

        /// <summary>
        /// Writes an array of single-precision floating-point values.
        /// </summary>
        /// <param name="identifier">The identifier of the array.</param>
        /// <param name="values">The values to write.</param>
        public abstract void WriteSingleArray(DataIdentifier identifier, float[]? values);

        /// <summary>
        /// Writes an array of double-precision floating-point values.
        /// </summary>
        /// <param name="key">The key identifying the array.</param>
        /// <param name="values">The values to write.</param>
        public void WriteDoubleArray(string key, double[]? values) => WriteDoubleArray(new DataIdentifier(key), values);

        /// <summary>
        /// Writes an array of double-precision floating-point values.
        /// </summary>
        /// <param name="identifier">The identifier of the array.</param>
        /// <param name="values">The values to write.</param>
        public abstract void WriteDoubleArray(DataIdentifier identifier, double[]? values);

        /// <summary>
        /// Writes an array of decimal values.
        /// </summary>
        /// <param name="key">The key identifying the array.</param>
        /// <param name="values">The values to write.</param>
        public void WriteDecimalArray(string key, decimal[]? values) => WriteDecimalArray(new DataIdentifier(key), values);

        /// <summary>
        /// Writes an array of decimal values.
        /// </summary>
        /// <param name="identifier">The identifier of the array.</param>
        /// <param name="values">The values to write.</param>
        public abstract void WriteDecimalArray(DataIdentifier identifier, decimal[]? values);

        /// <summary>
        /// Writes an array of character values.
        /// </summary>
        /// <param name="key">The key identifying the array.</param>
        /// <param name="values">The values to write.</param>
        public void WriteCharArray(string key, char[]? values) => WriteCharArray(new DataIdentifier(key), values);

        /// <summary>
        /// Writes an array of character values.
        /// </summary>
        /// <param name="identifier">The identifier of the array.</param>
        /// <param name="values">The values to write.</param>
        public abstract void WriteCharArray(DataIdentifier identifier, char[]? values);

        /// <summary>
        /// Writes an array of string values.
        /// </summary>
        /// <param name="key">The key identifying the array.</param>
        /// <param name="values">The values to write.</param>
        public void WriteStringArray(string key, string[]? values) => WriteStringArray(new DataIdentifier(key), values);

        /// <summary>
        /// Writes an array of string values.
        /// </summary>
        /// <param name="identifier">The identifier of the array.</param>
        /// <param name="values">The values to write.</param>
        public abstract void WriteStringArray(DataIdentifier identifier, string[]? values);

        /// <summary>
        /// Writes an array of time intervals.
        /// </summary>
        /// <param name="key">The key identifying the array.</param>
        /// <param name="values">The values to write.</param>
        public void WriteTimeSpanArray(string key, TimeSpan[]? values) => WriteTimeSpanArray(new DataIdentifier(key), values);

        /// <summary>
        /// Writes an array of time intervals.
        /// </summary>
        /// <param name="identifier">The identifier of the array.</param>
        /// <param name="values">The values to write.</param>
        public abstract void WriteTimeSpanArray(DataIdentifier identifier, TimeSpan[]? values);

        /// <summary>
        /// Writes an array of date and time values.
        /// </summary>
        /// <param name="key">The key identifying the array.</param>
        /// <param name="values">The values to write.</param>
        public void WriteDateTimeArray(string key, DateTime[]? values) => WriteDateTimeArray(new DataIdentifier(key), values);

        /// <summary>
        /// Writes an array of date and time values.
        /// </summary>
        /// <param name="identifier">The identifier of the array.</param>
        /// <param name="values">The values to write.</param>
        public abstract void WriteDateTimeArray(DataIdentifier identifier, DateTime[]? values);

        /// <summary>
        /// Writes an array of globally unique identifiers.
        /// </summary>
        /// <param name="key">The key identifying the array.</param>
        /// <param name="values">The values to write.</param>
        public void WriteGuidArray(string key, Guid[]? values) => WriteGuidArray(new DataIdentifier(key), values);

        /// <summary>
        /// Writes an array of globally unique identifiers.
        /// </summary>
        /// <param name="identifier">The identifier of the array.</param>
        /// <param name="values">The values to write.</param>
        public abstract void WriteGuidArray(DataIdentifier identifier, Guid[]? values);

        /// <summary>
        /// Writes an array of enumeration values.
        /// </summary>
        /// <typeparam name="TEnum">The enumeration type.</typeparam>
        /// <param name="key">The key identifying the array.</param>
        /// <param name="values">The values to write.</param>
        public void WriteEnumArray<TEnum>(string key, TEnum[]? values) where TEnum : struct, Enum => WriteEnumArray(new DataIdentifier(key), values);

        /// <summary>
        /// Writes an array of enumeration values.
        /// </summary>
        /// <typeparam name="TEnum">The enumeration type.</typeparam>
        /// <param name="identifier">The identifier of the array.</param>
        /// <param name="values">The values to write.</param>
        public abstract void WriteEnumArray<TEnum>(DataIdentifier identifier, TEnum[]? values) where TEnum : struct, Enum;

        /// <summary>
        /// Writes an array of objects.
        /// </summary>
        /// <param name="key">The key identifying the array.</param>
        /// <param name="values">The objects to write.</param>
        /// <param name="writerBefore">An optional callback invoked with the writer before each object is written.</param>
        public void WriteObjectArray(string key, object[]? values, Action<FormatWriterBase>? writerBefore = null) => WriteObjectArray(new DataIdentifier(key), values, writerBefore);

        /// <summary>
        /// Writes an array of objects.
        /// </summary>
        /// <param name="identifier">The identifier of the array.</param>
        /// <param name="values">The objects to write.</param>
        /// <param name="writerBefore">An optional callback invoked with the writer before each object is written.</param>
        public abstract void WriteObjectArray(DataIdentifier identifier, object[]? values, Action<FormatWriterBase>? writerBefore = null);

        /// <summary>
        /// Writes an array of nested data from format writers.
        /// </summary>
        /// <param name="key">The key identifying the array.</param>
        /// <param name="values">The writers containing the nested data to write.</param>
        public void WriteArray(string key, FormatWriterBase[]? values) => WriteArray(new DataIdentifier(key), values);

        /// <summary>
        /// Writes an array of nested data from format writers.
        /// </summary>
        /// <param name="identifier">The identifier of the array.</param>
        /// <param name="values">The writers containing the nested data to write.</param>
        public abstract void WriteArray(DataIdentifier identifier, FormatWriterBase[]? values);
    }
}