namespace Sachssoft.Documents.Serialization
{
    /// <summary>
    /// Defines methods for serializing and deserializing objects using document format readers and writers.
    /// </summary>
    public interface ISerialization
    {
        /// <summary>
        /// Serializes the specified source object using the specified format writer.
        /// </summary>
        /// <param name="source">The object to serialize.</param>
        /// <param name="writer">The format writer that receives the serialized data.</param>
        void Serialize(object source, FormatWriterBase writer);

        /// <summary>
        /// Deserializes data from the specified format reader into the target object.
        /// </summary>
        /// <param name="target">The object to populate with the deserialized data.</param>
        /// <param name="reader">The format reader that provides the serialized data.</param>
        void Deserialize(object target, FormatReaderBase reader);
    }
}