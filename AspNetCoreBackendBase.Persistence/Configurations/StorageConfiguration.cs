using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using AspNetCoreBackendBase.Domain.Entities;

namespace Persistence.Configurations
{
    public class StorageConfiguration : IEntityTypeConfiguration<Storage>
    {
        // Configures the Storage entity in the Entity Framework model.
        public void Configure(EntityTypeBuilder<Storage> builder)
        {
            // Ignores the UpdatedAt property in the database mapping.
            builder.Ignore(s => s.UpdatedAt);
        }
    }
}
