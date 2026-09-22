namespace Sachssoft.Documents.Formats.Binary;

/// <summary>Writes data to the Sasodoc binary document model.</summary>
public class BinaryWriter : FormatWriter<BinaryWriter>
{
    private BinaryNode _node;

    /// <summary>Initializes a new binary writer.</summary>
    public BinaryWriter() : this(new BinaryNode(), new BinaryOptions()) { }

    internal BinaryWriter(BinaryNode node, BinaryOptions binaryOptions)
    {
        _node = node;
        BinaryOptions = binaryOptions;
    }

    internal BinaryNode Node { get => _node; set => _node = value ?? throw new ArgumentNullException(nameof(value)); }

    /// <summary>Gets the options controlling the binary representation.</summary>
    public BinaryOptions BinaryOptions { get; }

    /// <inheritdoc />
    public override BinaryWriter CreateWriter() => new(new BinaryNode(), BinaryOptions) { Options = Options };

    private DataIdentifier GetIdentifier(DataIdentifier identifier) => ConvertPropertyIdentifier(identifier);
    private void Add(DataIdentifier identifier, BinaryValueType type, object? value) => _node.Entries.Add(new BinaryEntry(GetIdentifier(identifier), type, value));

    /// <inheritdoc />
    public override void WriteBoolean(DataIdentifier identifier, bool value) => Add(identifier, BinaryValueType.Boolean, value);

    /// <inheritdoc />
    public override void WriteByte(DataIdentifier identifier, byte value) => Add(identifier, BinaryValueType.Byte, value);

    /// <inheritdoc />
    public override void WriteSByte(DataIdentifier identifier, sbyte value) => Add(identifier, BinaryValueType.SByte, value);

    /// <inheritdoc />
    public override void WriteInt16(DataIdentifier identifier, short value) => Add(identifier, BinaryValueType.Int16, value);

    /// <inheritdoc />
    public override void WriteUInt16(DataIdentifier identifier, ushort value) => Add(identifier, BinaryValueType.UInt16, value);

    /// <inheritdoc />
    public override void WriteInt32(DataIdentifier identifier, int value) => Add(identifier, BinaryValueType.Int32, value);

    /// <inheritdoc />
    public override void WriteUInt32(DataIdentifier identifier, uint value) => Add(identifier, BinaryValueType.UInt32, value);

    /// <inheritdoc />
    public override void WriteInt64(DataIdentifier identifier, long value) => Add(identifier, BinaryValueType.Int64, value);

    /// <inheritdoc />
    public override void WriteUInt64(DataIdentifier identifier, ulong value) => Add(identifier, BinaryValueType.UInt64, value);

    /// <inheritdoc />
    public override void WriteSingle(DataIdentifier identifier, float value) => Add(identifier, BinaryValueType.Single, value);

    /// <inheritdoc />
    public override void WriteDouble(DataIdentifier identifier, double value) => Add(identifier, BinaryValueType.Double, value);

    /// <inheritdoc />
    public override void WriteDecimal(DataIdentifier identifier, decimal value) => Add(identifier, BinaryValueType.Decimal, value);

    /// <inheritdoc />
    public override void WriteChar(DataIdentifier identifier, char value) => Add(identifier, BinaryValueType.Char, value);

    /// <inheritdoc />
    public override void WriteTimeSpan(DataIdentifier identifier, TimeSpan value) => Add(identifier, BinaryValueType.TimeSpan, value);

    /// <inheritdoc />
    public override void WriteDateTime(DataIdentifier identifier, DateTime value) => Add(identifier, BinaryValueType.DateTime, value);

    /// <inheritdoc />
    public override void WriteGuid(DataIdentifier identifier, Guid value) => Add(identifier, BinaryValueType.Guid, value);

    /// <inheritdoc />
    public override void WriteString(DataIdentifier identifier, string? value) => Add(identifier, value == null ? BinaryValueType.Null : BinaryValueType.String, value);

    /// <inheritdoc />
    public override void WriteEnum<TEnum>(DataIdentifier identifier, TEnum value) => Add(identifier, BinaryValueType.Enum, ConvertFieldName(value.ToString()));

    /// <inheritdoc />
    public override void WriteObject(DataIdentifier identifier, IObjectWriter<BinaryWriter>? value, Action<BinaryWriter>? writerBefore = null)
    {
        if (value == null) { Add(identifier, BinaryValueType.Null, null); return; }
        var writer = CreateWriter(); writerBefore?.Invoke(writer); value.Write(writer); Add(identifier, BinaryValueType.Object, writer._node);
    }

    /// <inheritdoc />
    public override void Write(DataIdentifier identifier, BinaryWriter? value) => Add(identifier, value == null ? BinaryValueType.Null : BinaryValueType.Object, value?._node);


    /// <inheritdoc />
    public override void WriteBooleanArray(DataIdentifier identifier, bool[]? values) { if (values != null) Add(identifier, BinaryValueType.BooleanArray, values); }

    /// <inheritdoc />
    public override void WriteByteArray(DataIdentifier identifier, byte[]? values) { if (values != null) Add(identifier, BinaryValueType.ByteArray, values); }

    /// <inheritdoc />
    public override void WriteSByteArray(DataIdentifier identifier, sbyte[]? values) { if (values != null) Add(identifier, BinaryValueType.SByteArray, values); }

    /// <inheritdoc />
    public override void WriteInt16Array(DataIdentifier identifier, short[]? values) { if (values != null) Add(identifier, BinaryValueType.Int16Array, values); }

    /// <inheritdoc />
    public override void WriteUInt16Array(DataIdentifier identifier, ushort[]? values) { if (values != null) Add(identifier, BinaryValueType.UInt16Array, values); }

    /// <inheritdoc />
    public override void WriteInt32Array(DataIdentifier identifier, int[]? values) { if (values != null) Add(identifier, BinaryValueType.Int32Array, values); }

    /// <inheritdoc />
    public override void WriteUInt32Array(DataIdentifier identifier, uint[]? values) { if (values != null) Add(identifier, BinaryValueType.UInt32Array, values); }

    /// <inheritdoc />
    public override void WriteInt64Array(DataIdentifier identifier, long[]? values) { if (values != null) Add(identifier, BinaryValueType.Int64Array, values); }

    /// <inheritdoc />
    public override void WriteUInt64Array(DataIdentifier identifier, ulong[]? values) { if (values != null) Add(identifier, BinaryValueType.UInt64Array, values); }

    /// <inheritdoc />
    public override void WriteSingleArray(DataIdentifier identifier, float[]? values) { if (values != null) Add(identifier, BinaryValueType.SingleArray, values); }

    /// <inheritdoc />
    public override void WriteDoubleArray(DataIdentifier identifier, double[]? values) { if (values != null) Add(identifier, BinaryValueType.DoubleArray, values); }

    /// <inheritdoc />
    public override void WriteDecimalArray(DataIdentifier identifier, decimal[]? values) { if (values != null) Add(identifier, BinaryValueType.DecimalArray, values); }

    /// <inheritdoc />
    public override void WriteCharArray(DataIdentifier identifier, char[]? values) { if (values != null) Add(identifier, BinaryValueType.CharArray, values); }

    /// <inheritdoc />
    public override void WriteTimeSpanArray(DataIdentifier identifier, TimeSpan[]? values) { if (values != null) Add(identifier, BinaryValueType.TimeSpanArray, values); }

    /// <inheritdoc />
    public override void WriteDateTimeArray(DataIdentifier identifier, DateTime[]? values) { if (values != null) Add(identifier, BinaryValueType.DateTimeArray, values); }

    /// <inheritdoc />
    public override void WriteGuidArray(DataIdentifier identifier, Guid[]? values) { if (values != null) Add(identifier, BinaryValueType.GuidArray, values); }

    /// <inheritdoc />
    public override void WriteStringArray(DataIdentifier identifier, string[]? values) { if (values != null) Add(identifier, BinaryValueType.StringArray, values); }

    /// <inheritdoc />
    public override void WriteEnumArray<TEnum>(DataIdentifier identifier, TEnum[]? values)
    {
        if (values == null) return;
        var names = new string[values.Length];
        for (int i = 0; i < values.Length; i++) names[i] = ConvertFieldName(values[i].ToString());
        Add(identifier, BinaryValueType.EnumArray, names);
    }

    /// <inheritdoc />
    public override void WriteObjectArray(DataIdentifier identifier, IObjectWriter<BinaryWriter>[]? values, Action<BinaryWriter>? writerBefore = null)
    {
        if (values == null) return;
        var nodes = new BinaryNode[values.Length];
        for (int i = 0; i < values.Length; i++) { var writer = CreateWriter(); writerBefore?.Invoke(writer); values[i].Write(writer); nodes[i] = writer._node; }
        Add(identifier, BinaryValueType.Array, nodes);
    }

    /// <inheritdoc />
    public override void WriteArray(DataIdentifier identifier, BinaryWriter[]? values)
    {
        if (values == null) return;
        var nodes = new BinaryNode[values.Length]; for (int i = 0; i < values.Length; i++) nodes[i] = values[i]._node;
        Add(identifier, BinaryValueType.Array, nodes);
    }
}
