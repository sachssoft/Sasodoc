using System.Text;
using IOReader = System.IO.BinaryReader;
using IOWriter = System.IO.BinaryWriter;

namespace Sachssoft.Documents.Formats.Binary;

internal static class BinaryCodec
{
    private const byte Version = 1;

    public static void Write(Stream stream, BinaryNode root, BinaryOptions options)
    {
        ArgumentNullException.ThrowIfNull(stream);
        ArgumentNullException.ThrowIfNull(options);
        if (options.Magic.IsEmpty) throw new InvalidOperationException("The binary magic value cannot be empty.");

        using var writer = new IOWriter(stream, Encoding.UTF8, true);
        writer.Write(options.Magic.Span);
        writer.Write(Version);
        writer.Write((byte)options.IdentifierMode);
        writer.Write((byte)options.IdentifierLengthEncoding);
        writer.Write((byte)options.LengthEncoding);

        List<DataIdentifier>? registry = null;
        Dictionary<DataIdentifier, uint>? indexes = null;
        if (options.IdentifierMode == BinaryIdentifierMode.Registry)
        {
            registry = CollectIdentifiers(root);
            indexes = new Dictionary<DataIdentifier, uint>(registry.Count);
            for (int i = 0; i < registry.Count; i++) indexes[registry[i]] = checked((uint)i);
            WriteLength(writer, checked((uint)registry.Count), options.LengthEncoding);
            foreach (var identifier in registry) WriteIdentifierText(writer, identifier, options.IdentifierLengthEncoding);
        }

        WriteNode(writer, root, options, indexes);
    }

    public static BinaryNode Read(Stream stream, BinaryOptions options)
    {
        ArgumentNullException.ThrowIfNull(stream);
        ArgumentNullException.ThrowIfNull(options);
        if (options.Magic.IsEmpty) throw new InvalidOperationException("The binary magic value cannot be empty.");

        using var reader = new IOReader(stream, Encoding.UTF8, true);
        var expectedMagic = options.Magic.Span;
        var actualMagic = reader.ReadBytes(expectedMagic.Length);
        if (!actualMagic.AsSpan().SequenceEqual(expectedMagic)) throw new InvalidDataException("The binary document magic value is invalid.");
        if (reader.ReadByte() != Version) throw new InvalidDataException("The binary document version is not supported.");

        var identifierMode = (BinaryIdentifierMode)reader.ReadByte();
        var identifierLengthEncoding = (BinaryLengthEncoding)reader.ReadByte();
        var lengthEncoding = (BinaryLengthEncoding)reader.ReadByte();
        ValidateEnum(identifierMode, nameof(BinaryIdentifierMode));
        ValidateEnum(identifierLengthEncoding, nameof(BinaryLengthEncoding));
        ValidateEnum(lengthEncoding, nameof(BinaryLengthEncoding));
        options.IdentifierMode = identifierMode;
        options.IdentifierLengthEncoding = identifierLengthEncoding;
        options.LengthEncoding = lengthEncoding;

        List<DataIdentifier>? registry = null;
        if (identifierMode == BinaryIdentifierMode.Registry)
        {
            uint count = ReadLength(reader, lengthEncoding);
            registry = new List<DataIdentifier>(checked((int)count));
            for (uint i = 0; i < count; i++) registry.Add(ReadIdentifierText(reader, identifierLengthEncoding));
        }

        return ReadNode(reader, identifierMode, identifierLengthEncoding, lengthEncoding, registry);
    }

    private static void WriteNode(IOWriter writer, BinaryNode node, BinaryOptions options, Dictionary<DataIdentifier, uint>? registry)
    {
        WriteLength(writer, checked((uint)node.Entries.Count), options.LengthEncoding);
        foreach (var entry in node.Entries)
        {
            WriteIdentifier(writer, entry.Identifier, options, registry);
            writer.Write((byte)entry.Type);
            WriteValue(writer, entry.Type, entry.Value, options, registry);
        }
    }

    private static BinaryNode ReadNode(IOReader reader, BinaryIdentifierMode identifierMode, BinaryLengthEncoding identifierLengthEncoding, BinaryLengthEncoding lengthEncoding, List<DataIdentifier>? registry)
    {
        var node = new BinaryNode();
        uint count = ReadLength(reader, lengthEncoding);
        for (uint i = 0; i < count; i++)
        {
            var identifier = ReadIdentifier(reader, identifierMode, identifierLengthEncoding, lengthEncoding, registry);
            var type = (BinaryValueType)reader.ReadByte();
            if (!Enum.IsDefined(type)) throw new InvalidDataException($"Unknown binary value type '{(byte)type}'.");
            node.Entries.Add(new BinaryEntry(identifier, type, ReadValue(reader, type, identifierMode, identifierLengthEncoding, lengthEncoding, registry)));
        }
        return node;
    }

    private static void WriteIdentifier(IOWriter writer, DataIdentifier identifier, BinaryOptions options, Dictionary<DataIdentifier, uint>? registry)
    {
        switch (options.IdentifierMode)
        {
            case BinaryIdentifierMode.None: return;
            case BinaryIdentifierMode.Inline: WriteIdentifierText(writer, identifier, options.IdentifierLengthEncoding); return;
            case BinaryIdentifierMode.Registry:
                if (registry == null || !registry.TryGetValue(identifier, out uint index)) throw new InvalidOperationException("Identifier is missing from the binary registry.");
                WriteLength(writer, index, options.LengthEncoding); return;
            default: throw new InvalidOperationException("Unsupported identifier mode.");
        }
    }

    private static DataIdentifier ReadIdentifier(IOReader reader, BinaryIdentifierMode mode, BinaryLengthEncoding identifierLengthEncoding, BinaryLengthEncoding lengthEncoding, List<DataIdentifier>? registry)
    {
        return mode switch
        {
            BinaryIdentifierMode.None => new DataIdentifier(string.Empty),
            BinaryIdentifierMode.Inline => ReadIdentifierText(reader, identifierLengthEncoding),
            BinaryIdentifierMode.Registry => ReadRegistryIdentifier(reader, lengthEncoding, registry),
            _ => throw new InvalidDataException("Unsupported identifier mode.")
        };
    }

    private static DataIdentifier ReadRegistryIdentifier(IOReader reader, BinaryLengthEncoding lengthEncoding, List<DataIdentifier>? registry)
    {
        if (registry == null) throw new InvalidDataException("The identifier registry is missing.");
        uint index = ReadLength(reader, lengthEncoding);
        if (index >= registry.Count) throw new InvalidDataException("The identifier registry index is out of range.");
        return registry[(int)index];
    }

    private static void WriteIdentifierText(IOWriter writer, DataIdentifier identifier, BinaryLengthEncoding encoding)
    {
        writer.Write((byte)identifier.Kind);
        WriteUtf8(writer, identifier.Value, encoding);
    }

    private static DataIdentifier ReadIdentifierText(IOReader reader, BinaryLengthEncoding encoding)
    {
        var kind = (DataIdentifierKind)reader.ReadByte();
        return new DataIdentifier(ReadUtf8(reader, encoding), kind);
    }

    private static List<DataIdentifier> CollectIdentifiers(BinaryNode root)
    {
        var result = new List<DataIdentifier>();
        var set = new HashSet<DataIdentifier>();
        Collect(root, result, set);
        return result;

        static void Collect(BinaryNode node, List<DataIdentifier> result, HashSet<DataIdentifier> set)
        {
            foreach (var entry in node.Entries)
            {
                if (set.Add(entry.Identifier)) result.Add(entry.Identifier);
                if (entry.Value is BinaryNode child) Collect(child, result, set);
                else if (entry.Value is BinaryNode[] children) foreach (var item in children) Collect(item, result, set);
            }
        }
    }

    private static void WriteValue(IOWriter writer, BinaryValueType type, object? value, BinaryOptions options, Dictionary<DataIdentifier, uint>? registry)
    {
        switch (type)
        {
            case BinaryValueType.Null: return;
            case BinaryValueType.Boolean: writer.Write((bool)value!); return;
            case BinaryValueType.Byte: writer.Write((byte)value!); return;
            case BinaryValueType.SByte: writer.Write((sbyte)value!); return;
            case BinaryValueType.Int16: writer.Write((short)value!); return;
            case BinaryValueType.UInt16: writer.Write((ushort)value!); return;
            case BinaryValueType.Int32: writer.Write((int)value!); return;
            case BinaryValueType.UInt32: writer.Write((uint)value!); return;
            case BinaryValueType.Int64: writer.Write((long)value!); return;
            case BinaryValueType.UInt64: writer.Write((ulong)value!); return;
            case BinaryValueType.Single: writer.Write((float)value!); return;
            case BinaryValueType.Double: writer.Write((double)value!); return;
            case BinaryValueType.Decimal: writer.Write((decimal)value!); return;
            case BinaryValueType.Char: writer.Write((char)value!); return;
            case BinaryValueType.String: WriteNullableString(writer, (string?)value, options.LengthEncoding); return;
            case BinaryValueType.TimeSpan: writer.Write(((TimeSpan)value!).Ticks); return;
            case BinaryValueType.DateTime: writer.Write(((DateTime)value!).ToBinary()); return;
            case BinaryValueType.Guid: writer.Write(((Guid)value!).ToByteArray()); return;
            case BinaryValueType.Enum: WriteUtf8(writer, (string)value!, options.LengthEncoding); return;
            case BinaryValueType.Object: WriteNode(writer, (BinaryNode)value!, options, registry); return;
            case BinaryValueType.Array:
                var nodes = (BinaryNode[])value!; WriteLength(writer, checked((uint)nodes.Length), options.LengthEncoding); foreach (var node in nodes) WriteNode(writer, node, options, registry); return;
            default: WriteArrayValue(writer, type, value!, options.LengthEncoding); return;
        }
    }

    private static object? ReadValue(IOReader reader, BinaryValueType type, BinaryIdentifierMode mode, BinaryLengthEncoding identifierLengthEncoding, BinaryLengthEncoding lengthEncoding, List<DataIdentifier>? registry)
    {
        return type switch
        {
            BinaryValueType.Null => null,
            BinaryValueType.Boolean => reader.ReadBoolean(), BinaryValueType.Byte => reader.ReadByte(), BinaryValueType.SByte => reader.ReadSByte(),
            BinaryValueType.Int16 => reader.ReadInt16(), BinaryValueType.UInt16 => reader.ReadUInt16(), BinaryValueType.Int32 => reader.ReadInt32(), BinaryValueType.UInt32 => reader.ReadUInt32(),
            BinaryValueType.Int64 => reader.ReadInt64(), BinaryValueType.UInt64 => reader.ReadUInt64(), BinaryValueType.Single => reader.ReadSingle(), BinaryValueType.Double => reader.ReadDouble(), BinaryValueType.Decimal => reader.ReadDecimal(), BinaryValueType.Char => reader.ReadChar(),
            BinaryValueType.String => ReadNullableString(reader, lengthEncoding), BinaryValueType.TimeSpan => TimeSpan.FromTicks(reader.ReadInt64()), BinaryValueType.DateTime => DateTime.FromBinary(reader.ReadInt64()), BinaryValueType.Guid => new Guid(ReadExact(reader, 16)), BinaryValueType.Enum => ReadUtf8(reader, lengthEncoding),
            BinaryValueType.Object => ReadNode(reader, mode, identifierLengthEncoding, lengthEncoding, registry),
            BinaryValueType.Array => ReadNodeArray(reader, mode, identifierLengthEncoding, lengthEncoding, registry),
            _ => ReadArrayValue(reader, type, lengthEncoding)
        };
    }

    private static BinaryNode[] ReadNodeArray(IOReader reader, BinaryIdentifierMode mode, BinaryLengthEncoding identifierLengthEncoding, BinaryLengthEncoding lengthEncoding, List<DataIdentifier>? registry)
    {
        uint count = ReadLength(reader, lengthEncoding); var result = new BinaryNode[checked((int)count)];
        for (int i = 0; i < result.Length; i++) result[i] = ReadNode(reader, mode, identifierLengthEncoding, lengthEncoding, registry);
        return result;
    }

    private static void WriteArrayValue(IOWriter w, BinaryValueType t, object value, BinaryLengthEncoding e)
    {
        Array a = (Array)value; WriteLength(w, checked((uint)a.Length), e);
        foreach (object? x in a) switch (t)
        {
            case BinaryValueType.BooleanArray: w.Write((bool)x!); break; case BinaryValueType.ByteArray: w.Write((byte)x!); break; case BinaryValueType.SByteArray: w.Write((sbyte)x!); break;
            case BinaryValueType.Int16Array: w.Write((short)x!); break; case BinaryValueType.UInt16Array: w.Write((ushort)x!); break; case BinaryValueType.Int32Array: w.Write((int)x!); break; case BinaryValueType.UInt32Array: w.Write((uint)x!); break;
            case BinaryValueType.Int64Array: w.Write((long)x!); break; case BinaryValueType.UInt64Array: w.Write((ulong)x!); break; case BinaryValueType.SingleArray: w.Write((float)x!); break; case BinaryValueType.DoubleArray: w.Write((double)x!); break; case BinaryValueType.DecimalArray: w.Write((decimal)x!); break; case BinaryValueType.CharArray: w.Write((char)x!); break;
            case BinaryValueType.StringArray: WriteNullableString(w, (string?)x, e); break; case BinaryValueType.TimeSpanArray: w.Write(((TimeSpan)x!).Ticks); break; case BinaryValueType.DateTimeArray: w.Write(((DateTime)x!).ToBinary()); break; case BinaryValueType.GuidArray: w.Write(((Guid)x!).ToByteArray()); break; case BinaryValueType.EnumArray: WriteUtf8(w, (string)x!, e); break;
            default: throw new InvalidDataException("Unsupported array type.");
        }
    }

    private static object ReadArrayValue(IOReader r, BinaryValueType t, BinaryLengthEncoding e)
    {
        int n = checked((int)ReadLength(r, e));
        switch (t)
        {
            case BinaryValueType.BooleanArray: { var a=new bool[n]; for(int i=0;i<n;i++)a[i]=r.ReadBoolean(); return a; }
            case BinaryValueType.ByteArray: return ReadExact(r,n);
            case BinaryValueType.SByteArray: { var a=new sbyte[n]; for(int i=0;i<n;i++)a[i]=r.ReadSByte(); return a; }
            case BinaryValueType.Int16Array: { var a=new short[n]; for(int i=0;i<n;i++)a[i]=r.ReadInt16(); return a; }
            case BinaryValueType.UInt16Array: { var a=new ushort[n]; for(int i=0;i<n;i++)a[i]=r.ReadUInt16(); return a; }
            case BinaryValueType.Int32Array: { var a=new int[n]; for(int i=0;i<n;i++)a[i]=r.ReadInt32(); return a; }
            case BinaryValueType.UInt32Array: { var a=new uint[n]; for(int i=0;i<n;i++)a[i]=r.ReadUInt32(); return a; }
            case BinaryValueType.Int64Array: { var a=new long[n]; for(int i=0;i<n;i++)a[i]=r.ReadInt64(); return a; }
            case BinaryValueType.UInt64Array: { var a=new ulong[n]; for(int i=0;i<n;i++)a[i]=r.ReadUInt64(); return a; }
            case BinaryValueType.SingleArray: { var a=new float[n]; for(int i=0;i<n;i++)a[i]=r.ReadSingle(); return a; }
            case BinaryValueType.DoubleArray: { var a=new double[n]; for(int i=0;i<n;i++)a[i]=r.ReadDouble(); return a; }
            case BinaryValueType.DecimalArray: { var a=new decimal[n]; for(int i=0;i<n;i++)a[i]=r.ReadDecimal(); return a; }
            case BinaryValueType.CharArray: { var a=new char[n]; for(int i=0;i<n;i++)a[i]=r.ReadChar(); return a; }
            case BinaryValueType.StringArray: { var a=new string[n]; for(int i=0;i<n;i++)a[i]=ReadNullableString(r,e)!; return a; }
            case BinaryValueType.TimeSpanArray: { var a=new TimeSpan[n]; for(int i=0;i<n;i++)a[i]=TimeSpan.FromTicks(r.ReadInt64()); return a; }
            case BinaryValueType.DateTimeArray: { var a=new DateTime[n]; for(int i=0;i<n;i++)a[i]=DateTime.FromBinary(r.ReadInt64()); return a; }
            case BinaryValueType.GuidArray: { var a=new Guid[n]; for(int i=0;i<n;i++)a[i]=new Guid(ReadExact(r,16)); return a; }
            case BinaryValueType.EnumArray: { var a=new string[n]; for(int i=0;i<n;i++)a[i]=ReadUtf8(r,e); return a; }
            default: throw new InvalidDataException("Unsupported array type.");
        }
    }

    private static void WriteNullableString(IOWriter w, string? value, BinaryLengthEncoding e) { w.Write(value != null); if (value != null) WriteUtf8(w,value,e); }
    private static string? ReadNullableString(IOReader r, BinaryLengthEncoding e) => r.ReadBoolean() ? ReadUtf8(r,e) : null;
    private static void WriteUtf8(IOWriter w, string value, BinaryLengthEncoding e) { byte[] b=Encoding.UTF8.GetBytes(value); WriteLength(w,checked((uint)b.Length),e); w.Write(b); }
    private static string ReadUtf8(IOReader r, BinaryLengthEncoding e) => Encoding.UTF8.GetString(ReadExact(r,checked((int)ReadLength(r,e))));
    private static byte[] ReadExact(IOReader r, int n) { byte[] b=r.ReadBytes(n); if(b.Length!=n) throw new EndOfStreamException(); return b; }

    internal static void WriteLength(IOWriter w, uint v, BinaryLengthEncoding e)
    {
        switch(e) { case BinaryLengthEncoding.Byte: w.Write(checked((byte)v)); break; case BinaryLengthEncoding.UInt16: w.Write(checked((ushort)v)); break; case BinaryLengthEncoding.UInt32: w.Write(v); break; case BinaryLengthEncoding.Variable: WriteVarUInt(w,v); break; default: throw new InvalidOperationException("Unsupported length encoding."); }
    }
    internal static uint ReadLength(IOReader r, BinaryLengthEncoding e) => e switch { BinaryLengthEncoding.Byte=>r.ReadByte(), BinaryLengthEncoding.UInt16=>r.ReadUInt16(), BinaryLengthEncoding.UInt32=>r.ReadUInt32(), BinaryLengthEncoding.Variable=>ReadVarUInt(r), _=>throw new InvalidDataException("Unsupported length encoding.") };
    private static void WriteVarUInt(IOWriter w,uint v){while(v>=0x80){w.Write((byte)(v|0x80));v>>=7;}w.Write((byte)v);}
    private static uint ReadVarUInt(IOReader r){uint v=0;int s=0;for(int i=0;i<5;i++){byte b=r.ReadByte();v|=(uint)(b&0x7F)<<s;if((b&0x80)==0)return v;s+=7;}throw new InvalidDataException("Invalid variable-length integer.");}
    private static void ValidateEnum<T>(T value,string name) where T:struct,Enum { if(!Enum.IsDefined(value)) throw new InvalidDataException($"Invalid {name} value."); }
}
