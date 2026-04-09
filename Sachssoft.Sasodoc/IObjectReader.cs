namespace Sachssoft.Sasodoc;

public interface IObjectReader<TReader> where TReader : FormatReaderBase
{
    void Read(TReader reader);
}