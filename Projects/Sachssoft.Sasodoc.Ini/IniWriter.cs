using System;
using System.Globalization;

namespace Sachssoft.Documents.Formats.Ini;

/// <summary>
/// Writes document data to an INI document.
/// </summary>
public class IniWriter : FormatWriter<IniWriter>
{
    private readonly IniDocument _document;
    private readonly string _section;

    /// <summary>
    /// Initializes a new instance of the <see cref="IniWriter"/> class.
    /// </summary>
    public IniWriter() : this(new IniDocument(), string.Empty, new IniOptions()) { }

    internal IniWriter(IniDocument document, string section, IniOptions iniOptions)
    {
        _document = document ?? throw new ArgumentNullException(nameof(document));
        _section = section ?? throw new ArgumentNullException(nameof(section));
        IniOptions = iniOptions ?? throw new ArgumentNullException(nameof(iniOptions));
        _document.GetOrCreateSection(_section);
    }

    /// <summary>
    /// Gets the format-specific INI options.
    /// </summary>
    public IniOptions IniOptions { get; }

    /// <inheritdoc />
    public override IniWriter CreateWriter() => new(_document, _section, IniOptions) { Options = Options };

    private string GetKey(DataIdentifier identifier) => ConvertPropertyIdentifier(identifier).Value;
    private string GetFieldName(string name) => ConvertFieldName(name);
    private string GetChildSection(DataIdentifier identifier) => string.IsNullOrEmpty(_section) ? GetKey(identifier) : _section + IniOptions.SectionSeparator + GetKey(identifier);
    private string GetIndexedSection(DataIdentifier identifier, int index) => GetChildSection(identifier) + IniOptions.SectionSeparator + index.ToString(CultureInfo.InvariantCulture);
    private void SetValue(DataIdentifier identifier, string? value)
    {
        var section = _document.GetOrCreateSection(_section);
        var key = GetKey(identifier);
        if (value == null) section.Remove(key);
        else section[key] = value;
    }
    private void WritePrimitiveArray<T>(DataIdentifier identifier, T[]? values, Func<T, string> converter)
    {
        if (values == null) return;
        var converted = new string[values.Length];
        for (int i = 0; i < values.Length; i++) converted[i] = converter(values[i]);
        SetValue(identifier, IniCodec.EncodeArray(converted, IniOptions.ArraySeparator));
    }
    private IniWriter CreateChildWriter(string section)
    {
        _document.RemoveSectionTree(section, IniOptions.SectionSeparator);
        return new IniWriter(_document, section, IniOptions) { Options = Options };
    }

    /// <inheritdoc />
    public override void WriteBoolean(DataIdentifier identifier, bool value) => SetValue(identifier, value ? "true" : "false");
    /// <inheritdoc />
    public override void WriteByte(DataIdentifier identifier, byte value) => SetValue(identifier, value.ToString(CultureInfo.InvariantCulture));
    /// <inheritdoc />
    public override void WriteSByte(DataIdentifier identifier, sbyte value) => SetValue(identifier, value.ToString(CultureInfo.InvariantCulture));
    /// <inheritdoc />
    public override void WriteInt16(DataIdentifier identifier, short value) => SetValue(identifier, value.ToString(CultureInfo.InvariantCulture));
    /// <inheritdoc />
    public override void WriteUInt16(DataIdentifier identifier, ushort value) => SetValue(identifier, value.ToString(CultureInfo.InvariantCulture));
    /// <inheritdoc />
    public override void WriteInt32(DataIdentifier identifier, int value) => SetValue(identifier, value.ToString(CultureInfo.InvariantCulture));
    /// <inheritdoc />
    public override void WriteUInt32(DataIdentifier identifier, uint value) => SetValue(identifier, value.ToString(CultureInfo.InvariantCulture));
    /// <inheritdoc />
    public override void WriteInt64(DataIdentifier identifier, long value) => SetValue(identifier, value.ToString(CultureInfo.InvariantCulture));
    /// <inheritdoc />
    public override void WriteUInt64(DataIdentifier identifier, ulong value) => SetValue(identifier, value.ToString(CultureInfo.InvariantCulture));
    /// <inheritdoc />
    public override void WriteSingle(DataIdentifier identifier, float value) => SetValue(identifier, value.ToString("R", CultureInfo.InvariantCulture));
    /// <inheritdoc />
    public override void WriteDouble(DataIdentifier identifier, double value) => SetValue(identifier, value.ToString("R", CultureInfo.InvariantCulture));
    /// <inheritdoc />
    public override void WriteDecimal(DataIdentifier identifier, decimal value) => SetValue(identifier, value.ToString(CultureInfo.InvariantCulture));
    /// <inheritdoc />
    public override void WriteChar(DataIdentifier identifier, char value) => SetValue(identifier, value.ToString());
    /// <inheritdoc />
    public override void WriteString(DataIdentifier identifier, string? value) => SetValue(identifier, value);
    /// <inheritdoc />
    public override void WriteTimeSpan(DataIdentifier identifier, TimeSpan value) => SetValue(identifier, value.Ticks.ToString(CultureInfo.InvariantCulture));
    /// <inheritdoc />
    public override void WriteDateTime(DataIdentifier identifier, DateTime value) => SetValue(identifier, value.ToString("O", CultureInfo.InvariantCulture));
    /// <inheritdoc />
    public override void WriteGuid(DataIdentifier identifier, Guid value) => SetValue(identifier, value.ToString("D"));
    /// <inheritdoc />
    public override void WriteEnum<TEnum>(DataIdentifier identifier, TEnum value) => SetValue(identifier, GetFieldName(value.ToString()));

    /// <inheritdoc />
    public override void WriteObject(DataIdentifier identifier, IObjectWriter<IniWriter>? value, Action<IniWriter>? writerBefore = null)
    {
        if (value == null) return;
        var writer = CreateChildWriter(GetChildSection(identifier));
        writerBefore?.Invoke(writer);
        value.Write(writer);
    }
    /// <inheritdoc />
    public override void Write(DataIdentifier identifier, IniWriter? value)
    {
        if (value == null) return;
        var target = GetChildSection(identifier);
        _document.RemoveSectionTree(target, IniOptions.SectionSeparator);
        CopySectionTree(value._section, target, value._document);
    }
    private void CopySectionTree(string source, string target, IniDocument sourceDocument)
    {
        string prefix = string.IsNullOrEmpty(source) ? string.Empty : source + IniOptions.SectionSeparator;
        foreach (var pair in sourceDocument.Sections)
        {
            if (!string.Equals(pair.Key, source, StringComparison.Ordinal) && !pair.Key.StartsWith(prefix, StringComparison.Ordinal)) continue;
            string suffix = string.Equals(pair.Key, source, StringComparison.Ordinal) ? string.Empty : pair.Key[prefix.Length..];
            string targetName = string.IsNullOrEmpty(suffix) ? target : target + IniOptions.SectionSeparator + suffix;
            var section = _document.GetOrCreateSection(targetName);
            foreach (var value in pair.Value) section[value.Key] = value.Value;
        }
    }

    /// <inheritdoc />
    public override void WriteBooleanArray(DataIdentifier identifier, bool[]? values) => WritePrimitiveArray(identifier, values, static value => value ? "true" : "false");
    /// <inheritdoc />
    public override void WriteByteArray(DataIdentifier identifier, byte[]? values) => WritePrimitiveArray(identifier, values, static value => value.ToString(CultureInfo.InvariantCulture));
    /// <inheritdoc />
    public override void WriteSByteArray(DataIdentifier identifier, sbyte[]? values) => WritePrimitiveArray(identifier, values, static value => value.ToString(CultureInfo.InvariantCulture));
    /// <inheritdoc />
    public override void WriteInt16Array(DataIdentifier identifier, short[]? values) => WritePrimitiveArray(identifier, values, static value => value.ToString(CultureInfo.InvariantCulture));
    /// <inheritdoc />
    public override void WriteUInt16Array(DataIdentifier identifier, ushort[]? values) => WritePrimitiveArray(identifier, values, static value => value.ToString(CultureInfo.InvariantCulture));
    /// <inheritdoc />
    public override void WriteInt32Array(DataIdentifier identifier, int[]? values) => WritePrimitiveArray(identifier, values, static value => value.ToString(CultureInfo.InvariantCulture));
    /// <inheritdoc />
    public override void WriteUInt32Array(DataIdentifier identifier, uint[]? values) => WritePrimitiveArray(identifier, values, static value => value.ToString(CultureInfo.InvariantCulture));
    /// <inheritdoc />
    public override void WriteInt64Array(DataIdentifier identifier, long[]? values) => WritePrimitiveArray(identifier, values, static value => value.ToString(CultureInfo.InvariantCulture));
    /// <inheritdoc />
    public override void WriteUInt64Array(DataIdentifier identifier, ulong[]? values) => WritePrimitiveArray(identifier, values, static value => value.ToString(CultureInfo.InvariantCulture));
    /// <inheritdoc />
    public override void WriteSingleArray(DataIdentifier identifier, float[]? values) => WritePrimitiveArray(identifier, values, static value => value.ToString("R", CultureInfo.InvariantCulture));
    /// <inheritdoc />
    public override void WriteDoubleArray(DataIdentifier identifier, double[]? values) => WritePrimitiveArray(identifier, values, static value => value.ToString("R", CultureInfo.InvariantCulture));
    /// <inheritdoc />
    public override void WriteDecimalArray(DataIdentifier identifier, decimal[]? values) => WritePrimitiveArray(identifier, values, static value => value.ToString(CultureInfo.InvariantCulture));
    /// <inheritdoc />
    public override void WriteCharArray(DataIdentifier identifier, char[]? values) => WritePrimitiveArray(identifier, values, static value => value.ToString());
    /// <inheritdoc />
    public override void WriteStringArray(DataIdentifier identifier, string[]? values) => WritePrimitiveArray(identifier, values, static value => value);
    /// <inheritdoc />
    public override void WriteTimeSpanArray(DataIdentifier identifier, TimeSpan[]? values) => WritePrimitiveArray(identifier, values, static value => value.Ticks.ToString(CultureInfo.InvariantCulture));
    /// <inheritdoc />
    public override void WriteDateTimeArray(DataIdentifier identifier, DateTime[]? values) => WritePrimitiveArray(identifier, values, static value => value.ToString("O", CultureInfo.InvariantCulture));
    /// <inheritdoc />
    public override void WriteGuidArray(DataIdentifier identifier, Guid[]? values) => WritePrimitiveArray(identifier, values, static value => value.ToString("D"));
    /// <inheritdoc />
    public override void WriteEnumArray<TEnum>(DataIdentifier identifier, TEnum[]? values) => WritePrimitiveArray(identifier, values, value => GetFieldName(value.ToString()));

    /// <inheritdoc />
    public override void WriteObjectArray(DataIdentifier identifier, IObjectWriter<IniWriter>[]? values, Action<IniWriter>? writerBefore = null)
    {
        if (values == null) return;
        var root = GetChildSection(identifier);
        _document.RemoveSectionTree(root, IniOptions.SectionSeparator);
        for (int i = 0; i < values.Length; i++)
        {
            var writer = new IniWriter(_document, GetIndexedSection(identifier, i), IniOptions) { Options = Options };
            writerBefore?.Invoke(writer);
            values[i].Write(writer);
        }
    }
    /// <inheritdoc />
    public override void WriteArray(DataIdentifier identifier, IniWriter[]? values)
    {
        if (values == null) return;
        var root = GetChildSection(identifier);
        _document.RemoveSectionTree(root, IniOptions.SectionSeparator);
        for (int i = 0; i < values.Length; i++) CopySectionTree(values[i]._section, GetIndexedSection(identifier, i), values[i]._document);
    }
}
