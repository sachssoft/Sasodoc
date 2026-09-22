namespace Sachssoft.Documents.Formats.Binary;

/// <summary>Reads data from the Sasodoc binary document model.</summary>
public class BinaryReader : FormatReader<BinaryReader>
{
    private BinaryNode _node;
    private int _sequentialIndex;

    /// <summary>Initializes a new binary reader.</summary>
    public BinaryReader() : this(new BinaryNode(), new BinaryOptions()) { }

    internal BinaryReader(BinaryNode node, BinaryOptions binaryOptions)
    {
        _node = node;
        BinaryOptions = binaryOptions;
    }

    internal BinaryNode Node { get => _node; set => _node = value ?? throw new ArgumentNullException(nameof(value)); }

    /// <summary>Gets the options controlling the binary representation.</summary>
    public BinaryOptions BinaryOptions { get; }

    /// <inheritdoc />
    public override BinaryReader CreateReader() => new(new BinaryNode(), BinaryOptions) { Options = Options };

    private DataIdentifier GetIdentifier(DataIdentifier identifier) => ConvertPropertyIdentifier(identifier);

    private BinaryEntry? Find(DataIdentifier identifier, BinaryValueType? expectedType = null)
    {
        if (BinaryOptions.IdentifierMode == BinaryIdentifierMode.None)
        {
            if (_sequentialIndex >= _node.Entries.Count) return null;
            var sequential = _node.Entries[_sequentialIndex];
            if (expectedType != null && sequential.Type != expectedType && sequential.Type != BinaryValueType.Null) return null;
            _sequentialIndex++;
            return sequential;
        }

        var converted = GetIdentifier(identifier);
        foreach (var entry in _node.Entries)
            if (entry.Identifier == converted && (expectedType == null || entry.Type == expectedType || entry.Type == BinaryValueType.Null)) return entry;
        return null;
    }

    /// <inheritdoc />
    public override bool Contains(DataIdentifier identifier)
    {
        if (BinaryOptions.IdentifierMode == BinaryIdentifierMode.None) return _sequentialIndex < _node.Entries.Count;
        var converted = GetIdentifier(identifier); foreach (var entry in _node.Entries) if (entry.Identifier == converted) return true; return false;
    }

    /// <inheritdoc />
    public override bool ReadBoolean(DataIdentifier identifier, bool fallback = default) => Find(identifier, BinaryValueType.Boolean)?.Value is bool value ? value : fallback;

    /// <inheritdoc />
    public override byte ReadByte(DataIdentifier identifier, byte fallback = default) => Find(identifier, BinaryValueType.Byte)?.Value is byte value ? value : fallback;

    /// <inheritdoc />
    public override sbyte ReadSByte(DataIdentifier identifier, sbyte fallback = default) => Find(identifier, BinaryValueType.SByte)?.Value is sbyte value ? value : fallback;

    /// <inheritdoc />
    public override short ReadInt16(DataIdentifier identifier, short fallback = default) => Find(identifier, BinaryValueType.Int16)?.Value is short value ? value : fallback;

    /// <inheritdoc />
    public override ushort ReadUInt16(DataIdentifier identifier, ushort fallback = default) => Find(identifier, BinaryValueType.UInt16)?.Value is ushort value ? value : fallback;

    /// <inheritdoc />
    public override int ReadInt32(DataIdentifier identifier, int fallback = default) => Find(identifier, BinaryValueType.Int32)?.Value is int value ? value : fallback;

    /// <inheritdoc />
    public override uint ReadUInt32(DataIdentifier identifier, uint fallback = default) => Find(identifier, BinaryValueType.UInt32)?.Value is uint value ? value : fallback;

    /// <inheritdoc />
    public override long ReadInt64(DataIdentifier identifier, long fallback = default) => Find(identifier, BinaryValueType.Int64)?.Value is long value ? value : fallback;

    /// <inheritdoc />
    public override ulong ReadUInt64(DataIdentifier identifier, ulong fallback = default) => Find(identifier, BinaryValueType.UInt64)?.Value is ulong value ? value : fallback;

    /// <inheritdoc />
    public override float ReadSingle(DataIdentifier identifier, float fallback = default) => Find(identifier, BinaryValueType.Single)?.Value is float value ? value : fallback;

    /// <inheritdoc />
    public override double ReadDouble(DataIdentifier identifier, double fallback = default) => Find(identifier, BinaryValueType.Double)?.Value is double value ? value : fallback;

    /// <inheritdoc />
    public override decimal ReadDecimal(DataIdentifier identifier, decimal fallback = default) => Find(identifier, BinaryValueType.Decimal)?.Value is decimal value ? value : fallback;

    /// <inheritdoc />
    public override char ReadChar(DataIdentifier identifier, char fallback = default) => Find(identifier, BinaryValueType.Char)?.Value is char value ? value : fallback;

    /// <inheritdoc />
    public override TimeSpan ReadTimeSpan(DataIdentifier identifier, TimeSpan fallback = default) => Find(identifier, BinaryValueType.TimeSpan)?.Value is TimeSpan value ? value : fallback;

    /// <inheritdoc />
    public override DateTime ReadDateTime(DataIdentifier identifier, DateTime fallback = default) => Find(identifier, BinaryValueType.DateTime)?.Value is DateTime value ? value : fallback;

    /// <inheritdoc />
    public override Guid ReadGuid(DataIdentifier identifier, Guid fallback = default) => Find(identifier, BinaryValueType.Guid)?.Value is Guid value ? value : fallback;

    /// <inheritdoc />
    public override string? ReadString(DataIdentifier identifier, string? fallback = default)
    {
        var entry = Find(identifier); return entry?.Type == BinaryValueType.Null ? null : entry?.Type == BinaryValueType.String && entry.Value is string value ? value : fallback;
    }

    /// <inheritdoc />
    public override TEnum ReadEnum<TEnum>(DataIdentifier identifier, TEnum fallback = default)
    {
        var entry = Find(identifier, BinaryValueType.Enum);
        return entry?.Value is string value && Enum.TryParse<TEnum>(value, true, out var result) ? result : fallback;
    }

    /// <inheritdoc />
    public override IObjectReader<BinaryReader>? ReadObject(DataIdentifier identifier, ObjectReaderGeneratorCallback<BinaryReader> generator, IObjectReader<BinaryReader>? fallback = default)
    {
        ArgumentNullException.ThrowIfNull(generator);
        var entry = Find(identifier, BinaryValueType.Object); if (entry?.Value is not BinaryNode node) return fallback;
        var reader = new BinaryReader(node, BinaryOptions) { Options = Options }; var result = generator(reader); result?.Read(reader); return result ?? fallback;
    }

    /// <inheritdoc />
    public override BinaryReader Read(DataIdentifier identifier)
    {
        var entry = Find(identifier, BinaryValueType.Object); return entry?.Value is BinaryNode node ? new BinaryReader(node, BinaryOptions) { Options = Options } : CreateReader();
    }


    /// <inheritdoc />
    public override bool[] ReadBooleanArray(DataIdentifier identifier, bool[]? fallback = default) => Find(identifier, BinaryValueType.BooleanArray)?.Value is bool[] values ? values : fallback ?? Array.Empty<bool>();

    /// <inheritdoc />
    public override byte[] ReadByteArray(DataIdentifier identifier, byte[]? fallback = default) => Find(identifier, BinaryValueType.ByteArray)?.Value is byte[] values ? values : fallback ?? Array.Empty<byte>();

    /// <inheritdoc />
    public override sbyte[] ReadSByteArray(DataIdentifier identifier, sbyte[]? fallback = default) => Find(identifier, BinaryValueType.SByteArray)?.Value is sbyte[] values ? values : fallback ?? Array.Empty<sbyte>();

    /// <inheritdoc />
    public override short[] ReadInt16Array(DataIdentifier identifier, short[]? fallback = default) => Find(identifier, BinaryValueType.Int16Array)?.Value is short[] values ? values : fallback ?? Array.Empty<short>();

    /// <inheritdoc />
    public override ushort[] ReadUInt16Array(DataIdentifier identifier, ushort[]? fallback = default) => Find(identifier, BinaryValueType.UInt16Array)?.Value is ushort[] values ? values : fallback ?? Array.Empty<ushort>();

    /// <inheritdoc />
    public override int[] ReadInt32Array(DataIdentifier identifier, int[]? fallback = default) => Find(identifier, BinaryValueType.Int32Array)?.Value is int[] values ? values : fallback ?? Array.Empty<int>();

    /// <inheritdoc />
    public override uint[] ReadUInt32Array(DataIdentifier identifier, uint[]? fallback = default) => Find(identifier, BinaryValueType.UInt32Array)?.Value is uint[] values ? values : fallback ?? Array.Empty<uint>();

    /// <inheritdoc />
    public override long[] ReadInt64Array(DataIdentifier identifier, long[]? fallback = default) => Find(identifier, BinaryValueType.Int64Array)?.Value is long[] values ? values : fallback ?? Array.Empty<long>();

    /// <inheritdoc />
    public override ulong[] ReadUInt64Array(DataIdentifier identifier, ulong[]? fallback = default) => Find(identifier, BinaryValueType.UInt64Array)?.Value is ulong[] values ? values : fallback ?? Array.Empty<ulong>();

    /// <inheritdoc />
    public override float[] ReadSingleArray(DataIdentifier identifier, float[]? fallback = default) => Find(identifier, BinaryValueType.SingleArray)?.Value is float[] values ? values : fallback ?? Array.Empty<float>();

    /// <inheritdoc />
    public override double[] ReadDoubleArray(DataIdentifier identifier, double[]? fallback = default) => Find(identifier, BinaryValueType.DoubleArray)?.Value is double[] values ? values : fallback ?? Array.Empty<double>();

    /// <inheritdoc />
    public override decimal[] ReadDecimalArray(DataIdentifier identifier, decimal[]? fallback = default) => Find(identifier, BinaryValueType.DecimalArray)?.Value is decimal[] values ? values : fallback ?? Array.Empty<decimal>();

    /// <inheritdoc />
    public override char[] ReadCharArray(DataIdentifier identifier, char[]? fallback = default) => Find(identifier, BinaryValueType.CharArray)?.Value is char[] values ? values : fallback ?? Array.Empty<char>();

    /// <inheritdoc />
    public override TimeSpan[] ReadTimeSpanArray(DataIdentifier identifier, TimeSpan[]? fallback = default) => Find(identifier, BinaryValueType.TimeSpanArray)?.Value is TimeSpan[] values ? values : fallback ?? Array.Empty<TimeSpan>();

    /// <inheritdoc />
    public override DateTime[] ReadDateTimeArray(DataIdentifier identifier, DateTime[]? fallback = default) => Find(identifier, BinaryValueType.DateTimeArray)?.Value is DateTime[] values ? values : fallback ?? Array.Empty<DateTime>();

    /// <inheritdoc />
    public override Guid[] ReadGuidArray(DataIdentifier identifier, Guid[]? fallback = default) => Find(identifier, BinaryValueType.GuidArray)?.Value is Guid[] values ? values : fallback ?? Array.Empty<Guid>();

    /// <inheritdoc />
    public override string[] ReadStringArray(DataIdentifier identifier, string[]? fallback = default) => Find(identifier, BinaryValueType.StringArray)?.Value is string[] values ? values : fallback ?? Array.Empty<string>();

    /// <inheritdoc />
    public override TEnum[] ReadEnumArray<TEnum>(DataIdentifier identifier, TEnum[]? fallback = default)
    {
        if (Find(identifier, BinaryValueType.EnumArray)?.Value is not string[] values) return fallback ?? Array.Empty<TEnum>();
        var result = new TEnum[values.Length]; for (int i = 0; i < values.Length; i++) if (!Enum.TryParse(values[i], true, out result[i])) return fallback ?? Array.Empty<TEnum>(); return result;
    }

    /// <inheritdoc />
    public override IObjectReader<BinaryReader>[] ReadObjectArray(DataIdentifier identifier, ObjectReaderGeneratorCallback<BinaryReader> generator, IObjectReader<BinaryReader>[]? fallback = default)
    {
        ArgumentNullException.ThrowIfNull(generator);
        if (Find(identifier, BinaryValueType.Array)?.Value is not BinaryNode[] nodes) return fallback ?? Array.Empty<IObjectReader<BinaryReader>>();
        var result = new List<IObjectReader<BinaryReader>>(nodes.Length);
        foreach (var node in nodes) { var reader = new BinaryReader(node, BinaryOptions) { Options = Options }; var item = generator(reader); if (item != null) { item.Read(reader); result.Add(item); } }
        return result.ToArray();
    }

    /// <inheritdoc />
    public override BinaryReader[] ReadArray(DataIdentifier identifier)
    {
        if (Find(identifier, BinaryValueType.Array)?.Value is not BinaryNode[] nodes) return Array.Empty<BinaryReader>();
        var result = new BinaryReader[nodes.Length]; for (int i = 0; i < nodes.Length; i++) result[i] = new BinaryReader(nodes[i], BinaryOptions) { Options = Options }; return result;
    }
}
