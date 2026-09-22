using System;
using System.Collections.Generic;
using System.Text.Json.Nodes;

namespace Sachssoft.Documents.Formats.Json;

/// <summary>
/// Writes document data to a JSON object tree.
/// </summary>
public class JsonWriter : FormatWriter<JsonWriter>
{
    private JsonObject _node;

    /// <summary>
    /// Initializes a new instance of the <see cref="JsonWriter"/> class.
    /// </summary>
    public JsonWriter()
    {
        _node = new JsonObject();
    }

    /// <summary>
    /// Gets or sets the JSON object represented by this writer.
    /// </summary>
    internal JsonObject Node
    {
        get => _node;
        set => _node = value ?? throw new ArgumentNullException(nameof(value));
    }

    /// <inheritdoc />
    public override JsonWriter CreateWriter() => new() { Options = Options };

    private string GetPropertyName(DataIdentifier identifier) => ConvertPropertyIdentifier(identifier).Value;

    private void WritePrimitiveArray<T>(DataIdentifier identifier, T[]? values, Func<T, JsonNode?> factory)
    {
        values ??= Array.Empty<T>();
        var array = new JsonArray();

        foreach (var value in values)
            array.Add(factory(value));

        _node[GetPropertyName(identifier)] = array;
    }

    /// <inheritdoc />
    public override void Write(DataIdentifier identifier, JsonWriter? value)
    {
        if (value == null) throw new ArgumentException("Value must be a JsonWriter.", nameof(value));
        _node[GetPropertyName(identifier)] = value._node.DeepClone();
    }

    /// <inheritdoc />
    public override void WriteArray(DataIdentifier identifier, JsonWriter[]? values)
    {
        var array = new JsonArray();

        if (values != null)
        {
            foreach (var value in values)
                array.Add(value?._node.DeepClone());
        }

        _node[GetPropertyName(identifier)] = array;
    }

    /// <inheritdoc />
    public override void WriteBoolean(DataIdentifier identifier, bool value) => _node[GetPropertyName(identifier)] = value;

    /// <inheritdoc />
    public override void WriteBooleanArray(DataIdentifier identifier, bool[]? values) => WritePrimitiveArray(identifier, values, value => JsonValue.Create(value));

    /// <inheritdoc />
    public override void WriteByte(DataIdentifier identifier, byte value) => _node[GetPropertyName(identifier)] = value;

    /// <inheritdoc />
    public override void WriteByteArray(DataIdentifier identifier, byte[]? values) => WritePrimitiveArray(identifier, values, value => JsonValue.Create(value));

    /// <inheritdoc />
    public override void WriteSByte(DataIdentifier identifier, sbyte value) => _node[GetPropertyName(identifier)] = value;

    /// <inheritdoc />
    public override void WriteSByteArray(DataIdentifier identifier, sbyte[]? values) => WritePrimitiveArray(identifier, values, value => JsonValue.Create(value));

    /// <inheritdoc />
    public override void WriteInt16(DataIdentifier identifier, short value) => _node[GetPropertyName(identifier)] = value;

    /// <inheritdoc />
    public override void WriteInt16Array(DataIdentifier identifier, short[]? values) => WritePrimitiveArray(identifier, values, value => JsonValue.Create(value));

    /// <inheritdoc />
    public override void WriteUInt16(DataIdentifier identifier, ushort value) => _node[GetPropertyName(identifier)] = value;

    /// <inheritdoc />
    public override void WriteUInt16Array(DataIdentifier identifier, ushort[]? values) => WritePrimitiveArray(identifier, values, value => JsonValue.Create(value));

    /// <inheritdoc />
    public override void WriteInt32(DataIdentifier identifier, int value) => _node[GetPropertyName(identifier)] = value;

    /// <inheritdoc />
    public override void WriteInt32Array(DataIdentifier identifier, int[]? values) => WritePrimitiveArray(identifier, values, value => JsonValue.Create(value));

    /// <inheritdoc />
    public override void WriteUInt32(DataIdentifier identifier, uint value) => _node[GetPropertyName(identifier)] = value;

    /// <inheritdoc />
    public override void WriteUInt32Array(DataIdentifier identifier, uint[]? values) => WritePrimitiveArray(identifier, values, value => JsonValue.Create(value));

    /// <inheritdoc />
    public override void WriteInt64(DataIdentifier identifier, long value) => _node[GetPropertyName(identifier)] = value;

    /// <inheritdoc />
    public override void WriteInt64Array(DataIdentifier identifier, long[]? values) => WritePrimitiveArray(identifier, values, value => JsonValue.Create(value));

    /// <inheritdoc />
    public override void WriteUInt64(DataIdentifier identifier, ulong value) => _node[GetPropertyName(identifier)] = value;

    /// <inheritdoc />
    public override void WriteUInt64Array(DataIdentifier identifier, ulong[]? values) => WritePrimitiveArray(identifier, values, value => JsonValue.Create(value));

    /// <inheritdoc />
    public override void WriteSingle(DataIdentifier identifier, float value) => _node[GetPropertyName(identifier)] = value;

    /// <inheritdoc />
    public override void WriteSingleArray(DataIdentifier identifier, float[]? values) => WritePrimitiveArray(identifier, values, value => JsonValue.Create(value));

    /// <inheritdoc />
    public override void WriteDouble(DataIdentifier identifier, double value) => _node[GetPropertyName(identifier)] = value;

    /// <inheritdoc />
    public override void WriteDoubleArray(DataIdentifier identifier, double[]? values) => WritePrimitiveArray(identifier, values, value => JsonValue.Create(value));

    /// <inheritdoc />
    public override void WriteDecimal(DataIdentifier identifier, decimal value) => _node[GetPropertyName(identifier)] = value;

    /// <inheritdoc />
    public override void WriteDecimalArray(DataIdentifier identifier, decimal[]? values) => WritePrimitiveArray(identifier, values, value => JsonValue.Create(value));

    /// <inheritdoc />
    public override void WriteChar(DataIdentifier identifier, char value) => _node[GetPropertyName(identifier)] = value.ToString();

    /// <inheritdoc />
    public override void WriteCharArray(DataIdentifier identifier, char[]? values) => WritePrimitiveArray(identifier, values, value => JsonValue.Create(value.ToString()));

    /// <inheritdoc />
    public override void WriteString(DataIdentifier identifier, string? value) => _node[GetPropertyName(identifier)] = value;

    /// <inheritdoc />
    public override void WriteStringArray(DataIdentifier identifier, string[]? values) => WritePrimitiveArray(identifier, values, value => JsonValue.Create(value));

    /// <inheritdoc />
    public override void WriteTimeSpan(DataIdentifier identifier, TimeSpan value) => _node[GetPropertyName(identifier)] = value.Ticks;

    /// <inheritdoc />
    public override void WriteTimeSpanArray(DataIdentifier identifier, TimeSpan[]? values) => WritePrimitiveArray(identifier, values, value => JsonValue.Create(value.Ticks));

    /// <inheritdoc />
    public override void WriteDateTime(DataIdentifier identifier, DateTime value) => _node[GetPropertyName(identifier)] = value;

    /// <inheritdoc />
    public override void WriteDateTimeArray(DataIdentifier identifier, DateTime[]? values) => WritePrimitiveArray(identifier, values, value => JsonValue.Create(value));

    /// <inheritdoc />
    public override void WriteGuid(DataIdentifier identifier, Guid value) => _node[GetPropertyName(identifier)] = value;

    /// <inheritdoc />
    public override void WriteGuidArray(DataIdentifier identifier, Guid[]? values) => WritePrimitiveArray(identifier, values, value => JsonValue.Create(value));

    /// <inheritdoc />
    public override void WriteEnum<TEnum>(DataIdentifier identifier, TEnum value) => _node[GetPropertyName(identifier)] = ConvertFieldName(value.ToString());

    /// <inheritdoc />
    public override void WriteEnumArray<TEnum>(DataIdentifier identifier, TEnum[]? values)
    {
        if (values == null) return;

        var array = new JsonArray();

        foreach (var value in values)
        {
            var name = ConvertFieldName(value.ToString());
            JsonNode? node = JsonValue.Create(name);
            array.Add(node);
        }

        _node[GetPropertyName(identifier)] = array;
    }

    /// <inheritdoc />
    public override void WriteObject(DataIdentifier identifier, IObjectWriter<JsonWriter>? value, Action<JsonWriter>? writerBefore = null)
    {
        if (value == null) return;

        var writer = CreateWriter();
        writerBefore?.Invoke(writer);
        value.Write(writer);
        _node[GetPropertyName(identifier)] = writer._node;
    }

    /// <inheritdoc />
    public override void WriteObjectArray(DataIdentifier identifier, IObjectWriter<JsonWriter>[]? values, Action<JsonWriter>? writerBefore = null)
    {
        if (values == null) return;

        var array = new JsonArray();

        foreach (var value in values)
        {
            var writer = CreateWriter();
            writerBefore?.Invoke(writer);
            value.Write(writer);

            JsonNode node = writer._node;
            array.Add(node);
        }

        _node[GetPropertyName(identifier)] = array;
    }
}
