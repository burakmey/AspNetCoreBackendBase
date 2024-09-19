using AspNetCoreBackendBase.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace AspNetCoreBackendBase.Persistence.Configurations
{
    public class RoleConfiguration : IEntityTypeConfiguration<Role>
    {
        public void Configure(EntityTypeBuilder<Role> builder)
        {
            builder.Property(r => r.Name)
                .IsRequired()
                .HasMaxLength(64);

            builder.Property(r => r.NormalizedName)
                .IsRequired()
                .HasMaxLength(64);
        }
    }
}
