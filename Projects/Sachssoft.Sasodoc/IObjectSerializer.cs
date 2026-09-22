namespace Sachssoft.Documents
{
    /// <summary>
    /// Defines an object serializer that operates with strongly typed format readers and writers.
    /// </summary>
    /// <typeparam name="TReader">The type of format reader used for deserialization.</typeparam>
    /// <typeparam name="TWriter">The type of format writer used for serialization.</typeparam>
    public interface IObjectSerializer<TReader, TWriter>
        where TReader : FormatReaderBase
        where TWriter : FormatWriterBase
    {
    }
}