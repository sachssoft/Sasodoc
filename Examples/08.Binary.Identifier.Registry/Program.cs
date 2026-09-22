using Sachssoft.Documents.Formats.Binary;

var options = new BinaryOptions { IdentifierMode = BinaryIdentifierMode.Registry, IdentifierLengthEncoding = BinaryLengthEncoding.Variable, LengthEncoding = BinaryLengthEncoding.Variable, Magic = Magics.SASODOCBIN };
var formatter = new BinaryDocumentFormatter(options);
formatter.BinaryWriter.WriteString("name", "Registry example");
formatter.BinaryWriter.WriteInt32("width", 1920);
formatter.BinaryWriter.WriteInt32("height", 1080);
using var stream = new MemoryStream();
formatter.SaveTo(stream);
Console.WriteLine($"Registry document: {stream.Length} bytes");
