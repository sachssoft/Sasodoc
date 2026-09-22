using Sachssoft.Documents.Formats.Ini;

var formatter = new IniFormatter();
var writer = formatter.IniWriter;

writer.WriteString("name", "Simba");
writer.WriteInt32("age", 8);
writer.WriteBoolean("active", true);
writer.WriteDouble("weight", 4.75);
writer.WriteStringArray("tags", ["cat", "pet", "friendly"]);

using var stream = new MemoryStream();
formatter.SaveTo(stream);
Console.WriteLine($"Ini bytes: {stream.Length}");

stream.Position = 0;
var loaded = new IniFormatter();
loaded.LoadFrom(stream);
var reader = loaded.IniReader;

Console.WriteLine($"Name: {reader.ReadString("name")}");
Console.WriteLine($"Age: {reader.ReadInt32("age")}");
Console.WriteLine($"Active: {reader.ReadBoolean("active")}");
Console.WriteLine($"Weight: {reader.ReadDouble("weight")}");
Console.WriteLine($"Tags: {string.Join(", ", reader.ReadStringArray("tags"))}");
