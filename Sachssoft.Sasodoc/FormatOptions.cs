using Sachssoft.Sasodoc;
using Sachssoft.Sasodoc.Naming;
using Sachssoft.Sasodoc.Naming.Case;

namespace Sachssoft.Sasodoc
{
    public class FormatOptions
    {
        // Für Eigenschaften

        public INamingConvention? PropertyNamingConvention { get; set; }

        public NamingOptions? PropertyNamingOptions { get; set; }

        public string[]? PreservedPropertyNames { get; set; }

        // Für Enum, usw...

        public INamingConvention? FieldNamingConvention { get; set; }

        public NamingOptions? FieldNamingOptions { get; set; }

        public string[]? PreservedFieldNames { get; set; }
    }
}
