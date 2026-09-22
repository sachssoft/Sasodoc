using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Sachssoft.Documents
{
    /// <summary>
    /// Defines functionality for loading and reading document data.
    /// </summary>
    public interface IFormatReader
    {
        /// <summary>
        /// Loads document data from the specified stream.
        /// </summary>
        /// <param name="stream">The stream from which the document data is read.</param>
        void LoadFrom(Stream stream);

        /// <summary>
        /// Asynchronously loads document data from the specified stream.
        /// </summary>
        /// <param name="stream">The stream from which the document data is read.</param>
        /// <param name="cancellationToken">A token that can be used to cancel the asynchronous operation.</param>
        /// <returns>A task that represents the asynchronous load operation.</returns>
        Task LoadFromAsync(Stream stream, CancellationToken cancellationToken = default);
    }
}