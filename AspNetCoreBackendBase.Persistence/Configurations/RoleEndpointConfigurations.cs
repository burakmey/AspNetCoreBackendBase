using AspNetCoreBackendBase.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AspNetCoreBackendBase.Persistence.Configurations
{
    public class RoleEndpointConfigurations : IEntityTypeConfiguration<RoleEndpoint>
    {
        public void Configure(EntityTypeBuilder<RoleEndpoint> builder)
        {
            // Ignores UpdatedAt property in the database mapping.
            builder.Ignore(re => re.UpdatedAt);

            builder.Ignore(re => re.Id);

            // Configures the composite primary key using RoleId and EndpointId.
            builder.HasKey(re => new { re.RoleId, re.EndpointId });
            // This type not working because Entity Framework won’t understand that these two fields should form the composite key in the database.
            //builder.HasKey(re => re.Id);

            // Configures the relationship between RoleEndpoint and Role entities.
            builder.HasOne(re => re.Role)  // Each RoleEndpoint has one Role.
                   .WithMany(r => r.RoleEndpoints)  // A Role can have many RoleEndpoints.
                   .HasForeignKey(re => re.RoleId)  // RoleId is the foreign key in RoleEndpoint.
                   .IsRequired(); // Role is required.

            // Configures the relationship between RoleEndpoint and Endpoint entities.
            builder.HasOne(re => re.Endpoint)  // Each RoleEndpoint has one Endpoint.
                   .WithMany(e => e.RoleEndpoints)  // An Endpoint can have many RoleEndpoints.
                   .HasForeignKey(re => re.EndpointId) // EndpointId is the foreign key in RoleEndpoint.
                   .IsRequired(); // Endpoint is required.
        }
    }
}
