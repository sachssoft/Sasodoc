using Sachssoft.Documents.Formats.Binary;

foreach (var encoding in Enum.GetValues<BinaryLengthEncoding>())
{
    var options = new BinaryOptions { Magic = Magics.SDB, IdentifierMode = BinaryIdentifierMode.Inline, IdentifierLengthEncoding = encoding, LengthEncoding = encoding };
    var formatter = new BinaryDocumentFormatter(options);
    formatter.BinaryWriter.WriteString("description", "Length encoding example");
    formatter.BinaryWriter.WriteInt32Array("values", [10, 20, 30, 40, 50]);
    using var stream = new MemoryStream();
    formatter.SaveTo(stream);
    Console.WriteLine($"{encoding}: {stream.Length} bytes");
}
