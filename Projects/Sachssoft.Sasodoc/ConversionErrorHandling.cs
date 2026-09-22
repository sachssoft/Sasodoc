namespace Sachssoft.Documents
{
    /// <summary>
    /// Specifies how conversion errors are handled when converting document values.
    /// </summary>
    public enum ConversionErrorHandling
    {
        /// <summary>
        /// Uses the default conversion error handling behavior.
        /// </summary>
        Default,

        /// <summary>
        /// Ignores the conversion error and continues processing.
        /// </summary>
        Ignore,

        /// <summary>
        /// Cancels the conversion when an error occurs.
        /// </summary>
        Cancel,

        /// <summary>
        /// Replaces the invalid value when a conversion error occurs.
        /// </summary>
        Replace
    }
}