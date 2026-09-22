using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Sachssoft.Documents
{
    /// <summary>
    /// Provides extension methods for saving and loading object format readers and writers
    /// using streams, files, byte arrays and Base64 representations.
    /// </summary>
    public static class SerializerExtensions
    {
        #region Formatter

        /// <summary>
        /// Saves the specified object format writer to a stream.
        /// </summary>
        /// <typeparam name="TWriter">The type of format writer used by the object.</typeparam>
        /// <param name="obj">The object format writer to save.</param>
        /// <param name="stream">The stream to which the serialized data is written.</param>
        public static void Save<TWriter>(this IObjectFormatWriter<TWriter> obj, Stream stream) where TWriter : FormatWriterBase
        {
            obj.SaveTo(stream);
        }

        /// <summary>
        /// Saves the specified object format writer to a file.
        /// </summary>
        /// <typeparam name="TWriter">The type of format writer used by the object.</typeparam>
        /// <param name="obj">The object format writer to save.</param>
        /// <param name="filePath">The path of the file to which the serialized data is written.</param>
        public static void Save<TWriter>(this IObjectFormatWriter<TWriter> obj, string filePath) where TWriter : FormatWriterBase
        {
            using var stream = File.Create(filePath);
            obj.SaveTo(stream);
        }

        /// <summary>
        /// Asynchronously saves the specified object format writer to a file.
        /// </summary>
        /// <typeparam name="TWriter">The type of format writer used by the object.</typeparam>
        /// <param name="obj">The object format writer to save.</param>
        /// <param name="filePath">The path of the file to which the serialized data is written.</param>
        /// <param name="cancellationToken">A token that can be used to cancel the asynchronous operation.</param>
        /// <returns>A task that represents the asynchronous save operation.</returns>
        public static async Task SaveAsync<TWriter>(this IObjectFormatWriter<TWriter> obj, string filePath, CancellationToken cancellationToken = default) where TWriter : FormatWriterBase
        {
            await using var stream = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.None, 4096, useAsync: true);
            await obj.SaveToAsync(stream, cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// Saves the specified object format writer to a byte array.
        /// </summary>
        /// <typeparam name="TWriter">The type of format writer used by the object.</typeparam>
        /// <param name="obj">The object format writer to save.</param>
        /// <returns>A byte array containing the serialized data.</returns>
        public static byte[] SaveToBytes<TWriter>(this IObjectFormatWriter<TWriter> obj) where TWriter : FormatWriterBase
        {
            using var stream = new MemoryStream();
            obj.SaveTo(stream);
            return stream.ToArray();
        }

        /// <summary>
        /// Saves the specified object format writer to a Base64-encoded string.
        /// </summary>
        /// <typeparam name="TWriter">The type of format writer used by the object.</typeparam>
        /// <param name="obj">The object format writer to save.</param>
        /// <returns>A Base64-encoded string containing the serialized data.</returns>
        public static string SaveToBase64<TWriter>(this IObjectFormatWriter<TWriter> obj) where TWriter : FormatWriterBase
        {
            return Convert.ToBase64String(obj.SaveToBytes());
        }

        /// <summary>
        /// Loads serialized data from a stream into the specified object format reader.
        /// </summary>
        /// <typeparam name="TReader">The type of format reader used by the object.</typeparam>
        /// <param name="obj">The object format reader into which the data is loaded.</param>
        /// <param name="stream">The stream from which the serialized data is read.</param>
        public static void Load<TReader>(this IObjectFormatReader<TReader> obj, Stream stream) where TReader : FormatReaderBase
        {
            obj.LoadFrom(stream);
        }

        /// <summary>
        /// Loads serialized data from a file into the specified object format reader.
        /// </summary>
        /// <typeparam name="TReader">The type of format reader used by the object.</typeparam>
        /// <param name="obj">The object format reader into which the data is loaded.</param>
        /// <param name="filePath">The path of the file from which the serialized data is read.</param>
        public static void Load<TReader>(this IObjectFormatReader<TReader> obj, string filePath) where TReader : FormatReaderBase
        {
            using var stream = File.OpenRead(filePath);
            obj.LoadFrom(stream);
        }

        /// <summary>
        /// Asynchronously loads serialized data from a file into the specified object format reader.
        /// </summary>
        /// <typeparam name="TReader">The type of format reader used by the object.</typeparam>
        /// <param name="obj">The object format reader into which the data is loaded.</param>
        /// <param name="filePath">The path of the file from which the serialized data is read.</param>
        /// <param name="cancellationToken">A token that can be used to cancel the asynchronous operation.</param>
        /// <returns>A task that represents the asynchronous load operation.</returns>
        public static async Task LoadAsync<TReader>(this IObjectFormatReader<TReader> obj, string filePath, CancellationToken cancellationToken = default) where TReader : FormatReaderBase
        {
            await using var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read, 4096, useAsync: true);
            await obj.LoadFromAsync(stream, cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// Loads serialized data from a byte array into the specified object format reader.
        /// </summary>
        /// <typeparam name="TReader">The type of format reader used by the object.</typeparam>
        /// <param name="obj">The object format reader into which the data is loaded.</param>
        /// <param name="data">The byte array containing the serialized data.</param>
        public static void LoadFromBytes<TReader>(this IObjectFormatReader<TReader> obj, byte[] data) where TReader : FormatReaderBase
        {
            using var stream = new MemoryStream(data);
            obj.LoadFrom(stream);
        }

        /// <summary>
        /// Loads serialized data from a Base64-encoded string into the specified object format reader.
        /// </summary>
        /// <typeparam name="TReader">The type of format reader used by the object.</typeparam>
        /// <param name="obj">The object format reader into which the data is loaded.</param>
        /// <param name="base64">The Base64-encoded string containing the serialized data.</param>
        /// <exception cref="FormatException"><paramref name="base64"/> is not a valid Base64-encoded string.</exception>
        public static void LoadFromBase64<TReader>(this IObjectFormatReader<TReader> obj, string base64) where TReader : FormatReaderBase
        {
            var bytes = Convert.FromBase64String(base64);
            obj.LoadFromBytes(bytes);
        }

        #endregion
    }
}