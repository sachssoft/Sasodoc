using Sachssoft.Documents.Formats.Binary;

var options = new BinaryOptions { IdentifierMode = BinaryIdentifierMode.Inline, IdentifierLengthEncoding = BinaryLengthEncoding.Variable, Magic = Magics.SDB };
var formatter = new BinaryDocumentFormatter(options);
formatter.BinaryWriter.WriteString("player-name", "Tobias");
formatter.BinaryWriter.WriteInt32("score", 1250);
using var stream = new MemoryStream();
formatter.SaveTo(stream);
stream.Position = 0;
var loaded = new BinaryDocumentFormatter(options);
loaded.LoadFrom(stream);
Console.WriteLine(loaded.BinaryReader.ReadString("player-name"));
Console.WriteLine(loaded.BinaryReader.ReadInt32("score"));
