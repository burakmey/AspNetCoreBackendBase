using AspNetCoreBackendBase.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AspNetCoreBackendBase.Persistence.Configurations
{
    public class RouteConfiguration : IEntityTypeConfiguration<Route>
    {
        public void Configure(EntityTypeBuilder<Route> builder)
        {
            // Ignores the UpdatedAt property in the database mapping.
            builder.Ignore(s => s.UpdatedAt);
            builder.Property(s => s.Name).IsRequired();
        }
    }
}
