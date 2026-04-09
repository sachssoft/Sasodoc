using System;

namespace Sachssoft.Sasodoc
{
    public interface IDocumentFormatter : IFormatReader, IFormatWriter
    {
        object? Root { get; }

        FormatWriterBase Writer { get; }

        FormatReaderBase Reader { get; }

    }
}
