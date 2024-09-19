using AspNetCoreBackendBase.Application.DTOs;
using AspNetCoreBackendBase.Application.Services;
using AspNetCoreBackendBase.Application.Utils;
using AspNetCoreBackendBase.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace AspNetCoreBackendBase.Persistence.SeedServices
{
    /// <summary>
    /// Seeder service for seeding <see cref="Endpoint"/> entities into the database.
    /// </summary>
    public class EndpointSeederService : BaseSeederService<Endpoint, int>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EndpointSeederService"/> class.
        /// </summary>
        /// <param name="dbContext">The <see cref="DbContext"/> used to interact with the database.</param>
        public EndpointSeederService(DbContext dbContext) : base(dbContext,
            async () =>
            {
                // Get all endpoints from assembly utility
                List<Menu> menus = AssemblyUtils.GetMenusWithAssembly();
                //List<string> routes = AssemblyUtils.GetRoutesWithAssembly();

                DateTime currentTime = DateTime.UtcNow;

                var routes = await dbContext.Set<Route>()
                    .AsNoTracking()
                    .ToListAsync();

                // Retrieve existing endpoint names from the database
                var existingEndpoints = await dbContext.Set<Endpoint>()
                    .AsNoTracking()
                    .Include(e => e.Route)
                    .ToListAsync();

                // Creates a list of Endpoint entities based on the retrieved routes.
                List<Endpoint> endpointEntities = [];
                foreach (Menu menu in menus)
                {
                    int routeId = routes.First(r => r.Name == menu.Route).Id;
                    for (int i = 0; i < menu.Codes.Count; i++)
                    {
                        string code = menu.Codes[i];

                        if (existingEndpoints.Find(e => e.Code == code && e.RouteId == routeId) == null)
                        {
                            endpointEntities.Add(new()
                            {
                                RouteId = routeId,
                                Code = code,
                                CreatedAt = currentTime
                            });
                        }

                    }
                }
                return endpointEntities;
            })
        {
        }

        protected override string GetEntityInfo(Endpoint entity)
        {
            return $"{nameof(Endpoint)} => {nameof(entity.RouteId)}: {entity.RouteId}, {nameof(entity.Code)}: {entity.Code}";
        }
    }
}
