namespace Sachssoft.Sasodoc
{
    public interface IObjectWriter<TWriter> where TWriter : FormatWriterBase
    {
        void Write(TWriter writer);
    }
}
