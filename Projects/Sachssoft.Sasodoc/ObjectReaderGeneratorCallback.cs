namespace Sachssoft.Documents
{
    /// <summary>
    /// Represents a callback that creates an object reader from the specified format reader.
    /// </summary>
    /// <param name="reader">The format reader used to create the object reader.</param>
    /// <returns>The created object reader, or <see langword="null"/> if no object reader is created.</returns>
    public delegate object? ObjectReaderGeneratorCallback(FormatReaderBase reader);

    /// <summary>
    /// Represents a callback that creates a strongly typed object reader from the specified format reader.
    /// </summary>
    /// <typeparam name="TReader">The type of format reader supplied to the callback.</typeparam>
    /// <param name="reader">The format reader used to create the object reader.</param>
    /// <returns>The created object reader, or <see langword="null"/> if no object reader is created.</returns>
    public delegate IObjectReader<TReader>? ObjectReaderGeneratorCallback<TReader>(TReader reader) where TReader : FormatReaderBase;
}
