namespace Sachssoft.Documents
{
    /// <summary>
    /// Defines an object writer that also provides access to format writing functionality.
    /// </summary>
    /// <typeparam name="TWriter">The type of format writer used to write object data.</typeparam>
    public interface IObjectFormatWriter<TWriter> : IFormatWriter, IObjectWriter<TWriter> where TWriter : FormatWriterBase
    {
    }
}