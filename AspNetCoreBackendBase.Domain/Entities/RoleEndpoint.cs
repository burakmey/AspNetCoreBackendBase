namespace AspNetCoreBackendBase.Domain.Entities
{
    /// <summary>
    /// Represents the association between a role and an endpoint in the application.
    /// This class uses a composite key consisting of <see cref="RoleId"/> (of type <see cref="Guid"/>) and <see cref="EndpointId"/> (of type <see langword="int"/>).
    /// </summary>
    public class RoleEndpoint : BaseEntity<object>
    {
        /// <summary>
        /// Gets or sets the composite unique identifier consisting of <see cref="RoleId"/> and <see cref="EndpointId"/>.
        /// </summary>
        /// <value>
        /// An <see langword="object"/> representing the composite key of the association, which combines <see cref="RoleId"/> and <see cref="EndpointId"/>.
        /// </value>
        public override object Id { get => new { RoleId, EndpointId }; set { /* Not needed */ } }

        /// <summary>
        /// Gets or sets the unique identifier of the associated role.
        /// </summary>
        /// <value>
        /// A <see cref="Guid"/> representing the unique identifier of the role.
        /// </value>
        public Guid RoleId { get; set; }

        /// <summary>
        /// Gets or sets the role associated with this endpoint.
        /// </summary>
        /// <value>
        /// A <see cref="Entities.Role"/> object representing the role associated with this endpoint,
        /// but it can be <see langword="null"/> if it is not explicitly included in the query.
        /// </value>
        public Role Role { get; set; } = null!;

        /// <summary>
        /// Gets or sets the unique identifier of the associated endpoint.
        /// </summary>
        /// <value>
        /// An <see langword="int"/> representing the unique identifier of the endpoint.
        /// </value>
        public int EndpointId { get; set; }

        /// <summary>
        /// Gets or sets the endpoint associated with this role.
        /// </summary>
        /// <value>
        /// A <see cref="Entities.Endpoint"/> object representing the endpoint associated with this role,
        /// but it can be <see langword="null"/> if it is not explicitly included in the query.
        /// </value>
        public Endpoint Endpoint { get; set; } = null!;
    }
}
