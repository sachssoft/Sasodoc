using Sachssoft.Sasodoc;
using Sachssoft.Sasodoc.Formats.Json;
using Sachssoft.Sasodoc.Naming.Cases;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sachsssoft.Sasodoc.Examples.Animals
{
    class Animal
    {
        private readonly FormatOptions _options;
        private readonly JsonDocumentFormatter _formatter;

        public Animal()
        {
            _options = new FormatOptions
            {
                FieldNamingConvention = new KebabCase(),
                PropertyNamingConvention = new KebabCase()
            };

            _formatter = new JsonDocumentFormatter();
            _formatter.Reader.Options = _options;
            _formatter.Writer.Options = _options;
        }

        public string? Name { get; set; }

        public int Age { get; set; }

        public AnimalType Type { get; set; }

        public AnimalOwner? Owner { get; set; }

        public static Animal Load(Stream stream)
        {
            var animal = new Animal();
            animal._formatter.LoadFrom(stream);
            animal.OnRead(animal._formatter.Reader);
            return animal;
        }

        public void Save(Stream stream)
        {
            OnWrite(_formatter.Writer);
            _formatter.SaveTo(stream);
        }

        protected virtual void OnRead(JsonReader reader)
        {
            Name = reader.ReadString(nameof(Name));
            Age = reader.ReadInt32(nameof(Age));
            Type = reader.ReadEnum<AnimalType>(nameof(Type));

            if (reader.Contains("owner"))
            {
                var ownerReader = (JsonReader)reader.Read("owner");
                ownerReader.Options = _options;
                Owner = new AnimalOwner();
                Owner.OnRead(ownerReader);
            }
        }

        protected virtual void OnWrite(JsonWriter writer)
        {
            writer.WriteString(nameof(Name), Name);
            writer.WriteInt32(nameof(Age), Age);
            writer.WriteEnum<AnimalType>(nameof(Type), Type);

            if (Owner != null)
            {
                var ownerWriter = new JsonWriter();
                ownerWriter.Options = _options;
                Owner.OnWrite(ownerWriter);
                writer.Write("owner", ownerWriter);
            }
        }

    }
}
