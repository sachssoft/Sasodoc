namespace Sachssoft.Documents
{
    /// <summary>
    /// Specifies how an object should be represented during serialization.
    /// </summary>
    public enum SerializationFormat
    {
        /// <summary>
        /// Uses a compact, machine-friendly representation.
        /// </summary>
        /// <remarks>
        /// <list type="bullet">
        /// <item><description>Binary formats may store the value as raw binary data.</description></item>
        /// <item><description>Text-based formats such as JSON or XML may store binary data as a Base64-encoded string.</description></item>
        /// </list>
        /// </remarks>
        Compact,

        /// <summary>
        /// Writes the underlying structure directly.
        /// </summary>
        /// <remarks>
        /// <list type="bullet">
        /// <item><description>JSON formats may represent the value as an object or array.</description></item>
        /// <item><description>XML formats may represent the value using nested elements.</description></item>
        /// <item><description>Binary formats may represent the value as structured binary data.</description></item>
        /// </list>
        /// </remarks>
        Underlying
    }
}
