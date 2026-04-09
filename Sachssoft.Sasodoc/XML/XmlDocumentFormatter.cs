using System.IO;

namespace Sachssoft.Sasodoc.Xml
{
    public class XmlDocumentFormatter : IDocumentFormatter
    {
        public FormatWriterBase Writer => throw new System.NotImplementedException();

        public FormatReaderBase Reader => throw new System.NotImplementedException();

        public object? Root => throw new System.NotImplementedException();

        public void LoadFrom(Stream stream)
        {
            throw new System.NotImplementedException();
        }

        public void SaveTo(Stream stream)
        {
            throw new System.NotImplementedException();
        }
    }
}
