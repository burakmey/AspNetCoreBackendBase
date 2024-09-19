using AspNetCoreBackendBase.Application.Services;
using AspNetCoreBackendBase.Application.Utils;
using AspNetCoreBackendBase.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace AspNetCoreBackendBase.Persistence.SeedServices
{
    /// <summary>
    /// Seeder service for seeding <see cref="Route"/> entities into the database.
    /// </summary>
    public class RouteSeederService : BaseSeederService<Route, int>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RouteSeederService"/> class.
        /// </summary>
        /// <param name="dbContext">The <see cref="DbContext"/> used to interact with the database.</param>
        public RouteSeederService(DbContext dbContext) : base(dbContext,
            async () =>
            {
                // Get all route names from assembly utility
                List<string> routes = AssemblyUtils.GetRoutesWithAssembly();
                DateTime currentTime = DateTime.UtcNow;

                // Retrieve existing route names from the database
                var existingRoutes = await dbContext.Set<Route>()
                    .AsNoTracking()
                    .Select(r => r.Name)
                    .ToListAsync();

                // Create new Route entities from route names if they don't already exist
                return routes
                    .Where(route => !existingRoutes.Contains(route))
                    .Select(route => new Route
                    {
                        Name = route,
                        CreatedAt = currentTime,
                    })
                    .ToList();
            })
        {
        }

        protected override string GetEntityInfo(Route entity)
        {
            return $"{nameof(Route)} => {entity.Name}";
        }
    }
}