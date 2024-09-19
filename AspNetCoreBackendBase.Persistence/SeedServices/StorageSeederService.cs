using AspNetCoreBackendBase.Application.Services;
using AspNetCoreBackendBase.Domain.Entities;
using AspNetCoreBackendBase.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace AspNetCoreBackendBase.Persistence.SeedServices
{
    /// <summary>
    /// Seeder service for seeding <see cref="Storage"/> entities into the database.
    /// </summary>
    public class StorageSeederService : BaseSeederService<Storage, int>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="StorageSeederService"/> class.
        /// </summary>
        /// <param name="dbContext">The <see cref="DbContext"/> used to interact with the database.</param>
        public StorageSeederService(DbContext dbContext) : base(dbContext,
            async () =>
            {
                // Get all storage types
                var storageTypes = Enum.GetValues(typeof(StorageType)).Cast<StorageType>();
                DateTime currentTime = DateTime.UtcNow;

                // Retrieve existing storage names from the database
                var existingStorages = await dbContext.Set<Storage>()
                    .AsNoTracking()
                    .Select(r => r.Name)
                    .ToListAsync();

                // Create new Storage entities from storageType names
                return storageTypes
                    .Where(storage => !existingStorages.Contains(storage.ToString()))
                    .Select(storage => new Storage
                    {
                        Name = storage.ToString(),
                        CreatedAt = currentTime,
                    })
                    .ToList();
            })
        {
        }

        protected override string GetEntityInfo(Storage entity)
        {
            return $"{nameof(Storage)} => {entity.Name}";
        }
    }
}
