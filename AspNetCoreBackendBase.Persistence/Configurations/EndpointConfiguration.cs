using AspNetCoreBackendBase.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AspNetCoreBackendBase.Persistence.Configurations
{
    public class EndpointConfiguration : IEntityTypeConfiguration<Endpoint>
    {
        public void Configure(EntityTypeBuilder<Endpoint> builder)
        {
            builder.Ignore(e => e.UpdatedAt);

            builder.HasOne(e => e.Route)
                .WithMany()
                .HasForeignKey(e => e.RouteId)
                .IsRequired();
        }
    }
}
