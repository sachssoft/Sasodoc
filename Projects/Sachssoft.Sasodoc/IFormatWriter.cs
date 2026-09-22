using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Sachssoft.Documents
{
    /// <summary>
    /// Defines functionality for writing and saving document data.
    /// </summary>
    public interface IFormatWriter
    {
        /// <summary>
        /// Saves the written document data to the specified stream.
        /// </summary>
        /// <param name="stream">The stream to which the document data is written.</param>
        void SaveTo(Stream stream);

        /// <summary>
        /// Asynchronously saves the written document data to the specified stream.
        /// </summary>
        /// <param name="stream">The stream to which the document data is written.</param>
        /// <param name="cancellationToken">A token that can be used to cancel the asynchronous operation.</param>
        /// <returns>A task that represents the asynchronous save operation.</returns>
        Task SaveToAsync(Stream stream, CancellationToken cancellationToken = default);
    }
}