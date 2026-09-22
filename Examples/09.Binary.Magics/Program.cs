using Sachssoft.Documents.Formats.Binary;

ReadOnlyMemory<byte>[] magics = [Magics.SDB, Magics.BIN, Magics.DAT, Magics.SASODOCBIN, Magics.SASODOCDAT];
foreach (var magic in magics)
{
    var options = new BinaryOptions { Magic = magic };
    var formatter = new BinaryDocumentFormatter(options);
    formatter.BinaryWriter.WriteString("value", "Magic example");
    using var stream = new MemoryStream();
    formatter.SaveTo(stream);
    Console.WriteLine($"{System.Text.Encoding.UTF8.GetString(magic.Span)}: {stream.Length} bytes");
}
