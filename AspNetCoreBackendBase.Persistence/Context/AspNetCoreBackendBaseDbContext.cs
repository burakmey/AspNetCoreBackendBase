using AspNetCoreBackendBase.Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using File = AspNetCoreBackendBase.Domain.Entities.File;

namespace AspNetCoreBackendBase.Persistence.Context
{
    public class AspNetCoreBackendBaseDbContext : IdentityDbContext<User, Role, Guid>
    {
        // DbSets for accessing entities in the database.
        public DbSet<Extension> Extensions { get; set; }
        public DbSet<File> Files { get; set; }
        public DbSet<Storage> Storages { get; set; }
        public DbSet<Endpoint> Endpoints { get; set; }
        public DbSet<Route> Routes { get; set; }
        public DbSet<RoleEndpoint> RoleEndpoints { get; set; }

        // Constructor accepting DbContextOptions to configure the context.
        public AspNetCoreBackendBaseDbContext(DbContextOptions<AspNetCoreBackendBaseDbContext> options) : base(options)
        {
        }

        // Configures the model by applying configurations from the assembly.
        // This method is used to configure entity mappings and relationships.
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Applies configurations from the current assembly.
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            // Calls the base method to apply additional configurations from IdentityDbContext.
            base.OnModelCreating(modelBuilder);
        }

        // Overrides SaveChangesAsync to set creation and update timestamps.
        // Automatically updates the CreatedAt and UpdatedAt fields of entities.
        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            // Iterates over changed entities and sets timestamps based on their state.
            var datas = ChangeTracker.Entries<BaseEntity<object>>();
            foreach (var data in datas)
            {
                _ = data.State switch
                {
                    EntityState.Added => data.Entity.CreatedAt = DateTime.UtcNow,  // Sets CreatedAt on newly added entities.
                    EntityState.Modified => data.Entity.UpdatedAt = DateTime.UtcNow, // Updates UpdatedAt on modified entities.
                    _ => DateTime.UtcNow  // Sets UpdatedAt on unchanged entities.
                };
            }

            // Calls the base method to save changes to the database.
            return await base.SaveChangesAsync(cancellationToken);
        }
    }
}
