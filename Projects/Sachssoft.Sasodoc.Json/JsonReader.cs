using System;
using System.Collections.Generic;
using System.Text.Json.Nodes;

namespace Sachssoft.Documents.Formats.Json;

/// <summary>
/// Reads document data from a JSON object tree.
/// </summary>
public class JsonReader : FormatReader<JsonReader>
{
    private JsonObject _node;

    /// <summary>
    /// Initializes a new instance of the <see cref="JsonReader"/> class.
    /// </summary>
    public JsonReader()
    {
        _node = new JsonObject();
    }

    /// <summary>
    /// Gets or sets the JSON object represented by this reader.
    /// </summary>
    internal JsonObject Node
    {
        get => _node;
        set => _node = value ?? throw new ArgumentNullException(nameof(value));
    }

    /// <summary>
    /// Gets or sets how failed primitive value conversions are handled.
    /// </summary>
    public ConversionErrorHandling ConversionErrorHandling { get; set; }

    /// <inheritdoc />
    public override JsonReader CreateReader() => new() { ConversionErrorHandling = ConversionErrorHandling, Options = Options };

    private string GetPropertyName(DataIdentifier identifier) => ConvertPropertyIdentifier(identifier).Value;

    /// <inheritdoc />
    public override bool Contains(DataIdentifier identifier) => _node.ContainsKey(GetPropertyName(identifier));

    private bool TryGetValue<T>(DataIdentifier identifier, Func<JsonValue, T> converter, out T value)
    {
        if (_node.TryGetPropertyValue(GetPropertyName(identifier), out var node) && node is JsonValue jsonValue)
        {
            try
            {
                value = converter(jsonValue);
                return true;
            }
            catch (Exception exception) when (exception is InvalidOperationException or FormatException or OverflowException)
            {
            }
        }

        value = default!;
        return false;
    }

    private T ReadValue<T>(DataIdentifier identifier, T fallback, Func<JsonValue, T> converter)
    {
        if (TryGetValue(identifier, converter, out T value))
            return value;

        return ConversionErrorHandling switch
        {
            ConversionErrorHandling.Default => default!,
            ConversionErrorHandling.Replace => fallback,
            _ => fallback
        };
    }

    private T[] ReadPrimitiveArray<T>(DataIdentifier identifier, T[]? fallback, Func<JsonValue, T> converter, ConversionErrorHandling errorHandling = ConversionErrorHandling.Cancel, T replacementValue = default!)
    {
        fallback ??= Array.Empty<T>();

        if (!_node.TryGetPropertyValue(GetPropertyName(identifier), out var node) || node is not JsonArray array)
            return fallback;

        var result = new List<T>(array.Count);

        foreach (var item in array)
        {
            if (item is not JsonValue jsonValue)
            {
                if (!HandleInvalidArrayValue(result, fallback, errorHandling, replacementValue, out var cancelled))
                    continue;

                if (cancelled)
                    return fallback;

                continue;
            }

            try
            {
                result.Add(converter(jsonValue));
            }
            catch (Exception exception) when (exception is InvalidOperationException or FormatException or OverflowException)
            {
                if (!HandleInvalidArrayValue(result, fallback, errorHandling, replacementValue, out var cancelled))
                    continue;

                if (cancelled)
                    return fallback;
            }
        }

        return result.ToArray();
    }

    private static bool HandleInvalidArrayValue<T>(List<T> result, T[] fallback, ConversionErrorHandling errorHandling, T replacementValue, out bool cancelled)
    {
        cancelled = false;

        switch (errorHandling)
        {
            case ConversionErrorHandling.Ignore:
                return false;

            case ConversionErrorHandling.Cancel:
                cancelled = true;
                return true;

            case ConversionErrorHandling.Replace:
                result.Add(replacementValue);
                return true;

            case ConversionErrorHandling.Default:
                result.Add(default!);
                return true;

            default:
                cancelled = true;
                return true;
        }
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
    public override bool ReadBoolean(DataIdentifier identifier, bool fallback = default) => ReadValue(identifier, fallback, value => value.GetValue<bool>());

    /// <inheritdoc />
    public override byte ReadByte(DataIdentifier identifier, byte fallback = default) => ReadValue(identifier, fallback, value => value.GetValue<byte>());

    /// <inheritdoc />
    public override sbyte ReadSByte(DataIdentifier identifier, sbyte fallback = default) => ReadValue(identifier, fallback, value => value.GetValue<sbyte>());

    /// <inheritdoc />
    public override short ReadInt16(DataIdentifier identifier, short fallback = default) => ReadValue(identifier, fallback, value => value.GetValue<short>());

    /// <inheritdoc />
    public override ushort ReadUInt16(DataIdentifier identifier, ushort fallback = default) => ReadValue(identifier, fallback, value => value.GetValue<ushort>());

    /// <inheritdoc />
    public override int ReadInt32(DataIdentifier identifier, int fallback = default) => ReadValue(identifier, fallback, value => value.GetValue<int>());

    /// <inheritdoc />
    public override uint ReadUInt32(DataIdentifier identifier, uint fallback = default) => ReadValue(identifier, fallback, value => value.GetValue<uint>());

    /// <inheritdoc />
    public override long ReadInt64(DataIdentifier identifier, long fallback = default) => ReadValue(identifier, fallback, value => value.GetValue<long>());

    /// <inheritdoc />
    public override ulong ReadUInt64(DataIdentifier identifier, ulong fallback = default) => ReadValue(identifier, fallback, value => value.GetValue<ulong>());

    /// <inheritdoc />
    public override float ReadSingle(DataIdentifier identifier, float fallback = default) => ReadValue(identifier, fallback, value => value.GetValue<float>());

    /// <inheritdoc />
    public override double ReadDouble(DataIdentifier identifier, double fallback = default) => ReadValue(identifier, fallback, value => value.GetValue<double>());

    /// <inheritdoc />
    public override decimal ReadDecimal(DataIdentifier identifier, decimal fallback = default) => ReadValue(identifier, fallback, value => value.GetValue<decimal>());

    /// <inheritdoc />
    public override char ReadChar(DataIdentifier identifier, char fallback = default) => ReadValue(identifier, fallback, value =>
    {
        var text = value.GetValue<string>();
        return text.Length > 0 ? text[0] : fallback;
    });

    /// <inheritdoc />
    public override string? ReadString(DataIdentifier identifier, string? fallback = default) => ReadValue(identifier, fallback, value => value.GetValue<string>());

    /// <inheritdoc />
    public override TimeSpan ReadTimeSpan(DataIdentifier identifier, TimeSpan fallback = default) => ReadValue(identifier, fallback, value => new TimeSpan(value.GetValue<long>()));

    /// <inheritdoc />
    public override DateTime ReadDateTime(DataIdentifier identifier, DateTime fallback = default) => ReadValue(identifier, fallback, value => value.GetValue<DateTime>());

    /// <inheritdoc />
    public override Guid ReadGuid(DataIdentifier identifier, Guid fallback = default) => ReadValue(identifier, fallback, value => value.GetValue<Guid>());

    /// <inheritdoc />
    public override TEnum ReadEnum<TEnum>(DataIdentifier identifier, TEnum fallback = default)
    {
        if (!_node.TryGetPropertyValue(GetPropertyName(identifier), out var node) || node is not JsonValue value)
            return fallback;

        try
        {
            return TryFindEnumValue(value.GetValue<string>(), out TEnum result) ? result : fallback;
        }
        catch (Exception exception) when (exception is InvalidOperationException or FormatException)
        {
            return fallback;
        }
    }

    /// <inheritdoc />
    public override IObjectReader<JsonReader>? ReadObject(DataIdentifier identifier, ObjectReaderGeneratorCallback<JsonReader> generator, IObjectReader<JsonReader>? fallback = default)
    {
        ArgumentNullException.ThrowIfNull(generator);

        if (!_node.TryGetPropertyValue(GetPropertyName(identifier), out var node) || node is not JsonObject jsonObject)
            return fallback;

        var reader = CreateReader();
        reader._node = jsonObject;

        var instance = generator(reader);
        if (instance == null)
            return fallback;

        instance.Read(reader);
        return instance;
    }

    /// <inheritdoc />
    public override JsonReader Read(DataIdentifier identifier)
    {
        var reader = CreateReader();

        if (_node.TryGetPropertyValue(GetPropertyName(identifier), out var node) && node is JsonObject jsonObject)
            reader._node = jsonObject;

        return reader;
    }

    /// <inheritdoc />
    public override bool[] ReadBooleanArray(DataIdentifier identifier, bool[]? fallback = default) => ReadPrimitiveArray(identifier, fallback, value => value.GetValue<bool>());

    /// <inheritdoc />
    public override byte[] ReadByteArray(DataIdentifier identifier, byte[]? fallback = default) => ReadPrimitiveArray(identifier, fallback, value => value.GetValue<byte>());

    /// <inheritdoc />
    public override sbyte[] ReadSByteArray(DataIdentifier identifier, sbyte[]? fallback = default) => ReadPrimitiveArray(identifier, fallback, value => value.GetValue<sbyte>());

    /// <inheritdoc />
    public override short[] ReadInt16Array(DataIdentifier identifier, short[]? fallback = default) => ReadPrimitiveArray(identifier, fallback, value => value.GetValue<short>());

    /// <inheritdoc />
    public override ushort[] ReadUInt16Array(DataIdentifier identifier, ushort[]? fallback = default) => ReadPrimitiveArray(identifier, fallback, value => value.GetValue<ushort>());

    /// <inheritdoc />
    public override int[] ReadInt32Array(DataIdentifier identifier, int[]? fallback = default) => ReadPrimitiveArray(identifier, fallback, value => value.GetValue<int>());

    /// <inheritdoc />
    public override uint[] ReadUInt32Array(DataIdentifier identifier, uint[]? fallback = default) => ReadPrimitiveArray(identifier, fallback, value => value.GetValue<uint>());

    /// <inheritdoc />
    public override long[] ReadInt64Array(DataIdentifier identifier, long[]? fallback = default) => ReadPrimitiveArray(identifier, fallback, value => value.GetValue<long>());

    /// <inheritdoc />
    public override ulong[] ReadUInt64Array(DataIdentifier identifier, ulong[]? fallback = default) => ReadPrimitiveArray(identifier, fallback, value => value.GetValue<ulong>());

    /// <inheritdoc />
    public override float[] ReadSingleArray(DataIdentifier identifier, float[]? fallback = default) => ReadPrimitiveArray(identifier, fallback, value => value.GetValue<float>());

    /// <inheritdoc />
    public override double[] ReadDoubleArray(DataIdentifier identifier, double[]? fallback = default) => ReadPrimitiveArray(identifier, fallback, value => value.GetValue<double>());

    /// <inheritdoc />
    public override decimal[] ReadDecimalArray(DataIdentifier identifier, decimal[]? fallback = default) => ReadPrimitiveArray(identifier, fallback, value => value.GetValue<decimal>());

    /// <inheritdoc />
    public override char[] ReadCharArray(DataIdentifier identifier, char[]? fallback = default) => ReadPrimitiveArray(identifier, fallback, value =>
    {
        var text = value.GetValue<string>();
        if (text.Length == 0) throw new FormatException("The JSON value cannot be converted to a character.");
        return text[0];
    });

    /// <inheritdoc />
    public override string[] ReadStringArray(DataIdentifier identifier, string[]? fallback = default)
    {
        fallback ??= Array.Empty<string>();

        if (!_node.TryGetPropertyValue(GetPropertyName(identifier), out var node) || node is not JsonArray array)
            return fallback;

        var result = new List<string>(array.Count);

        foreach (var item in array)
        {
            if (item is not JsonValue value)
            {
                if (ConversionErrorHandling == ConversionErrorHandling.Cancel)
                    return fallback;

                if (ConversionErrorHandling == ConversionErrorHandling.Replace)
                    result.Add(string.Empty);

                continue;
            }

            try
            {
                var text = value.GetValue<string>();
                result.Add(text);
            }
            catch (Exception exception) when (exception is InvalidOperationException or FormatException)
            {
                switch (ConversionErrorHandling)
                {
                    case ConversionErrorHandling.Cancel:
                        return fallback;

                    case ConversionErrorHandling.Replace:
                        result.Add(string.Empty);
                        break;

                    case ConversionErrorHandling.Ignore:
                        break;

                    case ConversionErrorHandling.Default:
                        result.Add(default!);
                        break;
                }
            }
        }

        return result.ToArray();
    }
    /// <inheritdoc />
    public override TimeSpan[] ReadTimeSpanArray(DataIdentifier identifier, TimeSpan[]? fallback = default) => ReadPrimitiveArray(identifier, fallback, value => new TimeSpan(value.GetValue<long>()));

    /// <inheritdoc />
    public override DateTime[] ReadDateTimeArray(DataIdentifier identifier, DateTime[]? fallback = default) => ReadPrimitiveArray(identifier, fallback, value => value.GetValue<DateTime>());

    /// <inheritdoc />
    public override Guid[] ReadGuidArray(DataIdentifier identifier, Guid[]? fallback = default) => ReadPrimitiveArray(identifier, fallback, value => value.GetValue<Guid>());

    /// <inheritdoc />
    public override TEnum[] ReadEnumArray<TEnum>(DataIdentifier identifier, TEnum[]? fallback = default)
    {
        fallback ??= Array.Empty<TEnum>();

        if (!_node.TryGetPropertyValue(GetPropertyName(identifier), out var node) || node is not JsonArray array)
            return fallback;

        var result = new List<TEnum>(array.Count);

        foreach (var item in array)
        {
            if (item is not JsonValue value)
                continue;

            try
            {
                if (TryFindEnumValue(value.GetValue<string>(), out TEnum enumValue))
                    result.Add(enumValue);
            }
            catch (Exception exception) when (exception is InvalidOperationException or FormatException)
            {
            }
        }

        return result.ToArray();
    }

    /// <inheritdoc />
    public override IObjectReader<JsonReader>[] ReadObjectArray(DataIdentifier identifier, ObjectReaderGeneratorCallback<JsonReader> generator, IObjectReader<JsonReader>[]? fallback = default)
    {
        ArgumentNullException.ThrowIfNull(generator);

        if (!_node.TryGetPropertyValue(GetPropertyName(identifier), out var node) || node is not JsonArray array)
            return fallback ?? Array.Empty<IObjectReader<JsonReader>>();

        var result = new List<IObjectReader<JsonReader>>(array.Count);

        foreach (var item in array)
        {
            if (item is not JsonObject jsonObject)
                continue;

            var reader = CreateReader();
            reader._node = jsonObject;

            var instance = generator(reader);
            if (instance == null)
                continue;

            instance.Read(reader);
            result.Add(instance);
        }

        return result.ToArray();
    }

    /// <inheritdoc />
    public override JsonReader[] ReadArray(DataIdentifier identifier)
    {
        if (!_node.TryGetPropertyValue(GetPropertyName(identifier), out var node) || node is not JsonArray array)
            return Array.Empty<JsonReader>();

        var result = new List<JsonReader>(array.Count);

        foreach (var item in array)
        {
            if (item is not JsonObject jsonObject)
                continue;

            var reader = CreateReader();
            reader._node = jsonObject;
            result.Add(reader);
        }

        return result.ToArray();
    }
}