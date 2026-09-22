using System;
using System.IO;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading;
using System.Threading.Tasks;

namespace Sachssoft.Documents.Formats.Json;

/// <summary>
/// Provides AOT-friendly helpers for loading and saving object format readers and writers as JSON.
/// </summary>
public static class JsonSerializer
{
    /// <summary>
    /// Loads JSON data from a stream into the specified object reader.
    /// </summary>
    /// <param name="reader">The object reader that receives the deserialized data.</param>
    /// <param name="stream">The stream containing the JSON document.</param>
    public static void Load(IObjectFormatReader<JsonReader> reader, Stream stream)
    {
        ArgumentNullException.ThrowIfNull(reader);
        ArgumentNullException.ThrowIfNull(stream);

        var root = JsonNode.Parse(stream) as JsonObject ?? throw new InvalidDataException("The JSON root must be an object.");
        var jsonReader = new JsonReader { Node = root };
        reader.Read(jsonReader);
    }

    /// <summary>
    /// Asynchronously loads JSON data from a stream into the specified object reader.
    /// </summary>
    /// <param name="reader">The object reader that receives the deserialized data.</param>
    /// <param name="stream">The stream containing the JSON document.</param>
    /// <param name="cancellationToken">A token that can be used to cancel the asynchronous operation.</param>
    /// <returns>A task that represents the asynchronous load operation.</returns>
    public static async Task LoadAsync(IObjectFormatReader<JsonReader> reader, Stream stream, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(reader);
        ArgumentNullException.ThrowIfNull(stream);

        var node = await JsonNode.ParseAsync(stream, cancellationToken: cancellationToken).ConfigureAwait(false);
        var root = node as JsonObject ?? throw new InvalidDataException("The JSON root must be an object.");
        var jsonReader = new JsonReader { Node = root };
        reader.Read(jsonReader);
    }

    /// <summary>
    /// Saves the specified object writer as JSON to a stream.
    /// </summary>
    /// <param name="writer">The object writer to serialize.</param>
    /// <param name="stream">The stream to which the JSON document is written.</param>
    /// <param name="options">Optional JSON serializer options.</param>
    public static void Save(IObjectFormatWriter<JsonWriter> writer, Stream stream, JsonSerializerOptions? options = null)
    {
        ArgumentNullException.ThrowIfNull(writer);
        ArgumentNullException.ThrowIfNull(stream);

        var jsonWriter = new JsonWriter();
        writer.Write(jsonWriter);
        using var utf8Writer = new Utf8JsonWriter(stream, new JsonWriterOptions { Indented = options?.WriteIndented ?? false });
        jsonWriter.Node.WriteTo(utf8Writer, options);
        utf8Writer.Flush();
    }

    /// <summary>
    /// Asynchronously saves the specified object writer as JSON to a stream.
    /// </summary>
    /// <param name="writer">The object writer to serialize.</param>
    /// <param name="stream">The stream to which the JSON document is written.</param>
    /// <param name="options">Optional JSON serializer options.</param>
    /// <param name="cancellationToken">A token that can be used to cancel the asynchronous operation.</param>
    /// <returns>A task that represents the asynchronous save operation.</returns>
    public static async Task SaveAsync(IObjectFormatWriter<JsonWriter> writer, Stream stream, JsonSerializerOptions? options = null, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(writer);
        ArgumentNullException.ThrowIfNull(stream);

        var jsonWriter = new JsonWriter();
        writer.Write(jsonWriter);
        await using var buffer = new MemoryStream();
        using (var utf8Writer = new Utf8JsonWriter(buffer, new JsonWriterOptions { Indented = options?.WriteIndented ?? false }))
        {
            jsonWriter.Node.WriteTo(utf8Writer, options);
            utf8Writer.Flush();
        }

        buffer.Position = 0;
        await buffer.CopyToAsync(stream, cancellationToken).ConfigureAwait(false);
    }
}
