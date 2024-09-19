using Microsoft.AspNetCore.Http;

namespace AspNetCoreBackendBase.Application.Services
{
    /// <summary>
    /// Defines methods for handling file storage operations.
    /// </summary>
    public interface IStorage
    {
        /// <summary>
        /// Asynchronously uploads files to the specified path or container.
        /// </summary>
        /// <param name="pathOrContainerName">A <see langword="string"/> representing the path or container name where the files will be uploaded.</param>
        /// <param name="files">A collection of files to upload.</param>
        /// <returns>
        /// The <see cref="Task"/> that represents the asynchronous operation, containing a list of tuples,
        /// where each tuple contains the file name and the path or container name.
        /// </returns>
        Task<List<(string fileName, string pathOrContainerName)>> UploadAsync(string pathOrContainerName, IFormFileCollection files);

        /// <summary>
        /// Asynchronously deletes a specific file from the specified path or container.
        /// </summary>
        /// <param name="pathOrContainerName">A <see langword="string"/> representing the path or container name where the file is located.</param>
        /// <param name="fileName">A <see langword="string"/> representing the name of the file to be deleted.</param>
        /// <returns>
        /// The <see cref="Task"/> that represents the asynchronous operation, containing <see langword="true"/>
        /// if the file was successfully deleted; otherwise, <see langword="false"/>.
        /// </returns>
        Task<bool> DeleteAsync(string pathOrContainerName, string fileName);

        /// <summary>
        /// Retrieves a list of file names from the specified path or container.
        /// </summary>
        /// <param name="pathOrContainerName">A <see langword="string"/> representing the path or container name from which to retrieve the file names.</param>
        /// <returns>
        /// A list of <see langword="string"/> representing the file names.
        /// </returns>
        List<string> GetFiles(string pathOrContainerName);

        /// <summary>
        /// Checks if a specific file exists in the specified path or container.
        /// </summary>
        /// <param name="pathOrContainerName">A <see langword="string"/> representing the path or container name where the file is located.</param>
        /// <param name="fileName">A <see langword="string"/> representing the name of the file to check.</param>
        /// <returns>
        /// <see langword="true"/> if the file exists; otherwise, <see langword="false"/>.
        /// </returns>
        bool HasFile(string pathOrContainerName, string fileName);
    }
}
