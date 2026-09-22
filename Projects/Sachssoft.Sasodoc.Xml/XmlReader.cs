using System;
using System.Collections.Generic;
using System.Globalization;
using System.Xml;
using System.Xml.Linq;

namespace Sachssoft.Documents.Formats.Xml;

/// <summary>
/// Reads document data from an XML element tree using attributes for primitive values.
/// </summary>
public class XmlReader : FormatReader<XmlReader>
{
    private XElement _node;

    /// <summary>
    /// Initializes a new instance of the <see cref="XmlReader"/> class.
    /// </summary>
    public XmlReader() : this(new XElement("document")) { }

    internal XmlReader(XElement node)
    {
        _node = node ?? throw new ArgumentNullException(nameof(node));
    }

    /// <summary>
    /// Gets or sets the XML element represented by this reader.
    /// </summary>
    internal XElement Node
    {
        get => _node;
        set => _node = value ?? throw new ArgumentNullException(nameof(value));
    }

    /// <summary>
    /// Gets or sets how failed primitive value conversions are handled.
    /// </summary>
    public ConversionErrorHandling ConversionErrorHandling { get; set; }

    /// <inheritdoc />
    public override XmlReader CreateReader() => new(new XElement("object")) { ConversionErrorHandling = ConversionErrorHandling, Options = Options };

    private string GetPropertyName(DataIdentifier identifier) => XmlConvert.EncodeLocalName(ConvertPropertyIdentifier(identifier).Value);

    /// <inheritdoc />
    public override bool Contains(DataIdentifier identifier)
    {
        var name = GetPropertyName(identifier);
        return _node.Attribute(name) != null || _node.Element(name) != null;
    }

    private T ReadValue<T>(DataIdentifier identifier, T fallback, Func<string, T> converter)
    {
        var attribute = _node.Attribute(GetPropertyName(identifier));
        if (attribute == null) return fallback;
        try { return converter(attribute.Value); }
        catch (Exception exception) when (exception is FormatException or OverflowException or ArgumentException)
        {
            return ConversionErrorHandling == ConversionErrorHandling.Default ? default! : fallback;
        }
    }

    private T[] ReadPrimitiveArray<T>(DataIdentifier identifier, T[]? fallback, Func<string, T> converter, T replacementValue = default!)
    {
        fallback ??= Array.Empty<T>();
        var container = _node.Element(GetPropertyName(identifier));
        if (container == null) return fallback;
        var result = new List<T>();
        foreach (var item in container.Elements("item"))
        {
            var attribute = item.Attribute("value");
            if (attribute != null)
            {
                try { result.Add(converter(attribute.Value)); continue; }
                catch (Exception exception) when (exception is FormatException or OverflowException or ArgumentException) { }
            }
            switch (ConversionErrorHandling)
            {
                case ConversionErrorHandling.Ignore: continue;
                case ConversionErrorHandling.Cancel: return fallback;
                case ConversionErrorHandling.Replace: result.Add(replacementValue); continue;
                case ConversionErrorHandling.Default: result.Add(default!); continue;
                default: return fallback;
            }
        }
        return result.ToArray();
    }

    private bool TryFindEnumValue<TEnum>(string value, out TEnum result) where TEnum : struct, Enum
    {
        foreach (var enumValue in Enum.GetValues<TEnum>())
        {
            if (string.Equals(ConvertFieldName(enumValue.ToString()), value, StringComparison.Ordinal))
            {
                result = enumValue;
                return true;
            }
        }
        result = default;
        return false;
    }

    /// <inheritdoc />
    public override bool ReadBoolean(DataIdentifier identifier, bool fallback = default) => ReadValue(identifier, fallback, XmlConvert.ToBoolean);
    /// <inheritdoc />
    public override byte ReadByte(DataIdentifier identifier, byte fallback = default) => ReadValue(identifier, fallback, XmlConvert.ToByte);
    /// <inheritdoc />
    public override sbyte ReadSByte(DataIdentifier identifier, sbyte fallback = default) => ReadValue(identifier, fallback, XmlConvert.ToSByte);
    /// <inheritdoc />
    public override short ReadInt16(DataIdentifier identifier, short fallback = default) => ReadValue(identifier, fallback, XmlConvert.ToInt16);
    /// <inheritdoc />
    public override ushort ReadUInt16(DataIdentifier identifier, ushort fallback = default) => ReadValue(identifier, fallback, XmlConvert.ToUInt16);
    /// <inheritdoc />
    public override int ReadInt32(DataIdentifier identifier, int fallback = default) => ReadValue(identifier, fallback, XmlConvert.ToInt32);
    /// <inheritdoc />
    public override uint ReadUInt32(DataIdentifier identifier, uint fallback = default) => ReadValue(identifier, fallback, XmlConvert.ToUInt32);
    /// <inheritdoc />
    public override long ReadInt64(DataIdentifier identifier, long fallback = default) => ReadValue(identifier, fallback, XmlConvert.ToInt64);
    /// <inheritdoc />
    public override ulong ReadUInt64(DataIdentifier identifier, ulong fallback = default) => ReadValue(identifier, fallback, XmlConvert.ToUInt64);
    /// <inheritdoc />
    public override float ReadSingle(DataIdentifier identifier, float fallback = default) => ReadValue(identifier, fallback, XmlConvert.ToSingle);
    /// <inheritdoc />
    public override double ReadDouble(DataIdentifier identifier, double fallback = default) => ReadValue(identifier, fallback, XmlConvert.ToDouble);
    /// <inheritdoc />
    public override decimal ReadDecimal(DataIdentifier identifier, decimal fallback = default) => ReadValue(identifier, fallback, XmlConvert.ToDecimal);
    /// <inheritdoc />
    public override char ReadChar(DataIdentifier identifier, char fallback = default) => ReadValue(identifier, fallback, static value => value.Length > 0 ? value[0] : throw new FormatException("The XML attribute is empty."));
    /// <inheritdoc />
    public override string? ReadString(DataIdentifier identifier, string? fallback = default) => _node.Attribute(GetPropertyName(identifier))?.Value ?? fallback;
    /// <inheritdoc />
    public override TimeSpan ReadTimeSpan(DataIdentifier identifier, TimeSpan fallback = default) => ReadValue(identifier, fallback, static value => new TimeSpan(XmlConvert.ToInt64(value)));
    /// <inheritdoc />
    public override DateTime ReadDateTime(DataIdentifier identifier, DateTime fallback = default) => ReadValue(identifier, fallback, static value => XmlConvert.ToDateTime(value, XmlDateTimeSerializationMode.RoundtripKind));
    /// <inheritdoc />
    public override Guid ReadGuid(DataIdentifier identifier, Guid fallback = default) => ReadValue(identifier, fallback, static value => Guid.Parse(value));
    /// <inheritdoc />
    public override TEnum ReadEnum<TEnum>(DataIdentifier identifier, TEnum fallback = default)
    {
        var value = _node.Attribute(GetPropertyName(identifier))?.Value;
        return value != null && TryFindEnumValue(value, out TEnum result) ? result : fallback;
    }

    /// <inheritdoc />
    public override IObjectReader<XmlReader>? ReadObject(DataIdentifier identifier, ObjectReaderGeneratorCallback<XmlReader> generator, IObjectReader<XmlReader>? fallback = default)
    {
        ArgumentNullException.ThrowIfNull(generator);
        var element = _node.Element(GetPropertyName(identifier));
        if (element == null) return fallback;
        var reader = new XmlReader(element) { ConversionErrorHandling = ConversionErrorHandling, Options = Options };
        var instance = generator(reader);
        if (instance == null) return fallback;
        instance.Read(reader);
        return instance;
    }

    /// <inheritdoc />
    public override XmlReader Read(DataIdentifier identifier)
    {
        var element = _node.Element(GetPropertyName(identifier));
        return element == null ? CreateReader() : new XmlReader(element) { ConversionErrorHandling = ConversionErrorHandling, Options = Options };
    }

    /// <inheritdoc />
    public override bool[] ReadBooleanArray(DataIdentifier identifier, bool[]? fallback = default) => ReadPrimitiveArray(identifier, fallback, XmlConvert.ToBoolean);
    /// <inheritdoc />
    public override byte[] ReadByteArray(DataIdentifier identifier, byte[]? fallback = default) => ReadPrimitiveArray(identifier, fallback, XmlConvert.ToByte);
    /// <inheritdoc />
    public override sbyte[] ReadSByteArray(DataIdentifier identifier, sbyte[]? fallback = default) => ReadPrimitiveArray(identifier, fallback, XmlConvert.ToSByte);
    /// <inheritdoc />
    public override short[] ReadInt16Array(DataIdentifier identifier, short[]? fallback = default) => ReadPrimitiveArray(identifier, fallback, XmlConvert.ToInt16);
    /// <inheritdoc />
    public override ushort[] ReadUInt16Array(DataIdentifier identifier, ushort[]? fallback = default) => ReadPrimitiveArray(identifier, fallback, XmlConvert.ToUInt16);
    /// <inheritdoc />
    public override int[] ReadInt32Array(DataIdentifier identifier, int[]? fallback = default) => ReadPrimitiveArray(identifier, fallback, XmlConvert.ToInt32);
    /// <inheritdoc />
    public override uint[] ReadUInt32Array(DataIdentifier identifier, uint[]? fallback = default) => ReadPrimitiveArray(identifier, fallback, XmlConvert.ToUInt32);
    /// <inheritdoc />
    public override long[] ReadInt64Array(DataIdentifier identifier, long[]? fallback = default) => ReadPrimitiveArray(identifier, fallback, XmlConvert.ToInt64);
    /// <inheritdoc />
    public override ulong[] ReadUInt64Array(DataIdentifier identifier, ulong[]? fallback = default) => ReadPrimitiveArray(identifier, fallback, XmlConvert.ToUInt64);
    /// <inheritdoc />
    public override float[] ReadSingleArray(DataIdentifier identifier, float[]? fallback = default) => ReadPrimitiveArray(identifier, fallback, XmlConvert.ToSingle);
    /// <inheritdoc />
    public override double[] ReadDoubleArray(DataIdentifier identifier, double[]? fallback = default) => ReadPrimitiveArray(identifier, fallback, XmlConvert.ToDouble);
    /// <inheritdoc />
    public override decimal[] ReadDecimalArray(DataIdentifier identifier, decimal[]? fallback = default) => ReadPrimitiveArray(identifier, fallback, XmlConvert.ToDecimal);
    /// <inheritdoc />
    public override char[] ReadCharArray(DataIdentifier identifier, char[]? fallback = default) => ReadPrimitiveArray(identifier, fallback, static value => value.Length > 0 ? value[0] : throw new FormatException("The XML attribute is empty."));
    /// <inheritdoc />
    public override string[] ReadStringArray(DataIdentifier identifier, string[]? fallback = default) => ReadPrimitiveArray(identifier, fallback, static value => value, string.Empty);
    /// <inheritdoc />
    public override TimeSpan[] ReadTimeSpanArray(DataIdentifier identifier, TimeSpan[]? fallback = default) => ReadPrimitiveArray(identifier, fallback, static value => new TimeSpan(XmlConvert.ToInt64(value)));
    /// <inheritdoc />
    public override DateTime[] ReadDateTimeArray(DataIdentifier identifier, DateTime[]? fallback = default) => ReadPrimitiveArray(identifier, fallback, static value => XmlConvert.ToDateTime(value, XmlDateTimeSerializationMode.RoundtripKind));
    /// <inheritdoc />
    public override Guid[] ReadGuidArray(DataIdentifier identifier, Guid[]? fallback = default) => ReadPrimitiveArray(identifier, fallback, static value => Guid.Parse(value));
    /// <inheritdoc />
    public override TEnum[] ReadEnumArray<TEnum>(DataIdentifier identifier, TEnum[]? fallback = default)
    {
        fallback ??= Array.Empty<TEnum>();
        var container = _node.Element(GetPropertyName(identifier));
        if (container == null) return fallback;
        var result = new List<TEnum>();
        foreach (var item in container.Elements("item"))
        {
            var value = item.Attribute("value")?.Value;
            if (value != null && TryFindEnumValue(value, out TEnum enumValue)) { result.Add(enumValue); continue; }
            switch (ConversionErrorHandling)
            {
                case ConversionErrorHandling.Ignore: continue;
                case ConversionErrorHandling.Cancel: return fallback;
                case ConversionErrorHandling.Replace:
                case ConversionErrorHandling.Default: result.Add(default); continue;
                default: return fallback;
            }
        }
        return result.ToArray();
    }

    /// <inheritdoc />
    public override IObjectReader<XmlReader>[] ReadObjectArray(DataIdentifier identifier, ObjectReaderGeneratorCallback<XmlReader> generator, IObjectReader<XmlReader>[]? fallback = default)
    {
        ArgumentNullException.ThrowIfNull(generator);
        fallback ??= Array.Empty<IObjectReader<XmlReader>>();
        var container = _node.Element(GetPropertyName(identifier));
        if (container == null) return fallback;
        var result = new List<IObjectReader<XmlReader>>();
        foreach (var element in container.Elements("item"))
        {
            var reader = new XmlReader(element) { ConversionErrorHandling = ConversionErrorHandling, Options = Options };
            var instance = generator(reader);
            if (instance == null)
            {
                if (ConversionErrorHandling == ConversionErrorHandling.Cancel) return fallback;
                continue;
            }
            instance.Read(reader);
            result.Add(instance);
        }
        return result.ToArray();
    }

    /// <inheritdoc />
    public override XmlReader[] ReadArray(DataIdentifier identifier)
    {
        var container = _node.Element(GetPropertyName(identifier));
        if (container == null) return Array.Empty<XmlReader>();
        var result = new List<XmlReader>();
        foreach (var element in container.Elements("item")) result.Add(new XmlReader(element) { ConversionErrorHandling = ConversionErrorHandling, Options = Options });
        return result.ToArray();
    }
}
