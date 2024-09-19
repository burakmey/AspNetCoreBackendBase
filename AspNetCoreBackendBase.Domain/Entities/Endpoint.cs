namespace AspNetCoreBackendBase.Domain.Entities
{
    /// <summary>
    /// Represents an API endpoint within the application.
    /// </summary>
    public class Endpoint : BaseEntity<int>
    {
        public override int Id { get; set; }

        /// <summary>
        /// Gets or sets the code for the endpoint.
        /// </summary>
        /// <value>
        /// A <see langword="string"/> representing the code of the endpoint.
        /// </value>
        public required string Code { get; set; }

        /// <summary>
        /// Gets or sets the unique identifier of the route associated with this endpoint.
        /// </summary>
        /// <value>
        /// An <see langword="int"/> representing the unique identifier of the associated route.
        /// </value>
        public required int RouteId { get; set; }

        /// <summary>
        /// Gets or sets the route associated with this endpoint.
        /// </summary>
        /// <value>
        /// A <see cref="Entities.Route"/> object representing the route details for this endpoint,
        /// but it can be <see langword="null"/> if it is not explicitly included in the query.
        /// </value>
        public Route Route { get; set; } = null!;

        /// <summary>
        /// Gets or sets the collection of role-endpoint associations for this endpoint.
        /// </summary>
        /// <value>
        /// A collection of <see cref="RoleEndpoint"/> objects representing the roles linked to this endpoint.
        /// If no roles are associated, the collection will be empty, not <see langword="null"/>.
        /// </value>
        public ICollection<RoleEndpoint> RoleEndpoints { get; set; } = [];
    }
}
