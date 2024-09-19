using Microsoft.AspNetCore.Identity;

namespace AspNetCoreBackendBase.Domain.Entities
{
    /// <summary>
    /// Represents a role in the application, extending <see cref="IdentityRole{Guid}"/> to include additional role-related information.
    /// </summary>
    public class Role : IdentityRole<Guid>
    {
        public Role() : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Role"/> class with the specified role name.
        /// </summary>
        /// <param name="roleName">The name of the role to be assigned.</param>
        public Role(string roleName) : base(roleName)
        {
            Id = Guid.NewGuid();
            Name = roleName;
            NormalizedName = roleName;
        }

        /// <summary>
        /// Override the Name property to ensure it is never null in practice.
        /// </summary>
        public override string? Name
        {
            get => base.Name;
            set => base.Name = value ?? throw new ArgumentNullException(nameof(Name), "Role name cannot be null.");
        }

        /// <summary>
        /// Override the NormalizedName property to ensure it is never null in practice.
        /// </summary>
        public override string? NormalizedName
        {
            get => base.NormalizedName;
            set => base.NormalizedName = value?.ToUpper() ?? throw new ArgumentNullException(nameof(NormalizedName), "Normalized role name cannot be null.");
        }

        /// <summary>
        /// Gets or sets the collection of role-endpoint associations for this role.
        /// </summary>
        /// <value>
        /// A collection of <see cref="RoleEndpoint"/> objects representing the endpoints linked to this role.
        /// If no endpoints are associated, the collection will be empty, not <see langword="null"/>.
        /// </value>
        public ICollection<RoleEndpoint> RoleEndpoints { get; set; } = [];
    }
}