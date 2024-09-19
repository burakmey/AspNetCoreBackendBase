using AspNetCoreBackendBase.Application.Services;
using AspNetCoreBackendBase.Domain.Entities;
using AspNetCoreBackendBase.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace AspNetCoreBackendBase.Persistence.SeedServices
{
    /// <summary>
    /// Seeder service for seeding <see cref="Extension"/> entities into the database.
    /// </summary>
    public class ExtensionSeederService : BaseSeederService<Extension, int>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ExtensionSeederService"/> class.
        /// </summary>
        /// <param name="dbContext">The <see cref="DbContext"/> used to interact with the database.</param>
        public ExtensionSeederService(DbContext dbContext) : base(dbContext,
            async () =>
            {
                // Get all extension types
                var extensionTypes = Enum.GetValues(typeof(ExtensionType)).Cast<ExtensionType>();
                DateTime currentTime = DateTime.UtcNow;

                // Retrieve existing extension names from the database
                var existingExtensions = await dbContext.Set<Extension>()
                    .AsNoTracking()
                    .Select(r => r.Name)
                    .ToListAsync();

                // Create new Extension entities from extensionType names
                return extensionTypes
                    .Where(extension => !existingExtensions.Contains(extension.ToString()))
                    .Select(extension => new Extension
                    {
                        Name = extension.ToString(),
                        CreatedAt = currentTime,
                    })
                    .ToList();
            })
        {
        }

        protected override string GetEntityInfo(Extension entity)
        {
            return $"{nameof(Extension)} => {entity.Name}";
        }
    }
}
