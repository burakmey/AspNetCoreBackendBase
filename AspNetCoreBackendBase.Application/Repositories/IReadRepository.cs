using AspNetCoreBackendBase.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace AspNetCoreBackendBase.Application.Repositories
{
    /// <summary>
    /// Represents a read repository pattern for entities derived from <see cref="BaseEntity{TKey}"/>.
    /// </summary>
    /// <typeparam name="T">The type of the entity that derives from <see cref="BaseEntity{TKey}"/>.</typeparam>
    /// <typeparam name="TKey">The type of the unique identifier for the entity, which is used in <see cref="BaseEntity{TKey}"/>.</typeparam>
    public interface IReadRepository<T, TKey> : IRepository<T, TKey> where T : BaseEntity<TKey>
    {
        /// <summary>
        /// Retrieves all entities of type <typeparamref name="T"/> from the database.
        /// </summary>
        /// <param name="tracking">Indicates whether to track changes to these entities in the <see cref="DbContext"/>.</param>
        /// <returns>
        /// The <see cref="IQueryable{T}"/> that represents the collection of entities of type <typeparamref name="T"/>.
        /// </returns>
        IQueryable<T> GetAll(bool tracking = true);

        /// <summary>
        /// Retrieves all entities of type <typeparamref name="T"/> that satisfy a given condition.
        /// </summary>
        /// <param name="method">An <see cref="Expression{Func{T, bool}}"/> specifying the condition for filtering entities.</param>
        /// <param name="tracking">Indicates whether to track changes to these entities in the <see cref="DbContext"/>.</param>
        /// <returns>
        /// The <see cref="IQueryable{T}"/> that represents the collection of entities of type <typeparamref name="T"/> that match the condition.
        /// </returns>
        IQueryable<T> GetWhere(Expression<Func<T, bool>> method, bool tracking = true);

        /// <summary>
        /// Asynchronously retrieves a single entity of type <typeparamref name="T"/> that satisfies a given condition.
        /// </summary>
        /// <param name="method">An <see cref="Expression{Func{T, bool}}"/> specifying the condition for filtering the entity.</param>
        /// <param name="tracking">Indicates whether to track changes to this entity in the <see cref="DbContext"/>.</param>
        /// <returns>
        /// The <see cref="Task"/> that represents the asynchronous operation, containing the entity that matches the condition,
        /// or <see langword="null"/> if no matching entity is found.
        /// </returns>
        Task<T?> GetSingleAsync(Expression<Func<T, bool>> method, bool tracking = true);

        /// <summary>
        /// Asynchronously retrieves an entity of type <typeparamref name="T"/> by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the entity to retrieve.</param>
        /// <param name="tracking">Indicates whether to track changes to this entity in the <see cref="DbContext"/>.</param>
        /// <returns>
        /// The <see cref="Task"/> that represents the asynchronous operation, containing the entity with the specified identifier,
        /// or <see langword="null"/> if no entity with the identifier is found.
        /// </returns>
        Task<T?> GetByIdAsync(TKey id, bool tracking = true);
    }
}
