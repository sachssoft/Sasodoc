using Sachssoft.Documents.Naming.Cases;
using System;
using System.Linq;

namespace Sachssoft.Documents
{
    /// <summary>
    /// Provides common functionality and configuration for format readers and writers.
    /// </summary>
    public abstract class FormatBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FormatBase"/> class.
        /// </summary>
        protected FormatBase() { }

        /// <summary>
        /// Gets or sets the options used for reading and writing document formats.
        /// </summary>
        public FormatOptions? Options { get; set; }

        /// <summary>
        /// Converts an identifier used for a property according to the configured property naming convention.
        /// </summary>
        /// <param name="identifier">The identifier to convert.</param>
        /// <returns>The converted identifier. Semantic identifiers are returned unchanged.</returns>
        protected DataIdentifier ConvertPropertyIdentifier(DataIdentifier identifier)
        {
            if (identifier.Kind != DataIdentifierKind.Default) return identifier;

            return new DataIdentifier(ConvertPropertyName(identifier.Value));
        }

        /// <summary>
        /// Converts an identifier used for a field according to the configured field naming convention.
        /// </summary>
        /// <param name="identifier">The identifier to convert.</param>
        /// <returns>The converted identifier. Semantic identifiers are returned unchanged.</returns>
        protected DataIdentifier ConvertFieldIdentifier(DataIdentifier identifier)
        {
            if (identifier.Kind != DataIdentifierKind.Default) return identifier;

            return new DataIdentifier(ConvertFieldName(identifier.Value));
        }

        /// <summary>
        /// Converts a property name according to the configured property naming convention.
        /// </summary>
        /// <param name="propertyName">The property name to convert.</param>
        /// <returns>The converted property name, or the original name if it is configured to be preserved.</returns>
        /// <exception cref="ArgumentException">Thrown when <paramref name="propertyName"/> is <see langword="null"/>, empty or consists only of white-space characters.</exception>
        /// <exception cref="InvalidOperationException">Thrown when the naming convention produces an invalid property name.</exception>
        protected string ConvertPropertyName(string propertyName)
        {
            if (string.IsNullOrWhiteSpace(propertyName))
                throw new ArgumentException("Property name must not be null or empty.", nameof(propertyName));

            var options = Options ?? new();

            if (options.PreservedPropertyNames != null && options.PreservedPropertyNames.Contains(propertyName))
            {
                return propertyName;
            }

            var namingConvention = options.PropertyNamingConvention ?? new SnakeCase();
            var converted = namingConvention.Convert(propertyName, options.PropertyNamingOptions ?? new());

            if (string.IsNullOrWhiteSpace(converted))
                throw new InvalidOperationException($"Conversion of property name '{propertyName}' resulted in an invalid value.");

            return converted;
        }

        /// <summary>
        /// Converts a field name according to the configured field naming convention.
        /// </summary>
        /// <param name="fieldName">The field name to convert.</param>
        /// <returns>The converted field name, or the original name if it is configured to be preserved.</returns>
        /// <exception cref="ArgumentException">Thrown when <paramref name="fieldName"/> is <see langword="null"/>, empty or consists only of white-space characters.</exception>
        /// <exception cref="InvalidOperationException">Thrown when the naming convention produces an invalid field name.</exception>
        protected string ConvertFieldName(string fieldName)
        {
            if (string.IsNullOrWhiteSpace(fieldName))
                throw new ArgumentException("Field name must not be null or empty.", nameof(fieldName));

            var options = Options ?? new();

            if (options.PreservedFieldNames != null && options.PreservedFieldNames.Contains(fieldName))
            {
                return fieldName;
            }

            var namingConvention = options.FieldNamingConvention ?? new SnakeCase();
            var converted = namingConvention.Convert(fieldName, options.FieldNamingOptions ?? new());

            if (string.IsNullOrWhiteSpace(converted))
                throw new InvalidOperationException($"Conversion of field name '{fieldName}' resulted in an invalid value.");

            return converted;
        }
    }
}