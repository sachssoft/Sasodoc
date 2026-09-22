using System;
using System.Globalization;
using System.Xml;
using System.Xml.Linq;

namespace Sachssoft.Documents.Formats.Xml;

/// <summary>
/// Writes document data to an XML element tree using attributes for primitive values.
/// </summary>
public class XmlWriter : FormatWriter<XmlWriter>
{
    private XElement _node;

    /// <summary>
    /// Initializes a new instance of the <see cref="XmlWriter"/> class.
    /// </summary>
    public XmlWriter() : this(new XElement("document")) { }

    internal XmlWriter(XElement node)
    {
        _node = node ?? throw new ArgumentNullException(nameof(node));
    }

    /// <summary>
    /// Gets or sets the XML element represented by this writer.
    /// </summary>
    internal XElement Node
    {
        get => _node;
        set => _node = value ?? throw new ArgumentNullException(nameof(value));
    }

    /// <inheritdoc />
    public override XmlWriter CreateWriter() => new(new XElement("object")) { Options = Options };

    private string GetPropertyName(DataIdentifier identifier) => XmlConvert.EncodeLocalName(ConvertPropertyIdentifier(identifier).Value);
    private string GetFieldName(string name) => XmlConvert.EncodeLocalName(ConvertFieldName(name));
    private void SetAttribute(DataIdentifier identifier, string? value)
    {
        var name = GetPropertyName(identifier);
        if (value == null) _node.Attribute(name)?.Remove();
        else _node.SetAttributeValue(name, value);
    }

    private void WritePrimitiveArray<T>(DataIdentifier identifier, T[]? values, Func<T, string> converter)
    {
        if (values == null) return;
        var container = new XElement(GetPropertyName(identifier));
        foreach (var value in values) container.Add(new XElement("item", new XAttribute("value", converter(value))));
        ReplaceElement(identifier, container);
    }

    private void ReplaceElement(DataIdentifier identifier, XElement element)
    {
        _node.Element(GetPropertyName(identifier))?.Remove();
        _node.Add(element);
    }

    /// <inheritdoc />
    public override void WriteBoolean(DataIdentifier identifier, bool value) => SetAttribute(identifier, XmlConvert.ToString(value));
    /// <inheritdoc />
    public override void WriteByte(DataIdentifier identifier, byte value) => SetAttribute(identifier, XmlConvert.ToString(value));
    /// <inheritdoc />
    public override void WriteSByte(DataIdentifier identifier, sbyte value) => SetAttribute(identifier, XmlConvert.ToString(value));
    /// <inheritdoc />
    public override void WriteInt16(DataIdentifier identifier, short value) => SetAttribute(identifier, XmlConvert.ToString(value));
    /// <inheritdoc />
    public override void WriteUInt16(DataIdentifier identifier, ushort value) => SetAttribute(identifier, XmlConvert.ToString(value));
    /// <inheritdoc />
    public override void WriteInt32(DataIdentifier identifier, int value) => SetAttribute(identifier, XmlConvert.ToString(value));
    /// <inheritdoc />
    public override void WriteUInt32(DataIdentifier identifier, uint value) => SetAttribute(identifier, XmlConvert.ToString(value));
    /// <inheritdoc />
    public override void WriteInt64(DataIdentifier identifier, long value) => SetAttribute(identifier, XmlConvert.ToString(value));
    /// <inheritdoc />
    public override void WriteUInt64(DataIdentifier identifier, ulong value) => SetAttribute(identifier, XmlConvert.ToString(value));
    /// <inheritdoc />
    public override void WriteSingle(DataIdentifier identifier, float value) => SetAttribute(identifier, XmlConvert.ToString(value));
    /// <inheritdoc />
    public override void WriteDouble(DataIdentifier identifier, double value) => SetAttribute(identifier, XmlConvert.ToString(value));
    /// <inheritdoc />
    public override void WriteDecimal(DataIdentifier identifier, decimal value) => SetAttribute(identifier, XmlConvert.ToString(value));
    /// <inheritdoc />
    public override void WriteChar(DataIdentifier identifier, char value) => SetAttribute(identifier, value.ToString());
    /// <inheritdoc />
    public override void WriteString(DataIdentifier identifier, string? value) => SetAttribute(identifier, value);
    /// <inheritdoc />
    public override void WriteTimeSpan(DataIdentifier identifier, TimeSpan value) => SetAttribute(identifier, XmlConvert.ToString(value.Ticks));
    /// <inheritdoc />
    public override void WriteDateTime(DataIdentifier identifier, DateTime value) => SetAttribute(identifier, XmlConvert.ToString(value, XmlDateTimeSerializationMode.RoundtripKind));
    /// <inheritdoc />
    public override void WriteGuid(DataIdentifier identifier, Guid value) => SetAttribute(identifier, value.ToString("D", CultureInfo.InvariantCulture));
    /// <inheritdoc />
    public override void WriteEnum<TEnum>(DataIdentifier identifier, TEnum value) => SetAttribute(identifier, GetFieldName(value.ToString()));

    /// <inheritdoc />
    public override void WriteObject(DataIdentifier identifier, IObjectWriter<XmlWriter>? value, Action<XmlWriter>? writerBefore = null)
    {
        if (value == null) return;
        var element = new XElement(GetPropertyName(identifier));
        var writer = new XmlWriter(element) { Options = Options };
        writerBefore?.Invoke(writer);
        value.Write(writer);
        ReplaceElement(identifier, element);
    }

    /// <inheritdoc />
    public override void Write(DataIdentifier identifier, XmlWriter? value)
    {
        if (value == null) return;
        ReplaceElement(identifier, new XElement(GetPropertyName(identifier), value._node.Attributes(), value._node.Nodes()));
    }

    /// <inheritdoc />
    public override void WriteBooleanArray(DataIdentifier identifier, bool[]? values) => WritePrimitiveArray(identifier, values, XmlConvert.ToString);
    /// <inheritdoc />
    public override void WriteByteArray(DataIdentifier identifier, byte[]? values) => WritePrimitiveArray(identifier, values, XmlConvert.ToString);
    /// <inheritdoc />
    public override void WriteSByteArray(DataIdentifier identifier, sbyte[]? values) => WritePrimitiveArray(identifier, values, XmlConvert.ToString);
    /// <inheritdoc />
    public override void WriteInt16Array(DataIdentifier identifier, short[]? values) => WritePrimitiveArray(identifier, values, XmlConvert.ToString);
    /// <inheritdoc />
    public override void WriteUInt16Array(DataIdentifier identifier, ushort[]? values) => WritePrimitiveArray(identifier, values, XmlConvert.ToString);
    /// <inheritdoc />
    public override void WriteInt32Array(DataIdentifier identifier, int[]? values) => WritePrimitiveArray(identifier, values, XmlConvert.ToString);
    /// <inheritdoc />
    public override void WriteUInt32Array(DataIdentifier identifier, uint[]? values) => WritePrimitiveArray(identifier, values, XmlConvert.ToString);
    /// <inheritdoc />
    public override void WriteInt64Array(DataIdentifier identifier, long[]? values) => WritePrimitiveArray(identifier, values, XmlConvert.ToString);
    /// <inheritdoc />
    public override void WriteUInt64Array(DataIdentifier identifier, ulong[]? values) => WritePrimitiveArray(identifier, values, XmlConvert.ToString);
    /// <inheritdoc />
    public override void WriteSingleArray(DataIdentifier identifier, float[]? values) => WritePrimitiveArray(identifier, values, XmlConvert.ToString);
    /// <inheritdoc />
    public override void WriteDoubleArray(DataIdentifier identifier, double[]? values) => WritePrimitiveArray(identifier, values, XmlConvert.ToString);
    /// <inheritdoc />
    public override void WriteDecimalArray(DataIdentifier identifier, decimal[]? values) => WritePrimitiveArray(identifier, values, XmlConvert.ToString);
    /// <inheritdoc />
    public override void WriteCharArray(DataIdentifier identifier, char[]? values) => WritePrimitiveArray(identifier, values, static value => value.ToString());
    /// <inheritdoc />
    public override void WriteStringArray(DataIdentifier identifier, string[]? values) => WritePrimitiveArray(identifier, values, static value => value);
    /// <inheritdoc />
    public override void WriteTimeSpanArray(DataIdentifier identifier, TimeSpan[]? values) => WritePrimitiveArray(identifier, values, static value => XmlConvert.ToString(value.Ticks));
    /// <inheritdoc />
    public override void WriteDateTimeArray(DataIdentifier identifier, DateTime[]? values) => WritePrimitiveArray(identifier, values, static value => XmlConvert.ToString(value, XmlDateTimeSerializationMode.RoundtripKind));
    /// <inheritdoc />
    public override void WriteGuidArray(DataIdentifier identifier, Guid[]? values) => WritePrimitiveArray(identifier, values, static value => value.ToString("D", CultureInfo.InvariantCulture));
    /// <inheritdoc />
    public override void WriteEnumArray<TEnum>(DataIdentifier identifier, TEnum[]? values)
    {
        if (values == null) return;
        WritePrimitiveArray(identifier, values, value => GetFieldName(value.ToString()));
    }

    /// <inheritdoc />
    public override void WriteObjectArray(DataIdentifier identifier, IObjectWriter<XmlWriter>[]? values, Action<XmlWriter>? writerBefore = null)
    {
        if (values == null) return;
        var container = new XElement(GetPropertyName(identifier));
        foreach (var value in values)
        {
            var element = new XElement("item");
            var writer = new XmlWriter(element) { Options = Options };
            writerBefore?.Invoke(writer);
            value.Write(writer);
            container.Add(element);
        }
        ReplaceElement(identifier, container);
    }

    /// <inheritdoc />
    public override void WriteArray(DataIdentifier identifier, XmlWriter[]? values)
    {
        if (values == null) return;
        var container = new XElement(GetPropertyName(identifier));
        foreach (var value in values) container.Add(new XElement("item", value._node.Attributes(), value._node.Nodes()));
        ReplaceElement(identifier, container);
    }
}
