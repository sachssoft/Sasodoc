using Sachssoft.Documents.Formats.Binary;

var options = new BinaryOptions { IdentifierMode = BinaryIdentifierMode.None, Magic = Magics.SDB };
var formatter = new BinaryDocumentFormatter(options);
formatter.BinaryWriter.WriteInt32("ignored-name", 42);
formatter.BinaryWriter.WriteString("ignored-title", "Sequential binary");
using var stream = new MemoryStream();
formatter.SaveTo(stream);
Console.WriteLine($"Bytes: {stream.Length}");
