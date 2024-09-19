namespace AspNetCoreBackendBase.Domain.Entities
{
    /// <summary>
    /// Represents the base class for all entities in the domain.
    /// </summary>
    /// <typeparam name="TKey">The type used for the primary key for the entity.</typeparam>
    public abstract class BaseEntity<TKey>
    {
        /// <summary>
        /// Gets or sets the unique identifier for the entity.
        /// </summary>
        /// <value>
        /// A value of type <typeparamref name="TKey"/> representing the unique identifier.
        /// </value>
        public abstract TKey Id { get; set; }

        /// <summary>
        /// Gets or sets the date and time when the entity was created.
        /// </summary>
        /// <value>
        /// A <see cref="DateTime"/> representing the creation timestamp.
        /// </value>
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// Gets or sets the date and time when the entity was last updated.
        /// </summary>
        /// <value>
        /// A <see cref="DateTime"/> representing the last updated timestamp.
        /// </value>
        public DateTime UpdatedAt { get; set; }
    }
}
