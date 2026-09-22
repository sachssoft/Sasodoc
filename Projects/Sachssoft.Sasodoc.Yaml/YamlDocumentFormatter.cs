using YamlDotNet.RepresentationModel;

namespace Sachssoft.Documents.Formats.Yaml;

/// <summary>
/// Provides YAML document loading and saving functionality for Sasodoc.
/// </summary>
public class YamlDocumentFormatter : IDocumentFormatter
{
    private YamlMappingNode _root = new();
    private readonly YamlWriter _writer = new();
    private readonly YamlReader _reader = new();

    /// <summary>
    /// Initializes a new instance of the <see cref="YamlDocumentFormatter"/> class.
    /// </summary>
    public YamlDocumentFormatter() => UpdateRoot(_root);

    /// <summary>
    /// Gets or sets a value indicating whether an empty YAML document is created when an empty stream is loaded.
    /// </summary>
    public bool CreateNewIfEmpty { get; set; } = true;

    /// <summary>
    /// Gets the root mapping node of the current YAML document.
    /// </summary>
    public YamlMappingNode Root => _root;

    /// <summary>
    /// Gets the writer associated with the current YAML document.
    /// </summary>
    public YamlWriter Writer => _writer;

    /// <summary>
    /// Gets the reader associated with the current YAML document.
    /// </summary>
    public YamlReader Reader => _reader;

    FormatWriterBase IDocumentFormatter.Writer => _writer;
    FormatReaderBase IDocumentFormatter.Reader => _reader;
    object IDocumentFormatter.Root => _root;

    private void UpdateRoot(YamlMappingNode root) { _root = root; _writer.Node = root; _reader.Node = root; }

    /// <inheritdoc />
    public void LoadFrom(Stream stream) { ArgumentNullException.ThrowIfNull(stream); if (stream.CanSeek && stream.Length == 0) { if (!CreateNewIfEmpty) throw new InvalidDataException("The stream is empty."); UpdateRoot(new YamlMappingNode()); return; } if (stream.CanSeek && stream.Position == stream.Length) stream.Position = 0; using var textReader = new StreamReader(stream, leaveOpen: true); var yaml = new YamlStream(); yaml.Load(textReader); if (yaml.Documents.Count == 0) { UpdateRoot(new YamlMappingNode()); return; } if (yaml.Documents[0].RootNode is not YamlMappingNode root) throw new InvalidDataException("The YAML root must be a mapping."); UpdateRoot(root); }

    /// <inheritdoc />
    public async Task LoadFromAsync(Stream stream, CancellationToken cancellationToken = default) { ArgumentNullException.ThrowIfNull(stream); using var buffer = new MemoryStream(); await stream.CopyToAsync(buffer, cancellationToken).ConfigureAwait(false); buffer.Position = 0; LoadFrom(buffer); }

    /// <inheritdoc />
    public void SaveTo(Stream stream) { ArgumentNullException.ThrowIfNull(stream); using var textWriter = new StreamWriter(stream, leaveOpen: true); var yaml = new YamlStream(new YamlDocument(_root)); yaml.Save(textWriter, false); textWriter.Flush(); }

    /// <inheritdoc />
    public async Task SaveToAsync(Stream stream, CancellationToken cancellationToken = default) { ArgumentNullException.ThrowIfNull(stream); await using var buffer = new MemoryStream(); SaveTo(buffer); buffer.Position = 0; await buffer.CopyToAsync(stream, cancellationToken).ConfigureAwait(false); }
}
