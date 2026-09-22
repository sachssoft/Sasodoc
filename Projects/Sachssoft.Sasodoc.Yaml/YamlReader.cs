using System.Globalization;
using YamlDotNet.RepresentationModel;

namespace Sachssoft.Documents.Formats.Yaml;

/// <summary>Reads document data from a YAML mapping tree.</summary>
public class YamlReader : FormatReader<YamlReader>
{
    private YamlMappingNode _node = new();
    internal YamlMappingNode Node { get => _node; set => _node = value ?? throw new ArgumentNullException(nameof(value)); }
    /// <summary>
    /// Gets or sets how failed YAML scalar value conversions are handled.
    /// </summary>
    public ConversionErrorHandling ConversionErrorHandling { get; set; }
    /// <inheritdoc />
    public override YamlReader CreateReader() => new() { ConversionErrorHandling = ConversionErrorHandling, Options = Options };
    private string GetPropertyName(DataIdentifier identifier) => ConvertPropertyIdentifier(identifier).Value;
    private bool TryGetNode(DataIdentifier identifier, out YamlNode node) => _node.Children.TryGetValue(new YamlScalarNode(GetPropertyName(identifier)), out node!);
    private bool TryGetScalar(DataIdentifier identifier, out string value) { if (TryGetNode(identifier, out var node) && node is YamlScalarNode scalar && scalar.Value != null) { value = scalar.Value; return true; } value = string.Empty; return false; }
    private T ReadValue<T>(DataIdentifier identifier, T fallback, Func<string, T> converter) { if (TryGetScalar(identifier, out var text)) { try { return converter(text); } catch (Exception ex) when (ex is FormatException or OverflowException or ArgumentException) { } } return ConversionErrorHandling == ConversionErrorHandling.Default ? default! : fallback; }
    private T[] ReadArrayCore<T>(DataIdentifier identifier, T[]? fallback, Func<string, T> converter) { fallback ??= Array.Empty<T>(); if (!TryGetNode(identifier, out var node) || node is not YamlSequenceNode sequence) return fallback; var result = new List<T>(sequence.Children.Count); foreach (var child in sequence.Children) { if (child is YamlScalarNode scalar && scalar.Value != null) { try { result.Add(converter(scalar.Value)); continue; } catch (Exception ex) when (ex is FormatException or OverflowException or ArgumentException) { } } switch (ConversionErrorHandling) { case ConversionErrorHandling.Ignore: continue; case ConversionErrorHandling.Default: result.Add(default!); continue; case ConversionErrorHandling.Replace: case ConversionErrorHandling.Cancel: default: return fallback; } } return result.ToArray(); }
    private bool TryFindEnumValue<TEnum>(string value, out TEnum result) where TEnum : struct, Enum { foreach (var enumValue in Enum.GetValues<TEnum>()) { if (string.Equals(ConvertFieldName(enumValue.ToString()), value, StringComparison.Ordinal)) { result = enumValue; return true; } } result = default; return false; }
    /// <inheritdoc />
    public override bool Contains(DataIdentifier identifier) => TryGetNode(identifier, out _);
    /// <inheritdoc />
    public override bool ReadBoolean(DataIdentifier identifier, bool fallback = default) => ReadValue(identifier, fallback, bool.Parse);
    /// <inheritdoc />
    public override byte ReadByte(DataIdentifier identifier, byte fallback = default) => ReadValue(identifier, fallback, v => byte.Parse(v, NumberStyles.Integer, CultureInfo.InvariantCulture));
    /// <inheritdoc />
    public override sbyte ReadSByte(DataIdentifier identifier, sbyte fallback = default) => ReadValue(identifier, fallback, v => sbyte.Parse(v, NumberStyles.Integer, CultureInfo.InvariantCulture));
    /// <inheritdoc />
    public override short ReadInt16(DataIdentifier identifier, short fallback = default) => ReadValue(identifier, fallback, v => short.Parse(v, NumberStyles.Integer, CultureInfo.InvariantCulture));
    /// <inheritdoc />
    public override ushort ReadUInt16(DataIdentifier identifier, ushort fallback = default) => ReadValue(identifier, fallback, v => ushort.Parse(v, NumberStyles.Integer, CultureInfo.InvariantCulture));
    /// <inheritdoc />
    public override int ReadInt32(DataIdentifier identifier, int fallback = default) => ReadValue(identifier, fallback, v => int.Parse(v, NumberStyles.Integer, CultureInfo.InvariantCulture));
    /// <inheritdoc />
    public override uint ReadUInt32(DataIdentifier identifier, uint fallback = default) => ReadValue(identifier, fallback, v => uint.Parse(v, NumberStyles.Integer, CultureInfo.InvariantCulture));
    /// <inheritdoc />
    public override long ReadInt64(DataIdentifier identifier, long fallback = default) => ReadValue(identifier, fallback, v => long.Parse(v, NumberStyles.Integer, CultureInfo.InvariantCulture));
    /// <inheritdoc />
    public override ulong ReadUInt64(DataIdentifier identifier, ulong fallback = default) => ReadValue(identifier, fallback, v => ulong.Parse(v, NumberStyles.Integer, CultureInfo.InvariantCulture));
    /// <inheritdoc />
    public override float ReadSingle(DataIdentifier identifier, float fallback = default) => ReadValue(identifier, fallback, v => float.Parse(v, NumberStyles.Float, CultureInfo.InvariantCulture));
    /// <inheritdoc />
    public override double ReadDouble(DataIdentifier identifier, double fallback = default) => ReadValue(identifier, fallback, v => double.Parse(v, NumberStyles.Float, CultureInfo.InvariantCulture));
    /// <inheritdoc />
    public override decimal ReadDecimal(DataIdentifier identifier, decimal fallback = default) => ReadValue(identifier, fallback, v => decimal.Parse(v, NumberStyles.Number, CultureInfo.InvariantCulture));
    /// <inheritdoc />
    public override char ReadChar(DataIdentifier identifier, char fallback = default) => ReadValue(identifier, fallback, v => v.Length == 1 ? v[0] : throw new FormatException());
    /// <inheritdoc />
    public override string? ReadString(DataIdentifier identifier, string? fallback = default) => TryGetScalar(identifier, out var value) ? value : fallback;
    /// <inheritdoc />
    public override TimeSpan ReadTimeSpan(DataIdentifier identifier, TimeSpan fallback = default) => ReadValue(identifier, fallback, v => new TimeSpan(long.Parse(v, NumberStyles.Integer, CultureInfo.InvariantCulture)));
    /// <inheritdoc />
    public override DateTime ReadDateTime(DataIdentifier identifier, DateTime fallback = default) => ReadValue(identifier, fallback, v => DateTime.Parse(v, CultureInfo.InvariantCulture, DateTimeStyles.RoundtripKind));
    /// <inheritdoc />
    public override Guid ReadGuid(DataIdentifier identifier, Guid fallback = default) => ReadValue(identifier, fallback, Guid.Parse);
    /// <inheritdoc />
    public override TEnum ReadEnum<TEnum>(DataIdentifier identifier, TEnum fallback = default) { if (TryGetScalar(identifier, out var value) && TryFindEnumValue(value, out TEnum result)) return result; return fallback; }
    /// <inheritdoc />
    public override YamlReader Read(DataIdentifier identifier) { var reader = CreateReader(); if (TryGetNode(identifier, out var node) && node is YamlMappingNode mapping) reader._node = mapping; return reader; }
    /// <inheritdoc />
    public override IObjectReader<YamlReader>? ReadObject(DataIdentifier identifier, ObjectReaderGeneratorCallback<YamlReader> generator, IObjectReader<YamlReader>? fallback = default) { ArgumentNullException.ThrowIfNull(generator); if (!TryGetNode(identifier, out var node) || node is not YamlMappingNode mapping) return fallback; var reader = CreateReader(); reader._node = mapping; var instance = generator(reader); if (instance == null) return fallback; instance.Read(reader); return instance; }
    /// <inheritdoc />
    public override bool[] ReadBooleanArray(DataIdentifier identifier, bool[]? fallback = default) => ReadArrayCore(identifier, fallback, bool.Parse);
    /// <inheritdoc />
    public override byte[] ReadByteArray(DataIdentifier identifier, byte[]? fallback = default) => ReadArrayCore(identifier, fallback, v => byte.Parse(v, CultureInfo.InvariantCulture));
    /// <inheritdoc />
    public override sbyte[] ReadSByteArray(DataIdentifier identifier, sbyte[]? fallback = default) => ReadArrayCore(identifier, fallback, v => sbyte.Parse(v, CultureInfo.InvariantCulture));
    /// <inheritdoc />
    public override short[] ReadInt16Array(DataIdentifier identifier, short[]? fallback = default) => ReadArrayCore(identifier, fallback, v => short.Parse(v, CultureInfo.InvariantCulture));
    /// <inheritdoc />
    public override ushort[] ReadUInt16Array(DataIdentifier identifier, ushort[]? fallback = default) => ReadArrayCore(identifier, fallback, v => ushort.Parse(v, CultureInfo.InvariantCulture));
    /// <inheritdoc />
    public override int[] ReadInt32Array(DataIdentifier identifier, int[]? fallback = default) => ReadArrayCore(identifier, fallback, v => int.Parse(v, CultureInfo.InvariantCulture));
    /// <inheritdoc />
    public override uint[] ReadUInt32Array(DataIdentifier identifier, uint[]? fallback = default) => ReadArrayCore(identifier, fallback, v => uint.Parse(v, CultureInfo.InvariantCulture));
    /// <inheritdoc />
    public override long[] ReadInt64Array(DataIdentifier identifier, long[]? fallback = default) => ReadArrayCore(identifier, fallback, v => long.Parse(v, CultureInfo.InvariantCulture));
    /// <inheritdoc />
    public override ulong[] ReadUInt64Array(DataIdentifier identifier, ulong[]? fallback = default) => ReadArrayCore(identifier, fallback, v => ulong.Parse(v, CultureInfo.InvariantCulture));
    /// <inheritdoc />
    public override float[] ReadSingleArray(DataIdentifier identifier, float[]? fallback = default) => ReadArrayCore(identifier, fallback, v => float.Parse(v, CultureInfo.InvariantCulture));
    /// <inheritdoc />
    public override double[] ReadDoubleArray(DataIdentifier identifier, double[]? fallback = default) => ReadArrayCore(identifier, fallback, v => double.Parse(v, CultureInfo.InvariantCulture));
    /// <inheritdoc />
    public override decimal[] ReadDecimalArray(DataIdentifier identifier, decimal[]? fallback = default) => ReadArrayCore(identifier, fallback, v => decimal.Parse(v, CultureInfo.InvariantCulture));
    /// <inheritdoc />
    public override char[] ReadCharArray(DataIdentifier identifier, char[]? fallback = default) => ReadArrayCore(identifier, fallback, v => v.Length == 1 ? v[0] : throw new FormatException());
    /// <inheritdoc />
    public override string[] ReadStringArray(DataIdentifier identifier, string[]? fallback = default) => ReadArrayCore(identifier, fallback, static v => v);
    /// <inheritdoc />
    public override TimeSpan[] ReadTimeSpanArray(DataIdentifier identifier, TimeSpan[]? fallback = default) => ReadArrayCore(identifier, fallback, v => new TimeSpan(long.Parse(v, CultureInfo.InvariantCulture)));
    /// <inheritdoc />
    public override DateTime[] ReadDateTimeArray(DataIdentifier identifier, DateTime[]? fallback = default) => ReadArrayCore(identifier, fallback, v => DateTime.Parse(v, CultureInfo.InvariantCulture, DateTimeStyles.RoundtripKind));
    /// <inheritdoc />
    public override Guid[] ReadGuidArray(DataIdentifier identifier, Guid[]? fallback = default) => ReadArrayCore(identifier, fallback, Guid.Parse);
    /// <inheritdoc />
    public override TEnum[] ReadEnumArray<TEnum>(DataIdentifier identifier, TEnum[]? fallback = default) { fallback ??= Array.Empty<TEnum>(); if (!TryGetNode(identifier, out var node) || node is not YamlSequenceNode sequence) return fallback; var result = new List<TEnum>(); foreach (var child in sequence.Children) { if (child is YamlScalarNode scalar && scalar.Value != null && TryFindEnumValue(scalar.Value, out TEnum value)) result.Add(value); else if (ConversionErrorHandling == ConversionErrorHandling.Cancel) return fallback; } return result.ToArray(); }
    /// <inheritdoc />
    public override IObjectReader<YamlReader>[] ReadObjectArray(DataIdentifier identifier, ObjectReaderGeneratorCallback<YamlReader> generator, IObjectReader<YamlReader>[]? fallback = default) { ArgumentNullException.ThrowIfNull(generator); fallback ??= Array.Empty<IObjectReader<YamlReader>>(); if (!TryGetNode(identifier, out var node) || node is not YamlSequenceNode sequence) return fallback; var result = new List<IObjectReader<YamlReader>>(); foreach (var child in sequence.Children) { if (child is not YamlMappingNode mapping) continue; var reader = CreateReader(); reader._node = mapping; var instance = generator(reader); if (instance == null) continue; instance.Read(reader); result.Add(instance); } return result.ToArray(); }
    /// <inheritdoc />
    public override YamlReader[] ReadArray(DataIdentifier identifier) { if (!TryGetNode(identifier, out var node) || node is not YamlSequenceNode sequence) return Array.Empty<YamlReader>(); var result = new List<YamlReader>(); foreach (var child in sequence.Children) if (child is YamlMappingNode mapping) { var reader = CreateReader(); reader._node = mapping; result.Add(reader); } return result.ToArray(); }
}
