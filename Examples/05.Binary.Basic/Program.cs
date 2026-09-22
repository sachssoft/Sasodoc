using Sachssoft.Documents.Formats.Binary;

var formatter = new BinaryDocumentFormatter();
var writer = formatter.BinaryWriter;

writer.WriteString("name", "Simba");
writer.WriteInt32("age", 8);
writer.WriteBoolean("active", true);
writer.WriteDouble("weight", 4.75);
writer.WriteStringArray("tags", ["cat", "pet", "friendly"]);

using var stream = new MemoryStream();
formatter.SaveTo(stream);
Console.WriteLine($"Binary bytes: {stream.Length}");

stream.Position = 0;
var loaded = new BinaryDocumentFormatter();
loaded.LoadFrom(stream);
var reader = loaded.BinaryReader;

Console.WriteLine($"Name: {reader.ReadString("name")}");
Console.WriteLine($"Age: {reader.ReadInt32("age")}");
Console.WriteLine($"Active: {reader.ReadBoolean("active")}");
Console.WriteLine($"Weight: {reader.ReadDouble("weight")}");
Console.WriteLine($"Tags: {string.Join(", ", reader.ReadStringArray("tags"))}");
