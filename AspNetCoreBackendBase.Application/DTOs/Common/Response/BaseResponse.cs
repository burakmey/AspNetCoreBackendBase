namespace AspNetCoreBackendBase.Application.DTOs
{
    /// <summary>
    /// Represents a base response structure for operations in the application.
    /// </summary>
    /// <typeparam name="T">The type of data returned by the operation.</typeparam>
    public class BaseResponse<T>
    {
        /// <summary>
        /// Gets or sets a value indicating whether the operation was successful.
        /// </summary>
        /// <value>
        /// A <see langword="bool"/> indicating the success status of the operation.
        /// </value>
        public bool IsSuccessful { get; set; } 

        /// <summary>
        /// Gets or sets the data resulting from the operation.
        /// </summary>
        /// <value>
        /// The data returned by the operation. The type of the data is determined by the type parameter <typeparamref name="T"/>.
        /// </value>
        public T? Data { get; set; }

        /// <summary>
        /// Gets or sets a message providing additional context or details about the operation's result.
        /// </summary>
        /// <value>
        /// A <see langword="string"/> message that typically contains error information or status messages related to the operation.
        /// </value>
        public string? Message { get; set; }
    }
}
