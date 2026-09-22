using System;

namespace Sachssoft.Documents
{
    /// <summary>
    /// Provides the base implementation for reading values, objects and arrays from a document format.
    /// </summary>
    public abstract class FormatReaderBase : FormatBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FormatReaderBase"/> class.
        /// </summary>
        protected FormatReaderBase() { }

        /// <summary>
        /// Creates a new reader of the concrete reader type.
        /// </summary>
        /// <returns>A new reader instance.</returns>
        /// <remarks>
        /// This factory is used internally when additional readers are required, for example when reading nested objects.
        /// </remarks>
        public abstract FormatReaderBase CreateReader();

        /// <summary>
        /// Determines whether data with the specified key exists.
        /// </summary>
        /// <param name="key">The key identifying the data.</param>
        /// <returns><see langword="true"/> if the data exists; otherwise, <see langword="false"/>.</returns>
        public bool Contains(string key) => Contains(new DataIdentifier(key));

        /// <summary>
        /// Determines whether data with the specified identifier exists.
        /// </summary>
        /// <param name="identifier">The identifier of the data.</param>
        /// <returns><see langword="true"/> if the data exists; otherwise, <see langword="false"/>.</returns>
        public abstract bool Contains(DataIdentifier identifier);

        /// <summary>
        /// Reads a Boolean value.
        /// </summary>
        /// <param name="key">The key identifying the value.</param>
        /// <param name="fallback">The value returned when the value cannot be read.</param>
        /// <returns>The read value or <paramref name="fallback"/>.</returns>
        public bool ReadBoolean(string key, bool fallback = default) => ReadBoolean(new DataIdentifier(key), fallback);

        /// <summary>
        /// Reads a Boolean value.
        /// </summary>
        /// <param name="identifier">The identifier of the value.</param>
        /// <param name="fallback">The value returned when the value cannot be read.</param>
        /// <returns>The read value or <paramref name="fallback"/>.</returns>
        public abstract bool ReadBoolean(DataIdentifier identifier, bool fallback = default);

        /// <summary>
        /// Reads an unsigned 8-bit integer.
        /// </summary>
        /// <param name="key">The key identifying the value.</param>
        /// <param name="fallback">The value returned when the value cannot be read.</param>
        /// <returns>The read value or <paramref name="fallback"/>.</returns>
        public byte ReadByte(string key, byte fallback = default) => ReadByte(new DataIdentifier(key), fallback);

        /// <summary>
        /// Reads an unsigned 8-bit integer.
        /// </summary>
        /// <param name="identifier">The identifier of the value.</param>
        /// <param name="fallback">The value returned when the value cannot be read.</param>
        /// <returns>The read value or <paramref name="fallback"/>.</returns>
        public abstract byte ReadByte(DataIdentifier identifier, byte fallback = default);

        /// <summary>
        /// Reads a signed 8-bit integer.
        /// </summary>
        /// <param name="key">The key identifying the value.</param>
        /// <param name="fallback">The value returned when the value cannot be read.</param>
        /// <returns>The read value or <paramref name="fallback"/>.</returns>
        public sbyte ReadSByte(string key, sbyte fallback = default) => ReadSByte(new DataIdentifier(key), fallback);

        /// <summary>
        /// Reads a signed 8-bit integer.
        /// </summary>
        /// <param name="identifier">The identifier of the value.</param>
        /// <param name="fallback">The value returned when the value cannot be read.</param>
        /// <returns>The read value or <paramref name="fallback"/>.</returns>
        public abstract sbyte ReadSByte(DataIdentifier identifier, sbyte fallback = default);

        /// <summary>
        /// Reads a signed 16-bit integer.
        /// </summary>
        /// <param name="key">The key identifying the value.</param>
        /// <param name="fallback">The value returned when the value cannot be read.</param>
        /// <returns>The read value or <paramref name="fallback"/>.</returns>
        public short ReadInt16(string key, short fallback = default) => ReadInt16(new DataIdentifier(key), fallback);

        /// <summary>
        /// Reads a signed 16-bit integer.
        /// </summary>
        /// <param name="identifier">The identifier of the value.</param>
        /// <param name="fallback">The value returned when the value cannot be read.</param>
        /// <returns>The read value or <paramref name="fallback"/>.</returns>
        public abstract short ReadInt16(DataIdentifier identifier, short fallback = default);

        /// <summary>
        /// Reads an unsigned 16-bit integer.
        /// </summary>
        /// <param name="key">The key identifying the value.</param>
        /// <param name="fallback">The value returned when the value cannot be read.</param>
        /// <returns>The read value or <paramref name="fallback"/>.</returns>
        public ushort ReadUInt16(string key, ushort fallback = default) => ReadUInt16(new DataIdentifier(key), fallback);

        /// <summary>
        /// Reads an unsigned 16-bit integer.
        /// </summary>
        /// <param name="identifier">The identifier of the value.</param>
        /// <param name="fallback">The value returned when the value cannot be read.</param>
        /// <returns>The read value or <paramref name="fallback"/>.</returns>
        public abstract ushort ReadUInt16(DataIdentifier identifier, ushort fallback = default);

        /// <summary>
        /// Reads a signed 32-bit integer.
        /// </summary>
        /// <param name="key">The key identifying the value.</param>
        /// <param name="fallback">The value returned when the value cannot be read.</param>
        /// <returns>The read value or <paramref name="fallback"/>.</returns>
        public int ReadInt32(string key, int fallback = default) => ReadInt32(new DataIdentifier(key), fallback);

        /// <summary>
        /// Reads a signed 32-bit integer.
        /// </summary>
        /// <param name="identifier">The identifier of the value.</param>
        /// <param name="fallback">The value returned when the value cannot be read.</param>
        /// <returns>The read value or <paramref name="fallback"/>.</returns>
        public abstract int ReadInt32(DataIdentifier identifier, int fallback = default);

        /// <summary>
        /// Reads an unsigned 32-bit integer.
        /// </summary>
        /// <param name="key">The key identifying the value.</param>
        /// <param name="fallback">The value returned when the value cannot be read.</param>
        /// <returns>The read value or <paramref name="fallback"/>.</returns>
        public uint ReadUInt32(string key, uint fallback = default) => ReadUInt32(new DataIdentifier(key), fallback);

        /// <summary>
        /// Reads an unsigned 32-bit integer.
        /// </summary>
        /// <param name="identifier">The identifier of the value.</param>
        /// <param name="fallback">The value returned when the value cannot be read.</param>
        /// <returns>The read value or <paramref name="fallback"/>.</returns>
        public abstract uint ReadUInt32(DataIdentifier identifier, uint fallback = default);

        /// <summary>
        /// Reads a signed 64-bit integer.
        /// </summary>
        /// <param name="key">The key identifying the value.</param>
        /// <param name="fallback">The value returned when the value cannot be read.</param>
        /// <returns>The read value or <paramref name="fallback"/>.</returns>
        public long ReadInt64(string key, long fallback = default) => ReadInt64(new DataIdentifier(key), fallback);

        /// <summary>
        /// Reads a signed 64-bit integer.
        /// </summary>
        /// <param name="identifier">The identifier of the value.</param>
        /// <param name="fallback">The value returned when the value cannot be read.</param>
        /// <returns>The read value or <paramref name="fallback"/>.</returns>
        public abstract long ReadInt64(DataIdentifier identifier, long fallback = default);

        /// <summary>
        /// Reads an unsigned 64-bit integer.
        /// </summary>
        /// <param name="key">The key identifying the value.</param>
        /// <param name="fallback">The value returned when the value cannot be read.</param>
        /// <returns>The read value or <paramref name="fallback"/>.</returns>
        public ulong ReadUInt64(string key, ulong fallback = default) => ReadUInt64(new DataIdentifier(key), fallback);

        /// <summary>
        /// Reads an unsigned 64-bit integer.
        /// </summary>
        /// <param name="identifier">The identifier of the value.</param>
        /// <param name="fallback">The value returned when the value cannot be read.</param>
        /// <returns>The read value or <paramref name="fallback"/>.</returns>
        public abstract ulong ReadUInt64(DataIdentifier identifier, ulong fallback = default);

        /// <summary>
        /// Reads a single-precision floating-point value.
        /// </summary>
        /// <param name="key">The key identifying the value.</param>
        /// <param name="fallback">The value returned when the value cannot be read.</param>
        /// <returns>The read value or <paramref name="fallback"/>.</returns>
        public float ReadSingle(string key, float fallback = default) => ReadSingle(new DataIdentifier(key), fallback);

        /// <summary>
        /// Reads a single-precision floating-point value.
        /// </summary>
        /// <param name="identifier">The identifier of the value.</param>
        /// <param name="fallback">The value returned when the value cannot be read.</param>
        /// <returns>The read value or <paramref name="fallback"/>.</returns>
        public abstract float ReadSingle(DataIdentifier identifier, float fallback = default);

        /// <summary>
        /// Reads a double-precision floating-point value.
        /// </summary>
        /// <param name="key">The key identifying the value.</param>
        /// <param name="fallback">The value returned when the value cannot be read.</param>
        /// <returns>The read value or <paramref name="fallback"/>.</returns>
        public double ReadDouble(string key, double fallback = default) => ReadDouble(new DataIdentifier(key), fallback);

        /// <summary>
        /// Reads a double-precision floating-point value.
        /// </summary>
        /// <param name="identifier">The identifier of the value.</param>
        /// <param name="fallback">The value returned when the value cannot be read.</param>
        /// <returns>The read value or <paramref name="fallback"/>.</returns>
        public abstract double ReadDouble(DataIdentifier identifier, double fallback = default);

        /// <summary>
        /// Reads a decimal value.
        /// </summary>
        /// <param name="key">The key identifying the value.</param>
        /// <param name="fallback">The value returned when the value cannot be read.</param>
        /// <returns>The read value or <paramref name="fallback"/>.</returns>
        public decimal ReadDecimal(string key, decimal fallback = default) => ReadDecimal(new DataIdentifier(key), fallback);

        /// <summary>
        /// Reads a decimal value.
        /// </summary>
        /// <param name="identifier">The identifier of the value.</param>
        /// <param name="fallback">The value returned when the value cannot be read.</param>
        /// <returns>The read value or <paramref name="fallback"/>.</returns>
        public abstract decimal ReadDecimal(DataIdentifier identifier, decimal fallback = default);

        /// <summary>
        /// Reads a character value.
        /// </summary>
        /// <param name="key">The key identifying the value.</param>
        /// <param name="fallback">The value returned when the value cannot be read.</param>
        /// <returns>The read value or <paramref name="fallback"/>.</returns>
        public char ReadChar(string key, char fallback = default) => ReadChar(new DataIdentifier(key), fallback);

        /// <summary>
        /// Reads a character value.
        /// </summary>
        /// <param name="identifier">The identifier of the value.</param>
        /// <param name="fallback">The value returned when the value cannot be read.</param>
        /// <returns>The read value or <paramref name="fallback"/>.</returns>
        public abstract char ReadChar(DataIdentifier identifier, char fallback = default);

        /// <summary>
        /// Reads a string value.
        /// </summary>
        /// <param name="key">The key identifying the value.</param>
        /// <param name="fallback">The value returned when the value cannot be read.</param>
        /// <returns>The read value or <paramref name="fallback"/>.</returns>
        public string? ReadString(string key, string? fallback = default) => ReadString(new DataIdentifier(key), fallback);

        /// <summary>
        /// Reads a string value.
        /// </summary>
        /// <param name="identifier">The identifier of the value.</param>
        /// <param name="fallback">The value returned when the value cannot be read.</param>
        /// <returns>The read value or <paramref name="fallback"/>.</returns>
        public abstract string? ReadString(DataIdentifier identifier, string? fallback = default);

        /// <summary>
        /// Reads a time interval.
        /// </summary>
        /// <param name="key">The key identifying the value.</param>
        /// <param name="fallback">The value returned when the value cannot be read.</param>
        /// <returns>The read value or <paramref name="fallback"/>.</returns>
        public TimeSpan ReadTimeSpan(string key, TimeSpan fallback = default) => ReadTimeSpan(new DataIdentifier(key), fallback);

        /// <summary>
        /// Reads a time interval.
        /// </summary>
        /// <param name="identifier">The identifier of the value.</param>
        /// <param name="fallback">The value returned when the value cannot be read.</param>
        /// <returns>The read value or <paramref name="fallback"/>.</returns>
        public abstract TimeSpan ReadTimeSpan(DataIdentifier identifier, TimeSpan fallback = default);

        /// <summary>
        /// Reads a date and time value.
        /// </summary>
        /// <param name="key">The key identifying the value.</param>
        /// <param name="fallback">The value returned when the value cannot be read.</param>
        /// <returns>The read value or <paramref name="fallback"/>.</returns>
        public DateTime ReadDateTime(string key, DateTime fallback = default) => ReadDateTime(new DataIdentifier(key), fallback);

        /// <summary>
        /// Reads a date and time value.
        /// </summary>
        /// <param name="identifier">The identifier of the value.</param>
        /// <param name="fallback">The value returned when the value cannot be read.</param>
        /// <returns>The read value or <paramref name="fallback"/>.</returns>
        public abstract DateTime ReadDateTime(DataIdentifier identifier, DateTime fallback = default);

        /// <summary>
        /// Reads a globally unique identifier.
        /// </summary>
        /// <param name="key">The key identifying the value.</param>
        /// <param name="fallback">The value returned when the value cannot be read.</param>
        /// <returns>The read value or <paramref name="fallback"/>.</returns>
        public Guid ReadGuid(string key, Guid fallback = default) => ReadGuid(new DataIdentifier(key), fallback);

        /// <summary>
        /// Reads a globally unique identifier.
        /// </summary>
        /// <param name="identifier">The identifier of the value.</param>
        /// <param name="fallback">The value returned when the value cannot be read.</param>
        /// <returns>The read value or <paramref name="fallback"/>.</returns>
        public abstract Guid ReadGuid(DataIdentifier identifier, Guid fallback = default);

        /// <summary>
        /// Reads an enumeration value.
        /// </summary>
        /// <typeparam name="TEnum">The enumeration type.</typeparam>
        /// <param name="key">The key identifying the value.</param>
        /// <param name="fallback">The value returned when the value cannot be read.</param>
        /// <returns>The read value or <paramref name="fallback"/>.</returns>
        public TEnum ReadEnum<TEnum>(string key, TEnum fallback = default) where TEnum : struct, Enum => ReadEnum(new DataIdentifier(key), fallback);

        /// <summary>
        /// Reads an enumeration value.
        /// </summary>
        /// <typeparam name="TEnum">The enumeration type.</typeparam>
        /// <param name="identifier">The identifier of the value.</param>
        /// <param name="fallback">The value returned when the value cannot be read.</param>
        /// <returns>The read value or <paramref name="fallback"/>.</returns>
        public abstract TEnum ReadEnum<TEnum>(DataIdentifier identifier, TEnum fallback = default) where TEnum : struct, Enum;

        /// <summary>
        /// Reads an object using the specified object reader generator.
        /// </summary>
        /// <param name="key">The key identifying the object.</param>
        /// <param name="generator">The callback used to generate the object reader.</param>
        /// <param name="fallback">The value returned when the object cannot be read.</param>
        /// <returns>The read object or <paramref name="fallback"/>.</returns>
        public object? ReadObject(string key, ObjectReaderGeneratorCallback generator, object? fallback = default) => ReadObject(new DataIdentifier(key), generator, fallback);

        /// <summary>
        /// Reads an object using the specified object reader generator.
        /// </summary>
        /// <param name="identifier">The identifier of the object.</param>
        /// <param name="generator">The callback used to generate the object reader.</param>
        /// <param name="fallback">The value returned when the object cannot be read.</param>
        /// <returns>The read object or <paramref name="fallback"/>.</returns>
        public abstract object? ReadObject(DataIdentifier identifier, ObjectReaderGeneratorCallback generator, object? fallback = default);

        /// <summary>
        /// Reads nested data and returns a reader for it.
        /// </summary>
        /// <param name="key">The key identifying the nested data.</param>
        /// <returns>A reader for the nested data.</returns>
        public FormatReaderBase Read(string key) => Read(new DataIdentifier(key));

        /// <summary>
        /// Reads nested data and returns a reader for it.
        /// </summary>
        /// <param name="identifier">The identifier of the nested data.</param>
        /// <returns>A reader for the nested data.</returns>
        public abstract FormatReaderBase Read(DataIdentifier identifier);

        /// <summary>
        /// Reads an array of Boolean values.
        /// </summary>
        /// <param name="key">The key identifying the array.</param>
        /// <param name="fallback">The value returned when the array cannot be read.</param>
        /// <returns>The read array or <paramref name="fallback"/>.</returns>
        public bool[] ReadBooleanArray(string key, bool[]? fallback = default) => ReadBooleanArray(new DataIdentifier(key), fallback);

        /// <summary>
        /// Reads an array of Boolean values.
        /// </summary>
        /// <param name="identifier">The identifier of the array.</param>
        /// <param name="fallback">The value returned when the array cannot be read.</param>
        /// <returns>The read array or <paramref name="fallback"/>.</returns>
        public abstract bool[] ReadBooleanArray(DataIdentifier identifier, bool[]? fallback = default);

        /// <summary>
        /// Reads an array of unsigned 8-bit integers.
        /// </summary>
        /// <param name="key">The key identifying the array.</param>
        /// <param name="fallback">The value returned when the array cannot be read.</param>
        /// <returns>The read array or <paramref name="fallback"/>.</returns>
        public byte[] ReadByteArray(string key, byte[]? fallback = default) => ReadByteArray(new DataIdentifier(key), fallback);

        /// <summary>
        /// Reads an array of unsigned 8-bit integers.
        /// </summary>
        /// <param name="identifier">The identifier of the array.</param>
        /// <param name="fallback">The value returned when the array cannot be read.</param>
        /// <returns>The read array or <paramref name="fallback"/>.</returns>
        public abstract byte[] ReadByteArray(DataIdentifier identifier, byte[]? fallback = default);

        /// <summary>
        /// Reads an array of signed 8-bit integers.
        /// </summary>
        /// <param name="key">The key identifying the array.</param>
        /// <param name="fallback">The value returned when the array cannot be read.</param>
        /// <returns>The read array or <paramref name="fallback"/>.</returns>
        public sbyte[] ReadSByteArray(string key, sbyte[]? fallback = default) => ReadSByteArray(new DataIdentifier(key), fallback);

        /// <summary>
        /// Reads an array of signed 8-bit integers.
        /// </summary>
        /// <param name="identifier">The identifier of the array.</param>
        /// <param name="fallback">The value returned when the array cannot be read.</param>
        /// <returns>The read array or <paramref name="fallback"/>.</returns>
        public abstract sbyte[] ReadSByteArray(DataIdentifier identifier, sbyte[]? fallback = default);

        /// <summary>
        /// Reads an array of signed 16-bit integers.
        /// </summary>
        /// <param name="key">The key identifying the array.</param>
        /// <param name="fallback">The value returned when the array cannot be read.</param>
        /// <returns>The read array or <paramref name="fallback"/>.</returns>
        public short[] ReadInt16Array(string key, short[]? fallback = default) => ReadInt16Array(new DataIdentifier(key), fallback);

        /// <summary>
        /// Reads an array of signed 16-bit integers.
        /// </summary>
        /// <param name="identifier">The identifier of the array.</param>
        /// <param name="fallback">The value returned when the array cannot be read.</param>
        /// <returns>The read array or <paramref name="fallback"/>.</returns>
        public abstract short[] ReadInt16Array(DataIdentifier identifier, short[]? fallback = default);

        /// <summary>
        /// Reads an array of unsigned 16-bit integers.
        /// </summary>
        /// <param name="key">The key identifying the array.</param>
        /// <param name="fallback">The value returned when the array cannot be read.</param>
        /// <returns>The read array or <paramref name="fallback"/>.</returns>
        public ushort[] ReadUInt16Array(string key, ushort[]? fallback = default) => ReadUInt16Array(new DataIdentifier(key), fallback);

        /// <summary>
        /// Reads an array of unsigned 16-bit integers.
        /// </summary>
        /// <param name="identifier">The identifier of the array.</param>
        /// <param name="fallback">The value returned when the array cannot be read.</param>
        /// <returns>The read array or <paramref name="fallback"/>.</returns>
        public abstract ushort[] ReadUInt16Array(DataIdentifier identifier, ushort[]? fallback = default);

        /// <summary>
        /// Reads an array of signed 32-bit integers.
        /// </summary>
        /// <param name="key">The key identifying the array.</param>
        /// <param name="fallback">The value returned when the array cannot be read.</param>
        /// <returns>The read array or <paramref name="fallback"/>.</returns>
        public int[] ReadInt32Array(string key, int[]? fallback = default) => ReadInt32Array(new DataIdentifier(key), fallback);

        /// <summary>
        /// Reads an array of signed 32-bit integers.
        /// </summary>
        /// <param name="identifier">The identifier of the array.</param>
        /// <param name="fallback">The value returned when the array cannot be read.</param>
        /// <returns>The read array or <paramref name="fallback"/>.</returns>
        public abstract int[] ReadInt32Array(DataIdentifier identifier, int[]? fallback = default);

        /// <summary>
        /// Reads an array of unsigned 32-bit integers.
        /// </summary>
        /// <param name="key">The key identifying the array.</param>
        /// <param name="fallback">The value returned when the array cannot be read.</param>
        /// <returns>The read array or <paramref name="fallback"/>.</returns>
        public uint[] ReadUInt32Array(string key, uint[]? fallback = default) => ReadUInt32Array(new DataIdentifier(key), fallback);

        /// <summary>
        /// Reads an array of unsigned 32-bit integers.
        /// </summary>
        /// <param name="identifier">The identifier of the array.</param>
        /// <param name="fallback">The value returned when the array cannot be read.</param>
        /// <returns>The read array or <paramref name="fallback"/>.</returns>
        public abstract uint[] ReadUInt32Array(DataIdentifier identifier, uint[]? fallback = default);

        /// <summary>
        /// Reads an array of signed 64-bit integers.
        /// </summary>
        /// <param name="key">The key identifying the array.</param>
        /// <param name="fallback">The value returned when the array cannot be read.</param>
        /// <returns>The read array or <paramref name="fallback"/>.</returns>
        public long[] ReadInt64Array(string key, long[]? fallback = default) => ReadInt64Array(new DataIdentifier(key), fallback);

        /// <summary>
        /// Reads an array of signed 64-bit integers.
        /// </summary>
        /// <param name="identifier">The identifier of the array.</param>
        /// <param name="fallback">The value returned when the array cannot be read.</param>
        /// <returns>The read array or <paramref name="fallback"/>.</returns>
        public abstract long[] ReadInt64Array(DataIdentifier identifier, long[]? fallback = default);

        /// <summary>
        /// Reads an array of unsigned 64-bit integers.
        /// </summary>
        /// <param name="key">The key identifying the array.</param>
        /// <param name="fallback">The value returned when the array cannot be read.</param>
        /// <returns>The read array or <paramref name="fallback"/>.</returns>
        public ulong[] ReadUInt64Array(string key, ulong[]? fallback = default) => ReadUInt64Array(new DataIdentifier(key), fallback);

        /// <summary>
        /// Reads an array of unsigned 64-bit integers.
        /// </summary>
        /// <param name="identifier">The identifier of the array.</param>
        /// <param name="fallback">The value returned when the array cannot be read.</param>
        /// <returns>The read array or <paramref name="fallback"/>.</returns>
        public abstract ulong[] ReadUInt64Array(DataIdentifier identifier, ulong[]? fallback = default);

        /// <summary>
        /// Reads an array of single-precision floating-point values.
        /// </summary>
        /// <param name="key">The key identifying the array.</param>
        /// <param name="fallback">The value returned when the array cannot be read.</param>
        /// <returns>The read array or <paramref name="fallback"/>.</returns>
        public float[] ReadSingleArray(string key, float[]? fallback = default) => ReadSingleArray(new DataIdentifier(key), fallback);

        /// <summary>
        /// Reads an array of single-precision floating-point values.
        /// </summary>
        /// <param name="identifier">The identifier of the array.</param>
        /// <param name="fallback">The value returned when the array cannot be read.</param>
        /// <returns>The read array or <paramref name="fallback"/>.</returns>
        public abstract float[] ReadSingleArray(DataIdentifier identifier, float[]? fallback = default);

        /// <summary>
        /// Reads an array of double-precision floating-point values.
        /// </summary>
        /// <param name="key">The key identifying the array.</param>
        /// <param name="fallback">The value returned when the array cannot be read.</param>
        /// <returns>The read array or <paramref name="fallback"/>.</returns>
        public double[] ReadDoubleArray(string key, double[]? fallback = default) => ReadDoubleArray(new DataIdentifier(key), fallback);

        /// <summary>
        /// Reads an array of double-precision floating-point values.
        /// </summary>
        /// <param name="identifier">The identifier of the array.</param>
        /// <param name="fallback">The value returned when the array cannot be read.</param>
        /// <returns>The read array or <paramref name="fallback"/>.</returns>
        public abstract double[] ReadDoubleArray(DataIdentifier identifier, double[]? fallback = default);

        /// <summary>
        /// Reads an array of decimal values.
        /// </summary>
        /// <param name="key">The key identifying the array.</param>
        /// <param name="fallback">The value returned when the array cannot be read.</param>
        /// <returns>The read array or <paramref name="fallback"/>.</returns>
        public decimal[] ReadDecimalArray(string key, decimal[]? fallback = default) => ReadDecimalArray(new DataIdentifier(key), fallback);

        /// <summary>
        /// Reads an array of decimal values.
        /// </summary>
        /// <param name="identifier">The identifier of the array.</param>
        /// <param name="fallback">The value returned when the array cannot be read.</param>
        /// <returns>The read array or <paramref name="fallback"/>.</returns>
        public abstract decimal[] ReadDecimalArray(DataIdentifier identifier, decimal[]? fallback = default);

        /// <summary>
        /// Reads an array of character values.
        /// </summary>
        /// <param name="key">The key identifying the array.</param>
        /// <param name="fallback">The value returned when the array cannot be read.</param>
        /// <returns>The read array or <paramref name="fallback"/>.</returns>
        public char[] ReadCharArray(string key, char[]? fallback = default) => ReadCharArray(new DataIdentifier(key), fallback);

        /// <summary>
        /// Reads an array of character values.
        /// </summary>
        /// <param name="identifier">The identifier of the array.</param>
        /// <param name="fallback">The value returned when the array cannot be read.</param>
        /// <returns>The read array or <paramref name="fallback"/>.</returns>
        public abstract char[] ReadCharArray(DataIdentifier identifier, char[]? fallback = default);

        /// <summary>
        /// Reads an array of string values.
        /// </summary>
        /// <param name="key">The key identifying the array.</param>
        /// <param name="fallback">The value returned when the array cannot be read.</param>
        /// <returns>The read array or <paramref name="fallback"/>.</returns>
        public string[] ReadStringArray(string key, string[]? fallback = default) => ReadStringArray(new DataIdentifier(key), fallback);

        /// <summary>
        /// Reads an array of string values.
        /// </summary>
        /// <param name="identifier">The identifier of the array.</param>
        /// <param name="fallback">The value returned when the array cannot be read.</param>
        /// <returns>The read array or <paramref name="fallback"/>.</returns>
        public abstract string[] ReadStringArray(DataIdentifier identifier, string[]? fallback = default);

        /// <summary>
        /// Reads an array of time intervals.
        /// </summary>
        /// <param name="key">The key identifying the array.</param>
        /// <param name="fallback">The value returned when the array cannot be read.</param>
        /// <returns>The read array or <paramref name="fallback"/>.</returns>
        public TimeSpan[] ReadTimeSpanArray(string key, TimeSpan[]? fallback = default) => ReadTimeSpanArray(new DataIdentifier(key), fallback);

        /// <summary>
        /// Reads an array of time intervals.
        /// </summary>
        /// <param name="identifier">The identifier of the array.</param>
        /// <param name="fallback">The value returned when the array cannot be read.</param>
        /// <returns>The read array or <paramref name="fallback"/>.</returns>
        public abstract TimeSpan[] ReadTimeSpanArray(DataIdentifier identifier, TimeSpan[]? fallback = default);

        /// <summary>
        /// Reads an array of date and time values.
        /// </summary>
        /// <param name="key">The key identifying the array.</param>
        /// <param name="fallback">The value returned when the array cannot be read.</param>
        /// <returns>The read array or <paramref name="fallback"/>.</returns>
        public DateTime[] ReadDateTimeArray(string key, DateTime[]? fallback = default) => ReadDateTimeArray(new DataIdentifier(key), fallback);

        /// <summary>
        /// Reads an array of date and time values.
        /// </summary>
        /// <param name="identifier">The identifier of the array.</param>
        /// <param name="fallback">The value returned when the array cannot be read.</param>
        /// <returns>The read array or <paramref name="fallback"/>.</returns>
        public abstract DateTime[] ReadDateTimeArray(DataIdentifier identifier, DateTime[]? fallback = default);

        /// <summary>
        /// Reads an array of globally unique identifiers.
        /// </summary>
        /// <param name="key">The key identifying the array.</param>
        /// <param name="fallback">The value returned when the array cannot be read.</param>
        /// <returns>The read array or <paramref name="fallback"/>.</returns>
        public Guid[] ReadGuidArray(string key, Guid[]? fallback = default) => ReadGuidArray(new DataIdentifier(key), fallback);

        /// <summary>
        /// Reads an array of globally unique identifiers.
        /// </summary>
        /// <param name="identifier">The identifier of the array.</param>
        /// <param name="fallback">The value returned when the array cannot be read.</param>
        /// <returns>The read array or <paramref name="fallback"/>.</returns>
        public abstract Guid[] ReadGuidArray(DataIdentifier identifier, Guid[]? fallback = default);

        /// <summary>
        /// Reads an array of enumeration values.
        /// </summary>
        /// <typeparam name="TEnum">The enumeration type.</typeparam>
        /// <param name="key">The key identifying the array.</param>
        /// <param name="fallback">The value returned when the array cannot be read.</param>
        /// <returns>The read array or <paramref name="fallback"/>.</returns>
        public TEnum[] ReadEnumArray<TEnum>(string key, TEnum[]? fallback = default) where TEnum : struct, Enum => ReadEnumArray(new DataIdentifier(key), fallback);

        /// <summary>
        /// Reads an array of enumeration values.
        /// </summary>
        /// <typeparam name="TEnum">The enumeration type.</typeparam>
        /// <param name="identifier">The identifier of the array.</param>
        /// <param name="fallback">The value returned when the array cannot be read.</param>
        /// <returns>The read array or <paramref name="fallback"/>.</returns>
        public abstract TEnum[] ReadEnumArray<TEnum>(DataIdentifier identifier, TEnum[]? fallback = default) where TEnum : struct, Enum;

        /// <summary>
        /// Reads an array of objects using the specified object reader generator.
        /// </summary>
        /// <param name="key">The key identifying the array.</param>
        /// <param name="generator">The callback used to generate object readers.</param>
        /// <param name="fallback">The value returned when the array cannot be read.</param>
        /// <returns>The read array or <paramref name="fallback"/>.</returns>
        public object[] ReadObjectArray(string key, ObjectReaderGeneratorCallback generator, object[]? fallback = default) => ReadObjectArray(new DataIdentifier(key), generator, fallback);

        /// <summary>
        /// Reads an array of objects using the specified object reader generator.
        /// </summary>
        /// <param name="identifier">The identifier of the array.</param>
        /// <param name="generator">The callback used to generate object readers.</param>
        /// <param name="fallback">The value returned when the array cannot be read.</param>
        /// <returns>The read array or <paramref name="fallback"/>.</returns>
        public abstract object[] ReadObjectArray(DataIdentifier identifier, ObjectReaderGeneratorCallback generator, object[]? fallback = default);

        /// <summary>
        /// Reads an array of nested data and returns readers for its elements.
        /// </summary>
        /// <param name="key">The key identifying the array.</param>
        /// <returns>An array containing readers for the nested data.</returns>
        public FormatReaderBase[] ReadArray(string key) => ReadArray(new DataIdentifier(key));

        /// <summary>
        /// Reads an array of nested data and returns readers for its elements.
        /// </summary>
        /// <param name="identifier">The identifier of the array.</param>
        /// <returns>An array containing readers for the nested data.</returns>
        public abstract FormatReaderBase[] ReadArray(DataIdentifier identifier);
    }
}