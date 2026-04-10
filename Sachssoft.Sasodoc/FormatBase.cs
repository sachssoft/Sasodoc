using Sachssoft.Sasodoc.Naming.Cases;
using System;
using System.Linq;

namespace Sachssoft.Sasodoc
{
    public abstract class FormatBase
    {
        protected FormatBase() { }

        public FormatOptions? Options { get; set; }

        protected string ConvertPropertyName(string propertyName)
        {
            if (string.IsNullOrWhiteSpace(propertyName))
                throw new ArgumentException("Property name must not be null or empty.", nameof(propertyName));

            var options = Options ?? new();

            if (options.PreservedPropertyNames != null && options.PreservedPropertyNames.Contains(propertyName))
            {
                return propertyName;
            }

            var nc = options.PropertyNamingConvention ?? new SnakeCase();
            var converted = nc.Convert(propertyName, options.PropertyNamingOptions ?? new());

            if (string.IsNullOrWhiteSpace(converted))
                throw new InvalidOperationException(
                    $"Conversion of property name '{propertyName}' resulted in an invalid value."
                );

            return converted;
        }

        protected string ConvertFieldName(string fieldName)
        {
            if (string.IsNullOrWhiteSpace(fieldName))
                throw new ArgumentException("Field name must not be null or empty.", nameof(fieldName));

            var options = Options ?? new();

            if (options.PreservedFieldNames != null && options.PreservedFieldNames.Contains(fieldName))
            {
                return fieldName;
            }

            var nc = options.FieldNamingConvention ?? new SnakeCase();
            var converted = nc.Convert(fieldName, options.FieldNamingOptions ?? new());

            if (string.IsNullOrWhiteSpace(converted))
                throw new InvalidOperationException(
                    $"Conversion of field name '{fieldName}' resulted in an invalid value."
                );

            return converted;
        }
    }
}
