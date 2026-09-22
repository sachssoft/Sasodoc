using Sachssoft.Documents;
using Sachssoft.Documents.Formats.Json;
using Sachssoft.Documents.Naming.Cases;

var formatter = new JsonDocumentFormatter();
var options = new FormatOptions { PropertyNamingConvention = new KebabCase(), FieldNamingConvention = new KebabCase() };
formatter.Writer.Options = options;
formatter.Reader.Options = options;
formatter.Writer.WriteString("PlayerName", "Tobias");
formatter.Writer.WriteInt32("HighScore", 9000);
using var stream = new MemoryStream();
formatter.SaveTo(stream);
Console.WriteLine(System.Text.Encoding.UTF8.GetString(stream.ToArray()));
