using AspNetCoreBackendBase.Domain.Enums;

namespace AspNetCoreBackendBase.Application.Services
{
    /// <summary>
    /// Defines methods and properties for a storage service.
    /// Inherits basic storage operations from <see cref="IStorage"/>.
    /// </summary>
    public interface IStorageService : IStorage
    {
        /// <summary>
        /// Gets the name of the storage service.
        /// </summary>
        /// <value>
        /// A <see langword="string"/> representing the name of the storage service.
        /// </value>
        string StorageName { get; }

        /// <summary>
        /// Gets the type identifier for the storage service.
        /// </summary>
        /// <value>
        /// An <see cref="StorageType"/> <see langword="int"/> value representing the storage type ID.
        /// </value>
        int StorageTypeId { get; }
    }
}
