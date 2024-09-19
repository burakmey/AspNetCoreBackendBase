using AspNetCoreBackendBase.Domain.Entities;

namespace AspNetCoreBackendBase.Application.Services
{
    /// <summary>
    /// Defines methods for seeding data into the database.
    /// </summary>
    /// <typeparam name="T">The type of the entity to be seeded, which derives from <see cref="BaseEntity{TKey}"/>.</typeparam>
    /// <typeparam name="TKey">The type of the unique identifier for the entity, which is used in <see cref="BaseEntity{TKey}"/>.</typeparam>
    public interface ISeedService<T, TKey> where T : BaseEntity<TKey>
    {
        /// <summary>
        /// Asynchronously seeds data for the specified entity type into the database.
        /// </summary>
        /// <returns>
        /// The <see cref="Task"/> that represents the asynchronous operation.
        /// </returns>
        Task SeedAsync();
    }
}
