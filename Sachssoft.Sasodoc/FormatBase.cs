using Sachssoft.Sasofly.Documents.Naming.Cases;
using System.Linq;

namespace Sachssoft.Sasodoc
{
    public abstract class FormatBase
    {
        protected FormatBase() { }

        public FormatOptions? Options { get; set; }

        // Property, usw...
        protected string ConvertPropertyName(string propertyName)
        {
            var options = Options ?? new();

            if (options.PreservedPropertyNames != null && options.PreservedPropertyNames.Contains(propertyName))
            {
                return propertyName;
            }

            var nc = options.PropertyNamingConvention ?? new SnakeCase();
            return nc.Convert(propertyName, options.PropertyNamingOptions ?? new());
        }

        // Enum, usw...
        protected string ConvertFieldName(string fieldName)
        {
            var options = Options ?? new();

            if (options.PreservedFieldNames != null && options.PreservedFieldNames.Contains(fieldName))
            {
                return fieldName;
            }

            var nc = options.FieldNamingConvention ?? new SnakeCase();
            return nc.Convert(fieldName, options.FieldNamingOptions ?? new());
        }
    }
}
