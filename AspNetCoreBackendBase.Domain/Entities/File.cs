namespace AspNetCoreBackendBase.Domain.Entities
{
    /// <summary>
    /// Represents a file stored in the system, including its extension and associated storage details.
    /// </summary>
    public class File : BaseEntity<Guid>
    {
        public override Guid Id { get; set; }

        /// <summary>
        /// Gets or sets the name of the file.
        /// </summary>
        /// <value>
        /// A <see langword="string"/> representing the name of the file.
        /// </value>
        public required string FileName { get; set; }

        /// <summary>
        /// Gets or sets the path where the file is stored.
        /// </summary>
        /// <value>
        /// A <see langword="string"/> representing the path of the file.
        /// </value>
        public required string Path { get; set; }

        /// <summary>
        /// Gets or sets the unique identifier of the <see cref="Entities.Storage"/> associated with this file.
        /// </summary>
        /// <value>
        /// An <see langword="int"/> representing the unique identifier of the associated storage.
        /// </value>
        public int StorageId { get; set; }

        /// <summary>
        /// Gets or sets the storage associated with this file.
        /// </summary>
        /// A <see cref="Entities.Storage"/> object representing the storage details for this file,
        /// but it can be <see langword="null"/> if it is not explicitly included in the query.
        /// </value>
        public Storage Storage { get; set; } = null!;

        /// <summary>
        /// Gets or sets the unique identifier of the <see cref="Entities.Extension"/> associated with this file.
        /// </summary>
        /// <value>
        /// An <see langword="int"/> representing the unique identifier of the file extension.
        /// </value>
        public int ExtensionId { get; set; }

        /// <summary>
        /// Gets or sets the unique identifier of the extension associated with this file.
        /// </summary>
        /// <value>
        /// A <see cref="Entities.Extension"/> object representingthe extension details for this file,
        /// but it can be <see langword="null"/> if it is not explicitly included in the query.
        /// </value>
        public Extension Extension { get; set; } = null!;
    }
}
