using System;

namespace Sachssoft.Documents.Serialization
{
    /// <summary>
    /// Provides a strongly typed base implementation for serialization handlers.
    /// </summary>
    /// <typeparam name="T">The object type handled by the serialization handler.</typeparam>
    public abstract class SerializationBase<T> : ISerialization where T : class
    {
        /// <summary>
        /// Serializes the specified source object using the specified format writer.
        /// </summary>
        /// <param name="source">The object to serialize.</param>
        /// <param name="writer">The format writer that receives the serialized data.</param>
        public abstract void Serialize(T source, FormatWriterBase writer);

        /// <summary>
        /// Deserializes data from the specified format reader into the target object.
        /// </summary>
        /// <param name="target">The object to populate with the deserialized data.</param>
        /// <param name="reader">The format reader that provides the serialized data.</param>
        public abstract void Deserialize(T target, FormatReaderBase reader);

        /// <inheritdoc />
        void ISerialization.Serialize(object source, FormatWriterBase writer)
        {
            ArgumentNullException.ThrowIfNull(writer);

            if (source is null)
                throw new ArgumentNullException(nameof(source), $"Cannot serialize null source for type {typeof(T).FullName}.");

            if (source is not T typed)
                throw new InvalidOperationException($"Serialize type mismatch. Expected {typeof(T).FullName}, got {source.GetType().FullName}.");

            Serialize(typed, writer);
        }

        /// <inheritdoc />
        void ISerialization.Deserialize(object target, FormatReaderBase reader)
        {
            ArgumentNullException.ThrowIfNull(reader);

            if (target is null)
                throw new ArgumentNullException(nameof(target), $"Cannot deserialize into null target for type {typeof(T).FullName}.");

            if (target is not T typed)
                throw new InvalidOperationException($"Deserialize type mismatch. Expected {typeof(T).FullName}, got {target.GetType().FullName}.");

            Deserialize(typed, reader);
        }
    }
}