using AspNetCoreBackendBase.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace AspNetCoreBackendBase.Application.Repositories
{
    /// <summary>
    /// Represents a generic repository pattern for entities derived from <see cref="BaseEntity{TKey}"/>.
    /// </summary>
    /// <typeparam name="T">The type of the entity that derives from <see cref="BaseEntity{TKey}"/>.</typeparam>
    /// <typeparam name="TKey">The type of the unique identifier for the entity, which is used in <see cref="BaseEntity{TKey}"/>.</typeparam>
    public interface IRepository<T, TKey> where T : BaseEntity<TKey>
    { 
        /// <summary>
        /// Gets the <see cref="DbSet{T}"/> for the specified entity type, allowing for CRUD operations.
        /// </summary>
        /// <value>
        /// A <see cref="DbSet{T}"/> representing the collection of entities of type <typeparamref name="T"/>.
        /// </value>
        DbSet<T> Table { get; }
    }
}
