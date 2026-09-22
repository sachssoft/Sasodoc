namespace Sachssoft.Documents
{
    /// <summary>
    /// Defines an object that can write its data using a strongly typed format writer.
    /// </summary>
    /// <typeparam name="TWriter">The type of format writer used to write the object data.</typeparam>
    public interface IObjectWriter<TWriter> where TWriter : FormatWriterBase
    {
        /// <summary>
        /// Writes the object data using the specified format writer.
        /// </summary>
        /// <param name="writer">The format writer used to write the object data.</param>
        void Write(TWriter writer);
    }
}