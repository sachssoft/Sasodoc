namespace Sachssoft.Sasodoc
{
    /// <summary>
    /// Specifies how an object should be serialized.
    /// </summary>
    public enum SerializationFormat
    {
        /// <summary>
        /// Compact, machine-friendly representation.
        /// - In binary serializers: raw <c>byte[]</c>
        /// - In text-based serializers (e.g., JSON, XML): Base64-encoded string
        /// </summary>
        Compact,

        /// <summary>
        /// The underlying structure is written directly.
        /// - In JSON: as an object or array
        /// - In XML: as nested elements
        /// - In binary formats: as structured raw data
        /// </summary>
        Underlying
    }
}
