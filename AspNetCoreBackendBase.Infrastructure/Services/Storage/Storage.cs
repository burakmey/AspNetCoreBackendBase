namespace AspNetCoreBackendBase.Infrastructure.Services
{
    /// <summary>
    /// Provides methods for handling file storage operations.
    /// </summary>
    public class Storage
    {
        /// <summary>
        /// Delegate definition for checking if a file exists in a specified path or container.
        /// </summary>
        /// <param name="pathOrContainerName">A <see langword="string"/> representing the path or container name where the file might exist.</param>
        /// <param name="fileName">A <see langword="string"/> representing the name of the file to check for existence.</param>
        /// <returns><see langword="true"/> if the file exists; otherwise, <see langword="false"/>.</returns>
        protected delegate bool HasFile(string pathOrContainerName, string fileName);

        /// <summary>
        /// Asynchronously generates a unique file name by appending a timestamp to avoid name collisions.
        /// </summary>
        /// <param name="pathOrContainerName">>A <see langword="string"/> representing the path or container name where the file will be stored.</param>
        /// <param name="fileName">>A <see langword="string"/> representing the original file name to be renamed.</param>
        /// <param name="hasFileMethod">A delegate method used to check if a file with the new name already exists.</param>
        /// <returns>A <see cref="Task"/> that represents the asynchronous operation. The task result is <see langword="string"/> representing a unique file name.</returns>
        protected static async Task<string> FileRenameAsync(string pathOrContainerName, string fileName, HasFile hasFileMethod)
        {
            // Asynchronously generates a unique file name.
            string newFileName = await Task.Run(async () =>
            {
                // Construct a new file name with a timestamp to ensure uniqueness.
                string newFileName = $"{Path.GetFileNameWithoutExtension(fileName)}_{DateTime.UtcNow:yyyyMMddHHmmssfff}{Path.GetExtension(fileName)}";

                // Check if the new file name already exists.
                if (hasFileMethod(pathOrContainerName, newFileName))
                    // Recursively call FileRenameAsync to generate a new name if a collision is detected.
                    return await FileRenameAsync(pathOrContainerName, newFileName, hasFileMethod);
                else
                    // Return the new file name if no collision is detected.
                    return newFileName;
            });
            return newFileName;
        }
    }
}

