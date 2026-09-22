namespace Sachssoft.Documents
{
    /// <summary>
    /// Defines an object reader that also provides access to format reading functionality.
    /// </summary>
    /// <typeparam name="TReader">The type of format reader used to read object data.</typeparam>
    public interface IObjectFormatReader<TReader> : IFormatReader, IObjectReader<TReader> where TReader : FormatReaderBase
    {
    }
}