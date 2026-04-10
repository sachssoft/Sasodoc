using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Nodes;

namespace Sachssoft.Sasodoc.Formats.Json;

public class JsonWriter : FormatWriter<JsonWriter, string>
{
    private JsonObject _node;

    public JsonWriter()
    {
        _node = new JsonObject();
    }

    internal JsonObject Node
    {
        get => _node;
        set => _node = value;
    }

    public override FormatWriterBase CreateWriter()
    {
        return new JsonWriter();
    }

    private void WritePrimitiveArray<T>(string? property, T[]? values, Func<T, JsonNode> instance)
    {
        if (property == null) throw new ArgumentNullException(nameof(property));

        values ??= Array.Empty<T>();

        var list = new List<JsonNode>(values.Length);
        foreach (var v in values)
        {
            list.Add(instance(v));
        }

        var propertyName = ConvertPropertyName(property);
        _node[propertyName] = new JsonArray(list.ToArray());
    }

    public override void Write(string? property, FormatWriter<JsonWriter, string>? value)
    {
        if (value is JsonWriter jw)
        {
            if (property == null)
                throw new ArgumentNullException(nameof(property), "PropertyDescriptor name cannot be null for complex objects.");

            var propertyName = ConvertPropertyName(property);
            _node[propertyName] = jw._node;
        }
        else
        {
            throw new ArgumentException("Value must be a JsonWriter.", nameof(value));
        }
    }

    public override void WriteArray(string? property, FormatWriter<JsonWriter, string>[]? values)
    {
        WritePrimitiveArray<JsonWriter>(property, values?.Select(x => (JsonWriter)x).ToArray(), (v) => v._node);
    }

    public override void WriteBoolean(string? property, bool value)
    {
        if (property != null)
        {
            var propertyName = ConvertPropertyName(property);
            _node[propertyName] = value;
        }
    }

    public override void WriteBooleanArray(string? property, bool[]? values)
    {
        WritePrimitiveArray<bool>(property, values, (v) => v);
    }

    public override void WriteByte(string? property, byte value)
    {
        if (property != null)
        {
            var propertyName = ConvertPropertyName(property);
            _node[propertyName] = value;
        }
    }

    public override void WriteByteArray(string? property, byte[]? values)
    {
        WritePrimitiveArray<byte>(property, values, (v) => v);
    }

    public override void WriteChar(string? property, char value)
    {
        if (property != null)
        {
            var propertyName = ConvertPropertyName(property);
            _node[propertyName] = value.ToString();
        }
    }

    public override void WriteCharArray(string? property, char[]? values)
    {
        WritePrimitiveArray<char>(property, values, (v) => v);
    }

    public override void WriteTimeSpan(string? property, TimeSpan value)
    {
        if (property != null)
        {
            var propertyName = ConvertPropertyName(property);
            _node[propertyName] = value.Ticks;
        }
    }

    public override void WriteDateTime(string? property, DateTime value)
    {
        if (property != null)
        {
            var propertyName = ConvertPropertyName(property);
            _node[propertyName] = value;
        }
    }

    public override void WriteTimeSpanArray(string? property, TimeSpan[]? values)
    {
        WritePrimitiveArray<TimeSpan>(property, values, (v) => v.Ticks);
    }

    public override void WriteDateTimeArray(string? property, DateTime[]? values)
    {
        WritePrimitiveArray<DateTime>(property, values, (v) => v);
    }

    public override void WriteDecimal(string? property, decimal value)
    {
        if (property != null)
        {
            var propertyName = ConvertPropertyName(property);
            _node[propertyName] = value;
        }
    }

    public override void WriteDecimalArray(string? property, decimal[]? values)
    {
        WritePrimitiveArray<decimal>(property, values, (v) => v);
    }

    public override void WriteDouble(string? property, double value)
    {
        if (property != null)
        {
            var propertyName = ConvertPropertyName(property);
            _node[propertyName] = value;
        }
    }

    public override void WriteDoubleArray(string? property, double[]? values)
    {
        WritePrimitiveArray<double>(property, values, (v) => v);
    }

    public override void WriteGuid(string? property, Guid value)
    {
        if (property != null)
        {
            var propertyName = ConvertPropertyName(property);
            _node[propertyName] = value.ToString();
        }
    }

    public override void WriteGuidArray(string? property, Guid[]? values)
    {
        WritePrimitiveArray<Guid>(property, values, (v) => v);
    }

    public override void WriteInt16(string? property, short value)
    {
        if (property != null)
        {
            var propertyName = ConvertPropertyName(property);
            _node[propertyName] = value;
        }
    }

    public override void WriteInt16Array(string? property, short[]? values)
    {
        WritePrimitiveArray<short>(property, values, (v) => v);
    }

    public override void WriteInt32(string? property, int value)
    {
        if (property != null)
        {
            var propertyName = ConvertPropertyName(property);
            _node[propertyName] = value;
        }
    }

    public override void WriteInt32Array(string? property, int[]? values)
    {
        WritePrimitiveArray<int>(property, values, (v) => v);
    }

    public override void WriteInt64(string? property, long value)
    {
        if (property != null)
        {
            var propertyName = ConvertPropertyName(property);
            _node[propertyName] = value;
        }
    }

    public override void WriteInt64Array(string? property, long[]? values)
    {
        WritePrimitiveArray<long>(property, values, (v) => v);
    }

    public override void WriteSByte(string? property, sbyte value)
    {
        if (property != null)
        {
            var propertyName = ConvertPropertyName(property);
            _node[propertyName] = value;
        }
    }

    public override void WriteSByteArray(string? property, sbyte[]? values)
    {
        WritePrimitiveArray<sbyte>(property, values, (v) => v);
    }

    public override void WriteSingle(string? property, float value)
    {
        if (property != null)
        {
            var propertyName = ConvertPropertyName(property);
            _node[propertyName] = value;
        }
    }

    public override void WriteSingleArray(string? property, float[]? values)
    {
        WritePrimitiveArray<float>(property, values, (v) => v);
    }

    public override void WriteString(string? property, string? value)
    {
        if (property != null)
        {
            var propertyName = ConvertPropertyName(property);
            _node[propertyName] = value;
        }
    }

    public override void WriteStringArray(string? property, string[]? values)
    {
        WritePrimitiveArray<string>(property, values, (v) => v);
    }

    public override void WriteUInt16(string? property, ushort value)
    {
        if (property != null)
        {
            var propertyName = ConvertPropertyName(property);
            _node[propertyName] = value;
        }
    }

    public override void WriteUInt16Array(string? property, ushort[]? values)
    {
        WritePrimitiveArray<ushort>(property, values, (v) => v);
    }

    public override void WriteUInt32(string? property, uint value)
    {
        if (property != null)
        {
            var propertyName = ConvertPropertyName(property);
            _node[propertyName] = value;
        }
    }

    public override void WriteUInt32Array(string? property, uint[]? values)
    {
        WritePrimitiveArray<uint>(property, values, (v) => v);
    }

    public override void WriteUInt64(string? property, ulong value)
    {
        if (property != null)
        {
            var propertyName = ConvertPropertyName(property);
            _node[propertyName] = value;
        }
    }

    public override void WriteUInt64Array(string? property, ulong[]? values)
    {
        WritePrimitiveArray<ulong>(property, values, (v) => v);
    }

    public override void WriteEnum<TEnum>(string? property, TEnum value)
    {
        if (property != null)
        {
            var propertyName = ConvertPropertyName(property);
            _node[propertyName] = ConvertFieldName(value.ToString());
        }
    }

    public override void WriteEnumArray<TEnum>(string? property, TEnum[]? values)
    {
        if (property != null && values != null)
        {
            var list = new List<JsonValue>();
            for (int i = 0; i < values.Length; i++)
            {
                list.Add(JsonValue.Create(ConvertFieldName(values[i].ToString())));
            }
            var propertyName = ConvertPropertyName(property);
            _node[propertyName] = new JsonArray(list.ToArray());
        }
    }

    public override void WriteObject(string? property, IObjectWriter<JsonWriter>? value, Action<JsonWriter>? writerBefore = null)
    {
        if (property != null && value != null)
        {
            var writer = new JsonWriter();
            writerBefore?.Invoke(writer);
            value.Write(writer);

            var propertyName = ConvertPropertyName(property);
            _node[propertyName] = writer._node;
        }
    }

    public override void WriteObjectArray(string? property, IObjectWriter<JsonWriter>[]? values)
    {
        if (property != null && values != null)
        {
            var list = new List<JsonObject>();
            for (int i = 0; i < values.Length; i++)
            {
                var writer = new JsonWriter();
                values[i].Write(writer);
                list.Add(writer._node);
            }

            var propertyName = ConvertPropertyName(property);
            _node[propertyName] = new JsonArray(list.ToArray());
        }
    }
}
