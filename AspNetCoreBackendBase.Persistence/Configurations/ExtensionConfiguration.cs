using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using AspNetCoreBackendBase.Domain.Entities;

namespace Persistence.Configurations
{
    public class ExtensionConfiguration : IEntityTypeConfiguration<Extension>
    {
        // Configures the Extension entity in the Entity Framework model.
        public void Configure(EntityTypeBuilder<Extension> builder)
        {
            // Ignores the UpdatedAt property in the database mapping.
            builder.Ignore(s => s.UpdatedAt);
        }
    }
}
