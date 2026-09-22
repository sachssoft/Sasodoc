using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace Sachssoft.Documents.Formats.Xml;

/// <summary>
/// Provides XML document loading, saving, reading and writing functionality.
/// </summary>
public sealed class XmlFormatter : IDocumentFormatter
{
    private XDocument _document;
    private XmlReader _reader;
    private XmlWriter _writer;

    /// <summary>
    /// Initializes a new instance of the <see cref="XmlFormatter"/> class.
    /// </summary>
    public XmlFormatter()
    {
        _document = new XDocument(new XElement("document"));
        _reader = new XmlReader(_document.Root!);
        _writer = new XmlWriter(_document.Root!);
    }

    /// <summary>
    /// Gets the XML document.
    /// </summary>
    public object Root => _document;

    /// <summary>
    /// Gets the strongly typed XML reader.
    /// </summary>
    public XmlReader XmlReader => _reader;

    /// <summary>
    /// Gets the strongly typed XML writer.
    /// </summary>
    public XmlWriter XmlWriter => _writer;

    /// <inheritdoc />
    public FormatWriterBase Writer => _writer;

    /// <inheritdoc />
    public FormatReaderBase Reader => _reader;

    /// <inheritdoc />
    public void LoadFrom(Stream stream)
    {
        ArgumentNullException.ThrowIfNull(stream);
        _document = XDocument.Load(stream, LoadOptions.None);
        SetRoot(_document.Root ?? throw new InvalidDataException("The XML document does not contain a root element."));
    }

    /// <inheritdoc />
    public async Task LoadFromAsync(Stream stream, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(stream);
        using var xmlReader = System.Xml.XmlReader.Create(stream, new XmlReaderSettings { Async = true, CloseInput = false });
        _document = await XDocument.LoadAsync(xmlReader, LoadOptions.None, cancellationToken).ConfigureAwait(false);
        SetRoot(_document.Root ?? throw new InvalidDataException("The XML document does not contain a root element."));
    }

    /// <inheritdoc />
    public void SaveTo(Stream stream)
    {
        ArgumentNullException.ThrowIfNull(stream);
        _document.Save(stream, SaveOptions.None);
    }

    /// <inheritdoc />
    public async Task SaveToAsync(Stream stream, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(stream);
        using var xmlWriter = System.Xml.XmlWriter.Create(stream, new XmlWriterSettings { Async = true, CloseOutput = false, Indent = true });
        await _document.SaveAsync(xmlWriter, cancellationToken).ConfigureAwait(false);
        await xmlWriter.FlushAsync().ConfigureAwait(false);
    }

    private void SetRoot(XElement root)
    {
        var readerOptions = _reader.Options;
        var writerOptions = _writer.Options;
        var errorHandling = _reader.ConversionErrorHandling;
        _reader = new XmlReader(root) { Options = readerOptions, ConversionErrorHandling = errorHandling };
        _writer = new XmlWriter(root) { Options = writerOptions };
    }
}
