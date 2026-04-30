using Sachssoft.Sasodoc.Formats.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;

namespace Sachsssoft.Sasodoc.Examples.Animals
{
    class AnimalOwner
    {
        public string? FirstName { get; set; }

        public string? LastName { get; set; }

        internal protected virtual void OnRead(JsonReader reader)
        {
            FirstName = reader.ReadString(nameof(FirstName));
            LastName = reader.ReadString(nameof(LastName));
        }

        internal protected virtual void OnWrite(JsonWriter writer)
        {
            writer.WriteString(nameof(FirstName), FirstName);
            writer.WriteString(nameof(LastName), LastName);
        }

        public override string ToString()
        {
            return $"{FirstName} {LastName}";
        }
    }
}
