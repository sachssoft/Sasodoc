namespace Sachssoft.Documents.Formats.Binary;

internal enum BinaryValueType : byte
{
    Null, Boolean, Byte, SByte, Int16, UInt16, Int32, UInt32, Int64, UInt64, Single, Double, Decimal, Char, String, TimeSpan, DateTime, Guid, Enum, Object, Array,
    BooleanArray, ByteArray, SByteArray, Int16Array, UInt16Array, Int32Array, UInt32Array, Int64Array, UInt64Array, SingleArray, DoubleArray, DecimalArray, CharArray, StringArray, TimeSpanArray, DateTimeArray, GuidArray, EnumArray
}
