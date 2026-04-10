namespace Sachssoft.Sasodoc
{
    public interface IObjectFormatWriter<TWriter> : IFormatWriter, IObjectWriter<TWriter> where TWriter : FormatWriterBase
    {
    }
}