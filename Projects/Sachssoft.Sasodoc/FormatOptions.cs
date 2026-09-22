using Sachssoft.Documents.Naming;

namespace Sachssoft.Documents
{
    /// <summary>
    /// Provides configuration options for document formatting and naming conventions.
    /// </summary>
    public class FormatOptions
    {
        /// <summary>
        /// Gets or sets the naming convention used to convert property names.
        /// </summary>
        public INamingConvention? PropertyNamingConvention { get; set; }

        /// <summary>
        /// Gets or sets the options used when converting property names.
        /// </summary>
        public NamingOptions? PropertyNamingOptions { get; set; }

        /// <summary>
        /// Gets or sets the property names that are preserved without applying a naming convention.
        /// </summary>
        public string[]? PreservedPropertyNames { get; set; }

        /// <summary>
        /// Gets or sets the naming convention used to convert field names.
        /// </summary>
        public INamingConvention? FieldNamingConvention { get; set; }

        /// <summary>
        /// Gets or sets the options used when converting field names.
        /// </summary>
        public NamingOptions? FieldNamingOptions { get; set; }

        /// <summary>
        /// Gets or sets the field names that are preserved without applying a naming convention.
        /// </summary>
        public string[]? PreservedFieldNames { get; set; }
    }
}