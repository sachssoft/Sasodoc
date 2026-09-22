using Sachssoft.Documents.Formats.Binary;

var formatter = new BinaryDocumentFormatter(new BinaryOptions { Magic = Magics.SDB });
formatter.BinaryWriter.WriteString("message", "Async Sasodoc Binary");
formatter.BinaryWriter.WriteDateTime("created", DateTime.UtcNow);
await using var stream = new MemoryStream();
await formatter.SaveToAsync(stream);
stream.Position = 0;
var loaded = new BinaryDocumentFormatter(new BinaryOptions { Magic = Magics.SDB });
await loaded.LoadFromAsync(stream);
Console.WriteLine(loaded.BinaryReader.ReadString("message"));
Console.WriteLine(loaded.BinaryReader.ReadDateTime("created"));
