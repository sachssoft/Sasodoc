using System;
using System.IO;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace Sachssoft.Sasodoc.Formats.Json
{
    public class JsonDocumentFormatter : IDocumentFormatter
    {
        private JsonObject _root;
        private readonly JsonWriter _writer;
        private readonly JsonReader _reader;

        public JsonDocumentFormatter()
        {
            _root = new JsonObject();
            _writer = new JsonWriter();
            _reader = new JsonReader();
            _writer.Node = _root;
            _reader.Node = _root;
        }

        public bool CreateNewIfEmpty { get; set; } = true;

        public JsonNode Root => _root;

        public JsonWriter Writer => _writer;

        public JsonReader Reader => _reader;

        FormatWriterBase IDocumentFormatter.Writer => _writer;

        FormatReaderBase IDocumentFormatter.Reader => _reader;

        object? IDocumentFormatter.Root => _root;

        private void UpdateRoot(JsonObject root)
        {
            _root = root;
            _writer.Node = _root;
            _reader.Node = _root;
        }


        public void LoadFrom(Stream stream)
        {
            if (stream == null)
                throw new ArgumentNullException(nameof(stream));

            // Prüfen, ob Stream leer ist
            if (stream.CanSeek && stream.Length == 0)
            {
                if (CreateNewIfEmpty)
                {
                    UpdateRoot(new JsonObject());
                    return;
                }
                else
                {
                    throw new InvalidDataException("Stream ist leer.");
                }
            }

            if (stream.CanSeek && stream.Position == stream.Length)
                stream.Position = 0; // zurückspulen

            // JSON direkt aus Stream parsen
            var root = JsonNode.Parse(stream) as JsonObject
                       ?? throw new InvalidDataException("Der JSON-Inhalt ist kein JsonObject.");

            UpdateRoot(root);
        }

        public void SaveTo(Stream stream) => SaveTo(stream, new JsonWriterOptions()
        {
            Indented = true
        });

        public void SaveTo(Stream stream, JsonWriterOptions options)
        {
            // Falls du JSON formatiert haben willst:
            using var writer = new Utf8JsonWriter(stream, options);
            _root.WriteTo(writer);
        }
    }
}
