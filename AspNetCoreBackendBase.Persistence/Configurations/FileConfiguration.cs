using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using File = AspNetCoreBackendBase.Domain.Entities.File;

namespace Persistence.Configurations
{
    public class FileConfiguration : IEntityTypeConfiguration<File>
    {
        public void Configure(EntityTypeBuilder<File> builder)
        {
            // Ignores the UpdatedAt property in the database mapping.
            builder.Ignore(f => f.UpdatedAt);
        }
    }
}
