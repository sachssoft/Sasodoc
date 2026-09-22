using System;
using System.Collections.Generic;
using System.Globalization;

namespace Sachssoft.Documents.Formats.Ini;

/// <summary>
/// Reads document data from an INI document.
/// </summary>
public class IniReader : FormatReader<IniReader>
{
    private readonly IniDocument _document;
    private readonly string _section;

    /// <summary>
    /// Initializes a new instance of the <see cref="IniReader"/> class.
    /// </summary>
    public IniReader() : this(new IniDocument(), string.Empty, new IniOptions()) { }

    internal IniReader(IniDocument document, string section, IniOptions iniOptions)
    {
        _document = document ?? throw new ArgumentNullException(nameof(document));
        _section = section ?? throw new ArgumentNullException(nameof(section));
        IniOptions = iniOptions ?? throw new ArgumentNullException(nameof(iniOptions));
    }

    /// <summary>
    /// Gets the format-specific INI options.
    /// </summary>
    public IniOptions IniOptions { get; }

    /// <summary>
    /// Gets or sets how failed value conversions are handled.
    /// </summary>
    public ConversionErrorHandling ConversionErrorHandling { get; set; }

    /// <inheritdoc />
    public override IniReader CreateReader() => new(_document, _section, IniOptions) { Options = Options, ConversionErrorHandling = ConversionErrorHandling };

    private string GetKey(DataIdentifier identifier) => ConvertPropertyIdentifier(identifier).Value;
    private string GetChildSection(DataIdentifier identifier) => string.IsNullOrEmpty(_section) ? GetKey(identifier) : _section + IniOptions.SectionSeparator + GetKey(identifier);
    private string GetIndexedSection(DataIdentifier identifier, int index) => GetChildSection(identifier) + IniOptions.SectionSeparator + index.ToString(CultureInfo.InvariantCulture);
    private bool TryGetRawValue(DataIdentifier identifier, out string value)
    {
        if (_document.TryGetSection(_section, out var section) && section != null && section.TryGetValue(GetKey(identifier), out var result)) { value = result; return true; }
        value = string.Empty;
        return false;
    }
    private T ReadValue<T>(DataIdentifier identifier, T fallback, TryParseCallback<T> parser)
    {
        if (TryGetRawValue(identifier, out var raw) && parser(raw, out var value)) return value;
        return ConversionErrorHandling == ConversionErrorHandling.Default ? default! : fallback;
    }
    private T[] ReadArrayValues<T>(DataIdentifier identifier, T[]? fallback, TryParseCallback<T> parser)
    {
        fallback ??= Array.Empty<T>();
        if (!TryGetRawValue(identifier, out var raw)) return fallback;
        string[] parts;
        try { parts = IniCodec.DecodeArray(raw, IniOptions.ArraySeparator); }
        catch (FormatException) { return fallback; }
        var result = new List<T>(parts.Length);
        foreach (var part in parts)
        {
            if (parser(part, out var value)) { result.Add(value); continue; }
            switch (ConversionErrorHandling)
            {
                case ConversionErrorHandling.Ignore: continue;
                case ConversionErrorHandling.Cancel: return fallback;
                case ConversionErrorHandling.Replace: result.Add(default!); continue;
                case ConversionErrorHandling.Default: result.Add(default!); continue;
                default: return fallback;
            }
        }
        return result.ToArray();
    }
    private delegate bool TryParseCallback<T>(string value, out T result);

    /// <inheritdoc />
    public override bool Contains(DataIdentifier identifier) => TryGetRawValue(identifier, out _) || _document.TryGetSection(GetChildSection(identifier), out _);
    /// <inheritdoc />
    public override bool ReadBoolean(DataIdentifier identifier, bool fallback = default) => ReadValue(identifier, fallback, bool.TryParse);
    /// <inheritdoc />
    public override byte ReadByte(DataIdentifier identifier, byte fallback = default) => ReadValue(identifier, fallback, static (string value, out byte result) => byte.TryParse(value, NumberStyles.Integer, CultureInfo.InvariantCulture, out result));
    /// <inheritdoc />
    public override sbyte ReadSByte(DataIdentifier identifier, sbyte fallback = default) => ReadValue(identifier, fallback, static (string value, out sbyte result) => sbyte.TryParse(value, NumberStyles.Integer, CultureInfo.InvariantCulture, out result));
    /// <inheritdoc />
    public override short ReadInt16(DataIdentifier identifier, short fallback = default) => ReadValue(identifier, fallback, static (string value, out short result) => short.TryParse(value, NumberStyles.Integer, CultureInfo.InvariantCulture, out result));
    /// <inheritdoc />
    public override ushort ReadUInt16(DataIdentifier identifier, ushort fallback = default) => ReadValue(identifier, fallback, static (string value, out ushort result) => ushort.TryParse(value, NumberStyles.Integer, CultureInfo.InvariantCulture, out result));
    /// <inheritdoc />
    public override int ReadInt32(DataIdentifier identifier, int fallback = default) => ReadValue(identifier, fallback, static (string value, out int result) => int.TryParse(value, NumberStyles.Integer, CultureInfo.InvariantCulture, out result));
    /// <inheritdoc />
    public override uint ReadUInt32(DataIdentifier identifier, uint fallback = default) => ReadValue(identifier, fallback, static (string value, out uint result) => uint.TryParse(value, NumberStyles.Integer, CultureInfo.InvariantCulture, out result));
    /// <inheritdoc />
    public override long ReadInt64(DataIdentifier identifier, long fallback = default) => ReadValue(identifier, fallback, static (string value, out long result) => long.TryParse(value, NumberStyles.Integer, CultureInfo.InvariantCulture, out result));
    /// <inheritdoc />
    public override ulong ReadUInt64(DataIdentifier identifier, ulong fallback = default) => ReadValue(identifier, fallback, static (string value, out ulong result) => ulong.TryParse(value, NumberStyles.Integer, CultureInfo.InvariantCulture, out result));
    /// <inheritdoc />
    public override float ReadSingle(DataIdentifier identifier, float fallback = default) => ReadValue(identifier, fallback, static (string value, out float result) => float.TryParse(value, NumberStyles.Float, CultureInfo.InvariantCulture, out result));
    /// <inheritdoc />
    public override double ReadDouble(DataIdentifier identifier, double fallback = default) => ReadValue(identifier, fallback, static (string value, out double result) => double.TryParse(value, NumberStyles.Float, CultureInfo.InvariantCulture, out result));
    /// <inheritdoc />
    public override decimal ReadDecimal(DataIdentifier identifier, decimal fallback = default) => ReadValue(identifier, fallback, static (string value, out decimal result) => decimal.TryParse(value, NumberStyles.Number, CultureInfo.InvariantCulture, out result));
    /// <inheritdoc />
    public override char ReadChar(DataIdentifier identifier, char fallback = default) => ReadValue(identifier, fallback, static (string value, out char result) => { if (value.Length == 1) { result = value[0]; return true; } result = default; return false; });
    /// <inheritdoc />
    public override string? ReadString(DataIdentifier identifier, string? fallback = default) => TryGetRawValue(identifier, out var value) ? value : fallback;
    /// <inheritdoc />
    public override TimeSpan ReadTimeSpan(DataIdentifier identifier, TimeSpan fallback = default) => ReadValue(identifier, fallback, static (string value, out TimeSpan result) => { if (long.TryParse(value, NumberStyles.Integer, CultureInfo.InvariantCulture, out var ticks)) { result = new TimeSpan(ticks); return true; } result = default; return false; });
    /// <inheritdoc />
    public override DateTime ReadDateTime(DataIdentifier identifier, DateTime fallback = default) => ReadValue(identifier, fallback, static (string value, out DateTime result) => DateTime.TryParseExact(value, "O", CultureInfo.InvariantCulture, DateTimeStyles.RoundtripKind, out result));
    /// <inheritdoc />
    public override Guid ReadGuid(DataIdentifier identifier, Guid fallback = default) => ReadValue(identifier, fallback, static (string value, out Guid result) => Guid.TryParseExact(value, "D", out result));
    /// <inheritdoc />
    public override TEnum ReadEnum<TEnum>(DataIdentifier identifier, TEnum fallback = default)
    {
        if (!TryGetRawValue(identifier, out var value)) return fallback;
        foreach (var enumValue in Enum.GetValues<TEnum>()) if (string.Equals(ConvertFieldName(enumValue.ToString()), value, StringComparison.Ordinal)) return enumValue;
        return ConversionErrorHandling == ConversionErrorHandling.Default ? default : fallback;
    }

    /// <inheritdoc />
    public override IObjectReader<IniReader>? ReadObject(DataIdentifier identifier, ObjectReaderGeneratorCallback<IniReader> generator, IObjectReader<IniReader>? fallback = default)
    {
        ArgumentNullException.ThrowIfNull(generator);
        var section = GetChildSection(identifier);
        if (!_document.TryGetSection(section, out _)) return fallback;
        var reader = new IniReader(_document, section, IniOptions) { Options = Options, ConversionErrorHandling = ConversionErrorHandling };
        var instance = generator(reader);
        if (instance == null) return fallback;
        instance.Read(reader);
        return instance;
    }
    /// <inheritdoc />
    public override IniReader Read(DataIdentifier identifier) => new(_document, GetChildSection(identifier), IniOptions) { Options = Options, ConversionErrorHandling = ConversionErrorHandling };

    /// <inheritdoc />
    public override bool[] ReadBooleanArray(DataIdentifier identifier, bool[]? fallback = default) => ReadArrayValues(identifier, fallback, bool.TryParse);
    /// <inheritdoc />
    public override byte[] ReadByteArray(DataIdentifier identifier, byte[]? fallback = default) => ReadArrayValues(identifier, fallback, static (string value, out byte result) => byte.TryParse(value, NumberStyles.Integer, CultureInfo.InvariantCulture, out result));
    /// <inheritdoc />
    public override sbyte[] ReadSByteArray(DataIdentifier identifier, sbyte[]? fallback = default) => ReadArrayValues(identifier, fallback, static (string value, out sbyte result) => sbyte.TryParse(value, NumberStyles.Integer, CultureInfo.InvariantCulture, out result));
    /// <inheritdoc />
    public override short[] ReadInt16Array(DataIdentifier identifier, short[]? fallback = default) => ReadArrayValues(identifier, fallback, static (string value, out short result) => short.TryParse(value, NumberStyles.Integer, CultureInfo.InvariantCulture, out result));
    /// <inheritdoc />
    public override ushort[] ReadUInt16Array(DataIdentifier identifier, ushort[]? fallback = default) => ReadArrayValues(identifier, fallback, static (string value, out ushort result) => ushort.TryParse(value, NumberStyles.Integer, CultureInfo.InvariantCulture, out result));
    /// <inheritdoc />
    public override int[] ReadInt32Array(DataIdentifier identifier, int[]? fallback = default) => ReadArrayValues(identifier, fallback, static (string value, out int result) => int.TryParse(value, NumberStyles.Integer, CultureInfo.InvariantCulture, out result));
    /// <inheritdoc />
    public override uint[] ReadUInt32Array(DataIdentifier identifier, uint[]? fallback = default) => ReadArrayValues(identifier, fallback, static (string value, out uint result) => uint.TryParse(value, NumberStyles.Integer, CultureInfo.InvariantCulture, out result));
    /// <inheritdoc />
    public override long[] ReadInt64Array(DataIdentifier identifier, long[]? fallback = default) => ReadArrayValues(identifier, fallback, static (string value, out long result) => long.TryParse(value, NumberStyles.Integer, CultureInfo.InvariantCulture, out result));
    /// <inheritdoc />
    public override ulong[] ReadUInt64Array(DataIdentifier identifier, ulong[]? fallback = default) => ReadArrayValues(identifier, fallback, static (string value, out ulong result) => ulong.TryParse(value, NumberStyles.Integer, CultureInfo.InvariantCulture, out result));
    /// <inheritdoc />
    public override float[] ReadSingleArray(DataIdentifier identifier, float[]? fallback = default) => ReadArrayValues(identifier, fallback, static (string value, out float result) => float.TryParse(value, NumberStyles.Float, CultureInfo.InvariantCulture, out result));
    /// <inheritdoc />
    public override double[] ReadDoubleArray(DataIdentifier identifier, double[]? fallback = default) => ReadArrayValues(identifier, fallback, static (string value, out double result) => double.TryParse(value, NumberStyles.Float, CultureInfo.InvariantCulture, out result));
    /// <inheritdoc />
    public override decimal[] ReadDecimalArray(DataIdentifier identifier, decimal[]? fallback = default) => ReadArrayValues(identifier, fallback, static (string value, out decimal result) => decimal.TryParse(value, NumberStyles.Number, CultureInfo.InvariantCulture, out result));
    /// <inheritdoc />
    public override char[] ReadCharArray(DataIdentifier identifier, char[]? fallback = default) => ReadArrayValues(identifier, fallback, static (string value, out char result) => { if (value.Length == 1) { result = value[0]; return true; } result = default; return false; });
    /// <inheritdoc />
    public override string[] ReadStringArray(DataIdentifier identifier, string[]? fallback = default)
    {
        fallback ??= Array.Empty<string>();
        if (!TryGetRawValue(identifier, out var raw)) return fallback;
        try { return IniCodec.DecodeArray(raw, IniOptions.ArraySeparator); }
        catch (FormatException) { return fallback; }
    }
    /// <inheritdoc />
    public override TimeSpan[] ReadTimeSpanArray(DataIdentifier identifier, TimeSpan[]? fallback = default) => ReadArrayValues(identifier, fallback, static (string value, out TimeSpan result) => { if (long.TryParse(value, NumberStyles.Integer, CultureInfo.InvariantCulture, out var ticks)) { result = new TimeSpan(ticks); return true; } result = default; return false; });
    /// <inheritdoc />
    public override DateTime[] ReadDateTimeArray(DataIdentifier identifier, DateTime[]? fallback = default) => ReadArrayValues(identifier, fallback, static (string value, out DateTime result) => DateTime.TryParseExact(value, "O", CultureInfo.InvariantCulture, DateTimeStyles.RoundtripKind, out result));
    /// <inheritdoc />
    public override Guid[] ReadGuidArray(DataIdentifier identifier, Guid[]? fallback = default) => ReadArrayValues(identifier, fallback, static (string value, out Guid result) => Guid.TryParseExact(value, "D", out result));
    /// <inheritdoc />
    public override TEnum[] ReadEnumArray<TEnum>(DataIdentifier identifier, TEnum[]? fallback = default)
    {
        fallback ??= Array.Empty<TEnum>();
        if (!TryGetRawValue(identifier, out var raw)) return fallback;
        string[] parts;
        try { parts = IniCodec.DecodeArray(raw, IniOptions.ArraySeparator); } catch (FormatException) { return fallback; }
        var result = new List<TEnum>(parts.Length);
        foreach (var part in parts)
        {
            bool found = false;
            foreach (var enumValue in Enum.GetValues<TEnum>())
            {
                if (!string.Equals(ConvertFieldName(enumValue.ToString()), part, StringComparison.Ordinal)) continue;
                result.Add(enumValue); found = true; break;
            }
            if (found) continue;
            if (ConversionErrorHandling == ConversionErrorHandling.Cancel) return fallback;
            if (ConversionErrorHandling is ConversionErrorHandling.Default or ConversionErrorHandling.Replace) result.Add(default);
        }
        return result.ToArray();
    }

    /// <inheritdoc />
    public override IObjectReader<IniReader>[] ReadObjectArray(DataIdentifier identifier, ObjectReaderGeneratorCallback<IniReader> generator, IObjectReader<IniReader>[]? fallback = default)
    {
        ArgumentNullException.ThrowIfNull(generator);
        fallback ??= Array.Empty<IObjectReader<IniReader>>();
        var result = new List<IObjectReader<IniReader>>();
        for (int i = 0; ; i++)
        {
            var section = GetIndexedSection(identifier, i);
            if (!_document.TryGetSection(section, out _)) break;
            var reader = new IniReader(_document, section, IniOptions) { Options = Options, ConversionErrorHandling = ConversionErrorHandling };
            var instance = generator(reader);
            if (instance == null) { if (ConversionErrorHandling == ConversionErrorHandling.Cancel) return fallback; continue; }
            instance.Read(reader); result.Add(instance);
        }
        return result.ToArray();
    }
    /// <inheritdoc />
    public override IniReader[] ReadArray(DataIdentifier identifier)
    {
        var result = new List<IniReader>();
        for (int i = 0; ; i++)
        {
            var section = GetIndexedSection(identifier, i);
            if (!_document.TryGetSection(section, out _)) break;
            result.Add(new IniReader(_document, section, IniOptions) { Options = Options, ConversionErrorHandling = ConversionErrorHandling });
        }
        return result.ToArray();
    }
}
