using System;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Sachssoft.Documents.Formats.Ini;

/// <summary>
/// Provides INI document loading, saving, reading and writing functionality.
/// </summary>
public sealed class IniFormatter : IDocumentFormatter
{
    private IniDocument _document;
    private IniReader _reader;
    private IniWriter _writer;

    /// <summary>
    /// Initializes a new instance of the <see cref="IniFormatter"/> class.
    /// </summary>
    public IniFormatter()
    {
        IniOptions = new IniOptions();
        _document = new IniDocument();
        _document.GetOrCreateSection(string.Empty);
        _reader = new IniReader(_document, string.Empty, IniOptions);
        _writer = new IniWriter(_document, string.Empty, IniOptions);
    }

    /// <summary>
    /// Gets the format-specific INI options.
    /// </summary>
    public IniOptions IniOptions { get; }
    /// <summary>
    /// Gets the internal INI document model.
    /// </summary>
    public object Root => _document;
    /// <summary>
    /// Gets the strongly typed INI reader.
    /// </summary>
    public IniReader IniReader => _reader;
    /// <summary>
    /// Gets the strongly typed INI writer.
    /// </summary>
    public IniWriter IniWriter => _writer;
    /// <inheritdoc />
    public FormatWriterBase Writer => _writer;
    /// <inheritdoc />
    public FormatReaderBase Reader => _reader;

    /// <inheritdoc />
    public void LoadFrom(Stream stream)
    {
        ArgumentNullException.ThrowIfNull(stream);
        using var reader = new StreamReader(stream, Encoding.UTF8, true, 1024, true);
        Load(reader.ReadToEnd());
    }
    /// <inheritdoc />
    public async Task LoadFromAsync(Stream stream, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(stream);
        using var reader = new StreamReader(stream, Encoding.UTF8, true, 1024, true);
        Load(await reader.ReadToEndAsync(cancellationToken).ConfigureAwait(false));
    }
    /// <inheritdoc />
    public void SaveTo(Stream stream)
    {
        ArgumentNullException.ThrowIfNull(stream);
        using var writer = new StreamWriter(stream, new UTF8Encoding(false), 1024, true);
        WriteDocument(writer);
        writer.Flush();
    }
    /// <inheritdoc />
    public async Task SaveToAsync(Stream stream, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(stream);
        using var writer = new StreamWriter(stream, new UTF8Encoding(false), 1024, true);
        foreach (var line in GetDocumentLines()) await writer.WriteLineAsync(line.AsMemory(), cancellationToken).ConfigureAwait(false);
        await writer.FlushAsync(cancellationToken).ConfigureAwait(false);
    }

    private void Load(string text)
    {
        var document = new IniDocument();
        string sectionName = string.Empty;
        document.GetOrCreateSection(sectionName);
        using var reader = new StringReader(text);
        string? line;
        int lineNumber = 0;
        while ((line = reader.ReadLine()) != null)
        {
            lineNumber++;
            var trimmed = line.Trim();
            if (trimmed.Length == 0 || trimmed[0] is ';' or '#') continue;
            if (trimmed[0] == '[')
            {
                if (trimmed[^1] != ']') throw new InvalidDataException($"Invalid INI section declaration at line {lineNumber}.");
                sectionName = trimmed[1..^1].Trim();
                if (sectionName.Length == 0) throw new InvalidDataException($"Empty INI section name at line {lineNumber}.");
                document.GetOrCreateSection(sectionName);
                continue;
            }
            int separator = FindAssignment(trimmed);
            if (separator <= 0) throw new InvalidDataException($"Invalid INI assignment at line {lineNumber}.");
            var key = trimmed[..separator].Trim();
            if (key.Length == 0) throw new InvalidDataException($"Empty INI key at line {lineNumber}.");
            var raw = StripInlineComment(trimmed[(separator + 1)..]);
            document.GetOrCreateSection(sectionName)[key] = IniCodec.DecodeValue(raw);
        }
        SetDocument(document);
    }

    private static int FindAssignment(string line)
    {
        bool quoted = false;
        bool escape = false;
        for (int i = 0; i < line.Length; i++)
        {
            char character = line[i];
            if (escape) { escape = false; continue; }
            if (character == '\\' && quoted) { escape = true; continue; }
            if (character == '"') { quoted = !quoted; continue; }
            if (character == '=' && !quoted) return i;
        }
        return -1;
    }
    private static string StripInlineComment(string value)
    {
        bool quoted = false;
        bool escape = false;
        for (int i = 0; i < value.Length; i++)
        {
            char character = value[i];
            if (escape) { escape = false; continue; }
            if (character == '\\' && quoted) { escape = true; continue; }
            if (character == '"') { quoted = !quoted; continue; }
            if ((character == ';' || character == '#') && !quoted && (i == 0 || char.IsWhiteSpace(value[i - 1]))) return value[..i].TrimEnd();
        }
        return value.Trim();
    }
    private void WriteDocument(TextWriter writer)
    {
        foreach (var line in GetDocumentLines()) writer.WriteLine(line);
    }
    private string[] GetDocumentLines()
    {
        var lines = new System.Collections.Generic.List<string>();
        bool firstSection = true;
        foreach (var section in _document.Sections)
        {
            if (!firstSection && (section.Key.Length > 0 || section.Value.Count > 0)) lines.Add(string.Empty);
            if (section.Key.Length > 0) lines.Add("[" + section.Key + "]");
            foreach (var pair in section.Value) lines.Add(pair.Key + "=" + IniCodec.EncodeValue(pair.Value, IniOptions.ArraySeparator));
            firstSection = false;
        }
        return lines.ToArray();
    }
    private void SetDocument(IniDocument document)
    {
        var readerOptions = _reader.Options;
        var writerOptions = _writer.Options;
        var errorHandling = _reader.ConversionErrorHandling;
        _document = document;
        _reader = new IniReader(_document, string.Empty, IniOptions) { Options = readerOptions, ConversionErrorHandling = errorHandling };
        _writer = new IniWriter(_document, string.Empty, IniOptions) { Options = writerOptions };
    }
}
