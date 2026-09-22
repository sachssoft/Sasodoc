using System;
using System.IO;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading;
using System.Threading.Tasks;

namespace Sachssoft.Documents.Formats.Json;

/// <summary>
/// Provides a document formatter backed by a JSON object tree.
/// </summary>
public class JsonDocumentFormatter : IDocumentFormatter
{
    private JsonObject _root;
    private readonly JsonWriter _writer;
    private readonly JsonReader _reader;

    /// <summary>
    /// Initializes a new instance of the <see cref="JsonDocumentFormatter"/> class.
    /// </summary>
    public JsonDocumentFormatter()
    {
        _root = new JsonObject();
        _writer = new JsonWriter();
        _reader = new JsonReader();
        UpdateRoot(_root);
    }

    /// <summary>
    /// Gets or sets whether an empty seekable stream creates a new empty JSON document.
    /// </summary>
    public bool CreateNewIfEmpty { get; set; } = true;

    /// <summary>
    /// Gets the root JSON object.
    /// </summary>
    public JsonNode Root => _root;

    /// <summary>
    /// Gets the JSON writer associated with the document.
    /// </summary>
    public JsonWriter Writer => _writer;

    /// <summary>
    /// Gets the JSON reader associated with the document.
    /// </summary>
    public JsonReader Reader => _reader;

    FormatWriterBase IDocumentFormatter.Writer => _writer;
    FormatReaderBase IDocumentFormatter.Reader => _reader;
    object IDocumentFormatter.Root => _root;

    private void UpdateRoot(JsonObject root)
    {
        _root = root;
        _writer.Node = root;
        _reader.Node = root;
    }

    /// <inheritdoc />
    public void LoadFrom(Stream stream)
    {
        ArgumentNullException.ThrowIfNull(stream);

        if (TryHandleEmptyStream(stream)) return;
        RewindIfAtEnd(stream);

        var root = JsonNode.Parse(stream) as JsonObject ?? throw new InvalidDataException("The JSON root must be an object.");
        UpdateRoot(root);
    }

    /// <inheritdoc />
    public async Task LoadFromAsync(Stream stream, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(stream);

        if (TryHandleEmptyStream(stream)) return;
        RewindIfAtEnd(stream);

        var node = await JsonNode.ParseAsync(stream, cancellationToken: cancellationToken).ConfigureAwait(false);
        var root = node as JsonObject ?? throw new InvalidDataException("The JSON root must be an object.");
        UpdateRoot(root);
    }

    /// <inheritdoc />
    public void SaveTo(Stream stream) => SaveTo(stream, new JsonWriterOptions { Indented = true });

    /// <summary>
    /// Saves the JSON document to the specified stream using the specified writer options.
    /// </summary>
    /// <param name="stream">The stream to which the JSON document is written.</param>
    /// <param name="options">The JSON writer options.</param>
    public void SaveTo(Stream stream, JsonWriterOptions options)
    {
        ArgumentNullException.ThrowIfNull(stream);

        using var writer = new Utf8JsonWriter(stream, options);
        _root.WriteTo(writer);
        writer.Flush();
    }

    /// <inheritdoc />
    public Task SaveToAsync(Stream stream, CancellationToken cancellationToken = default) => SaveToAsync(stream, new JsonSerializerOptions { WriteIndented = true }, cancellationToken);

    /// <summary>
    /// Asynchronously saves the JSON document to the specified stream.
    /// </summary>
    /// <param name="stream">The stream to which the JSON document is written.</param>
    /// <param name="options">The JSON serializer options.</param>
    /// <param name="cancellationToken">A token that can be used to cancel the asynchronous operation.</param>
    /// <returns>A task that represents the asynchronous save operation.</returns>
    public async Task SaveToAsync(Stream stream, JsonSerializerOptions options, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(stream);
        ArgumentNullException.ThrowIfNull(options);

        await using var buffer = new MemoryStream();
        using (var writer = new Utf8JsonWriter(buffer, new JsonWriterOptions { Indented = options.WriteIndented }))
        {
            _root.WriteTo(writer, options);
            writer.Flush();
        }

        buffer.Position = 0;
        await buffer.CopyToAsync(stream, cancellationToken).ConfigureAwait(false);
    }

    private bool TryHandleEmptyStream(Stream stream)
    {
        if (!stream.CanSeek || stream.Length != 0) return false;

        if (!CreateNewIfEmpty)
            throw new InvalidDataException("The stream is empty.");

        UpdateRoot(new JsonObject());
        return true;
    }

    private static void RewindIfAtEnd(Stream stream)
    {
        if (stream.CanSeek && stream.Position == stream.Length)
            stream.Position = 0;
    }
}
