using System.Globalization;
using YamlDotNet.RepresentationModel;

namespace Sachssoft.Documents.Formats.Yaml;

/// <summary>Writes document data to a YAML mapping tree.</summary>
public class YamlWriter : FormatWriter<YamlWriter>
{
    private YamlMappingNode _node = new();
    internal YamlMappingNode Node { get => _node; set => _node = value ?? throw new ArgumentNullException(nameof(value)); }
    /// <inheritdoc />
    public override YamlWriter CreateWriter() => new() { Options = Options };
    private string GetPropertyName(DataIdentifier identifier) => ConvertPropertyIdentifier(identifier).Value;
    private void Set(DataIdentifier identifier, YamlNode value) => _node.Children[new YamlScalarNode(GetPropertyName(identifier))] = value;
    private static YamlScalarNode Scalar(string? value) => new(value ?? string.Empty);
    private void WriteArrayCore<T>(DataIdentifier identifier, T[]? values, Func<T, YamlNode> factory) { var sequence = new YamlSequenceNode(); if (values != null) foreach (var value in values) sequence.Add(factory(value)); Set(identifier, sequence); }
    /// <inheritdoc />
    public override void Write(DataIdentifier identifier, YamlWriter? value) { if (value == null) throw new ArgumentNullException(nameof(value)); Set(identifier, value._node); }
    /// <inheritdoc />
    public override void WriteArray(DataIdentifier identifier, YamlWriter[]? values) { var sequence = new YamlSequenceNode(); if (values != null) foreach (var value in values) sequence.Add(value._node); Set(identifier, sequence); }
    /// <inheritdoc />
    public override void WriteBoolean(DataIdentifier identifier, bool value) => Set(identifier, Scalar(value ? "true" : "false"));
    /// <inheritdoc />
    public override void WriteByte(DataIdentifier identifier, byte value) => Set(identifier, Scalar(value.ToString(CultureInfo.InvariantCulture)));
    /// <inheritdoc />
    public override void WriteSByte(DataIdentifier identifier, sbyte value) => Set(identifier, Scalar(value.ToString(CultureInfo.InvariantCulture)));
    /// <inheritdoc />
    public override void WriteInt16(DataIdentifier identifier, short value) => Set(identifier, Scalar(value.ToString(CultureInfo.InvariantCulture)));
    /// <inheritdoc />
    public override void WriteUInt16(DataIdentifier identifier, ushort value) => Set(identifier, Scalar(value.ToString(CultureInfo.InvariantCulture)));
    /// <inheritdoc />
    public override void WriteInt32(DataIdentifier identifier, int value) => Set(identifier, Scalar(value.ToString(CultureInfo.InvariantCulture)));
    /// <inheritdoc />
    public override void WriteUInt32(DataIdentifier identifier, uint value) => Set(identifier, Scalar(value.ToString(CultureInfo.InvariantCulture)));
    /// <inheritdoc />
    public override void WriteInt64(DataIdentifier identifier, long value) => Set(identifier, Scalar(value.ToString(CultureInfo.InvariantCulture)));
    /// <inheritdoc />
    public override void WriteUInt64(DataIdentifier identifier, ulong value) => Set(identifier, Scalar(value.ToString(CultureInfo.InvariantCulture)));
    /// <inheritdoc />
    public override void WriteSingle(DataIdentifier identifier, float value) => Set(identifier, Scalar(value.ToString("R", CultureInfo.InvariantCulture)));
    /// <inheritdoc />
    public override void WriteDouble(DataIdentifier identifier, double value) => Set(identifier, Scalar(value.ToString("R", CultureInfo.InvariantCulture)));
    /// <inheritdoc />
    public override void WriteDecimal(DataIdentifier identifier, decimal value) => Set(identifier, Scalar(value.ToString(CultureInfo.InvariantCulture)));
    /// <inheritdoc />
    public override void WriteChar(DataIdentifier identifier, char value) => Set(identifier, Scalar(value.ToString()));
    /// <inheritdoc />
    public override void WriteString(DataIdentifier identifier, string? value) => Set(identifier, Scalar(value));
    /// <inheritdoc />
    public override void WriteTimeSpan(DataIdentifier identifier, TimeSpan value) => WriteInt64(identifier, value.Ticks);
    /// <inheritdoc />
    public override void WriteDateTime(DataIdentifier identifier, DateTime value) => Set(identifier, Scalar(value.ToString("O", CultureInfo.InvariantCulture)));
    /// <inheritdoc />
    public override void WriteGuid(DataIdentifier identifier, Guid value) => Set(identifier, Scalar(value.ToString("D")));
    /// <inheritdoc />
    public override void WriteEnum<TEnum>(DataIdentifier identifier, TEnum value) => Set(identifier, Scalar(ConvertFieldName(value.ToString())));
    /// <inheritdoc />
    public override void WriteBooleanArray(DataIdentifier identifier, bool[]? values) => WriteArrayCore(identifier, values, v => Scalar(v ? "true" : "false"));
    /// <inheritdoc />
    public override void WriteByteArray(DataIdentifier identifier, byte[]? values) => WriteArrayCore(identifier, values, v => Scalar(v.ToString(CultureInfo.InvariantCulture)));
    /// <inheritdoc />
    public override void WriteSByteArray(DataIdentifier identifier, sbyte[]? values) => WriteArrayCore(identifier, values, v => Scalar(v.ToString(CultureInfo.InvariantCulture)));
    /// <inheritdoc />
    public override void WriteInt16Array(DataIdentifier identifier, short[]? values) => WriteArrayCore(identifier, values, v => Scalar(v.ToString(CultureInfo.InvariantCulture)));
    /// <inheritdoc />
    public override void WriteUInt16Array(DataIdentifier identifier, ushort[]? values) => WriteArrayCore(identifier, values, v => Scalar(v.ToString(CultureInfo.InvariantCulture)));
    /// <inheritdoc />
    public override void WriteInt32Array(DataIdentifier identifier, int[]? values) => WriteArrayCore(identifier, values, v => Scalar(v.ToString(CultureInfo.InvariantCulture)));
    /// <inheritdoc />
    public override void WriteUInt32Array(DataIdentifier identifier, uint[]? values) => WriteArrayCore(identifier, values, v => Scalar(v.ToString(CultureInfo.InvariantCulture)));
    /// <inheritdoc />
    public override void WriteInt64Array(DataIdentifier identifier, long[]? values) => WriteArrayCore(identifier, values, v => Scalar(v.ToString(CultureInfo.InvariantCulture)));
    /// <inheritdoc />
    public override void WriteUInt64Array(DataIdentifier identifier, ulong[]? values) => WriteArrayCore(identifier, values, v => Scalar(v.ToString(CultureInfo.InvariantCulture)));
    /// <inheritdoc />
    public override void WriteSingleArray(DataIdentifier identifier, float[]? values) => WriteArrayCore(identifier, values, v => Scalar(v.ToString("R", CultureInfo.InvariantCulture)));
    /// <inheritdoc />
    public override void WriteDoubleArray(DataIdentifier identifier, double[]? values) => WriteArrayCore(identifier, values, v => Scalar(v.ToString("R", CultureInfo.InvariantCulture)));
    /// <inheritdoc />
    public override void WriteDecimalArray(DataIdentifier identifier, decimal[]? values) => WriteArrayCore(identifier, values, v => Scalar(v.ToString(CultureInfo.InvariantCulture)));
    /// <inheritdoc />
    public override void WriteCharArray(DataIdentifier identifier, char[]? values) => WriteArrayCore(identifier, values, v => Scalar(v.ToString()));
    /// <inheritdoc />
    public override void WriteStringArray(DataIdentifier identifier, string[]? values) => WriteArrayCore(identifier, values, v => Scalar(v));
    /// <inheritdoc />
    public override void WriteTimeSpanArray(DataIdentifier identifier, TimeSpan[]? values) => WriteArrayCore(identifier, values, v => Scalar(v.Ticks.ToString(CultureInfo.InvariantCulture)));
    /// <inheritdoc />
    public override void WriteDateTimeArray(DataIdentifier identifier, DateTime[]? values) => WriteArrayCore(identifier, values, v => Scalar(v.ToString("O", CultureInfo.InvariantCulture)));
    /// <inheritdoc />
    public override void WriteGuidArray(DataIdentifier identifier, Guid[]? values) => WriteArrayCore(identifier, values, v => Scalar(v.ToString("D")));
    /// <inheritdoc />
    public override void WriteEnumArray<TEnum>(DataIdentifier identifier, TEnum[]? values) => WriteArrayCore(identifier, values, v => Scalar(ConvertFieldName(v.ToString())));
    /// <inheritdoc />
    public override void WriteObject(DataIdentifier identifier, IObjectWriter<YamlWriter>? value, Action<YamlWriter>? writerBefore = null) { if (value == null) return; var writer = CreateWriter(); writerBefore?.Invoke(writer); value.Write(writer); Set(identifier, writer._node); }
    /// <inheritdoc />
    public override void WriteObjectArray(DataIdentifier identifier, IObjectWriter<YamlWriter>[]? values, Action<YamlWriter>? writerBefore = null) { if (values == null) return; var sequence = new YamlSequenceNode(); foreach (var value in values) { var writer = CreateWriter(); writerBefore?.Invoke(writer); value.Write(writer); sequence.Add(writer._node); } Set(identifier, sequence); }
}
