using Sachssoft.Documents.Formats.Binary;

var formatter = new BinaryDocumentFormatter();
formatter.BinaryWriter.WriteBooleanArray("flags", [true, false, true]);
formatter.BinaryWriter.WriteInt32Array("numbers", [1, 2, 3, 5, 8, 13]);
formatter.BinaryWriter.WriteDoubleArray("weights", [1.25, 2.5, 5.0]);
formatter.BinaryWriter.WriteStringArray("names", ["Alpha", "Beta", "Gamma"]);
using var stream = new MemoryStream();
formatter.SaveTo(stream);
stream.Position = 0;
var loaded = new BinaryDocumentFormatter();
loaded.LoadFrom(stream);
Console.WriteLine(string.Join(", ", loaded.BinaryReader.ReadInt32Array("numbers")));
Console.WriteLine(string.Join(", ", loaded.BinaryReader.ReadStringArray("names")));
