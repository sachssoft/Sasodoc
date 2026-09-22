namespace Sachssoft.Documents
{
    /// <summary>
    /// Defines a document formatter that provides both reading and writing capabilities.
    /// </summary>
    public interface IDocumentFormatter : IFormatReader, IFormatWriter
    {
        /// <summary>
        /// Gets the root representation of the formatted document.
        /// </summary>
        object? Root { get; }

        /// <summary>
        /// Gets the format writer used to write document data.
        /// </summary>
        FormatWriterBase Writer { get; }

        /// <summary>
        /// Gets the format reader used to read document data.
        /// </summary>
        FormatReaderBase Reader { get; }
    }
}