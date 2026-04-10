namespace Sachssoft.Sasodoc
{
    public interface IObjectFormatReader<TReader> : IFormatReader, IObjectReader<TReader> where TReader : FormatReaderBase
    {
    }
}
