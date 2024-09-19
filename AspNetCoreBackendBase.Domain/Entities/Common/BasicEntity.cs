namespace AspNetCoreBackendBase.Domain.Entities
{
    /// <summary>
    /// Represents a basic entity that extends <see cref="BaseEntity{int}"/> with additional properties.
    /// </summary>
    public abstract class BasicEntity : BaseEntity<int>
    {
        public override int Id { get; set; }

        /// <summary>
        /// Gets or sets the name of the entity.
        /// </summary>
        /// <value>
        /// A <see langword="string"/> representing the name of the <see cref="BasicEntity"/>.
        /// </value>
        public required string Name { get; set; }
    }
}
