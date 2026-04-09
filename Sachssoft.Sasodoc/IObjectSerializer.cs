namespace Sachssoft.Sasodoc;

public interface IObjectSerializer<TReader, TWriter>
     where TReader : FormatReaderBase
     where TWriter : FormatWriterBase
{
}
