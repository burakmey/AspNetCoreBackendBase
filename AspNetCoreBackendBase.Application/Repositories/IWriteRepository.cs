using AspNetCoreBackendBase.Domain.Entities;

namespace AspNetCoreBackendBase.Application.Repositories
{
    /// <summary>
    /// Represents a write repository pattern for entities derived from <see cref="BaseEntity{TKey}"/>.
    /// </summary>
    /// <typeparam name="T">The type of the entity that derives from <see cref="BaseEntity{TKey}"/>.</typeparam>
    /// <typeparam name="TKey">The type of the unique identifier for the entity, which is used in <see cref="BaseEntity{TKey}"/>.</typeparam>
    public interface IWriteRepository<T, TKey> : IRepository<T, TKey> where T : BaseEntity<TKey>
    {
        /// <summary>
        /// Asynchronously adds a single entity of type <typeparamref name="T"/> to the database.
        /// </summary>
        /// <param name="model">The entity to add.</param>
        /// <returns>
        /// The <see cref="Task"/> that represents the asynchronous operation, containing <see langword="true"/> if the entity was successfully added, otherwise <see langword="false"/>
        /// </returns>
        Task<bool> AddAsync(T model);

        /// <summary>
        /// Asynchronously adds multiple entities of type <typeparamref name="T"/> to the database.
        /// </summary>
        /// <param name="models">The list of entities to add.</param>
        /// <returns>
        /// The <see cref="Task"/> that represents the asynchronous operation, containing <see langword="true"/> if the entities were successfully added, otherwise <see langword="false"/>.
        /// </returns>
        Task<bool> AddRangeAsync(List<T> models);

        /// <summary>
        /// Removes a single entity of type <typeparamref name="T"/> from the database.
        /// </summary>
        /// <param name="model">The entity to remove.</param>
        /// <returns>
        /// <see langword="true"/> if the entity was successfully removed, otherwise <see langword="false"/>.
        /// </returns>
        bool Remove(T model);

        /// <summary>
        /// Removes multiple entities of type <typeparamref name="T"/> from the database.
        /// </summary>
        /// <param name="models">The list of entities to remove.</param>
        /// <returns>
        /// <see langword="true"/> if the entities were successfully removed, otherwise <see langword="false"/>.
        /// </returns>
        bool RemoveRange(List<T> models);

        /// <summary>
        /// Asynchronously removes an entity of type <typeparamref name="T"/> from the database by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the entity to remove.</param>
        /// <returns>
        /// The <see cref="Task"/> that represents the asynchronous operation, containing <see langword="true"/> if the entity was successfully removed, otherwise <see langword="false"/>.
        /// </returns>
        Task<bool> RemoveAsync(TKey id);

        /// <summary>
        /// Updates an existing entity of type <typeparamref name="T"/> in the database.
        /// </summary>
        /// <param name="model">The entity to update.</param>
        /// <returns>
        /// <see langword="true"/> if the entity was successfully updated, otherwise <see langword="false"/>.
        /// </returns>
        bool Update(T model);

        /// <summary>
        /// Asynchronously saves all changes made in the context of the database.
        /// </summary>
        /// <returns>
        /// The <see cref="Task"/> that represents the asynchronous operation, containing the number of state entries written to the database.
        /// </returns>
        Task<int> SaveAsync();
    }
}
