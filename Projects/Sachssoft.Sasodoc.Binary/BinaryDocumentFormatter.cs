namespace Sachssoft.Documents.Formats.Binary;

/// <summary>Provides reading and writing support for Sasodoc binary documents.</summary>
public sealed class BinaryDocumentFormatter : IDocumentFormatter
{
    /// <summary>Initializes a formatter using default binary options.</summary>
    public BinaryDocumentFormatter() : this(new BinaryOptions()) { }

    /// <summary>Initializes a formatter using the specified binary options.</summary>
    /// <param name="options">The binary format options.</param>
    public BinaryDocumentFormatter(BinaryOptions options)
    {
        BinaryOptions = options ?? throw new ArgumentNullException(nameof(options));
        BinaryReader = new BinaryReader(new BinaryNode(), BinaryOptions);
        BinaryWriter = new BinaryWriter(new BinaryNode(), BinaryOptions);
    }

    /// <summary>Gets the binary format options.</summary>
    public BinaryOptions BinaryOptions { get; }

    /// <summary>Gets the strongly typed binary reader.</summary>
    public BinaryReader BinaryReader { get; }

    /// <summary>Gets the strongly typed binary writer.</summary>
    public BinaryWriter BinaryWriter { get; }

    /// <inheritdoc />
    public object Root => BinaryReader;

    /// <inheritdoc />
    public FormatWriterBase Writer => BinaryWriter;

    /// <inheritdoc />
    public FormatReaderBase Reader => BinaryReader;

    /// <inheritdoc />
    public void SaveTo(Stream stream) => BinaryCodec.Write(stream, BinaryWriter.Node, BinaryOptions);

    /// <inheritdoc />
    public async Task SaveToAsync(Stream stream, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(stream);
        using var buffer = new MemoryStream();
        SaveTo(buffer);
        buffer.Position = 0;
        await buffer.CopyToAsync(stream, 81920, cancellationToken).ConfigureAwait(false);
    }

    /// <inheritdoc />
    public void LoadFrom(Stream stream) => BinaryReader.Node = BinaryCodec.Read(stream, BinaryOptions);

    /// <inheritdoc />
    public async Task LoadFromAsync(Stream stream, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(stream);
        using var buffer = new MemoryStream();
        await stream.CopyToAsync(buffer, 81920, cancellationToken).ConfigureAwait(false);
        buffer.Position = 0;
        LoadFrom(buffer);
    }
}
