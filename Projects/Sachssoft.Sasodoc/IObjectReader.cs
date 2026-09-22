namespace Sachssoft.Documents
{
    /// <summary>
    /// Defines an object that can read its data using a strongly typed format reader.
    /// </summary>
    /// <typeparam name="TReader">The type of format reader used to read the object data.</typeparam>
    public interface IObjectReader<TReader> where TReader : FormatReaderBase
    {
        /// <summary>
        /// Reads the object data using the specified format reader.
        /// </summary>
        /// <param name="reader">The format reader used to read the object data.</param>
        void Read(TReader reader);
    }
}