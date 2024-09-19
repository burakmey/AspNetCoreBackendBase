using AspNetCoreBackendBase.Application;
using AspNetCoreBackendBase.Application.Loggers;
using AspNetCoreBackendBase.Persistence.Context;
using AspNetCoreBackendBase.Persistence.SeedServices;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Serilog;

namespace AspNetCoreBackendBase.Persistence
{
    /// <summary>
    /// Factory class for creating instances of <see cref="AspNetCoreBackendBaseDbContext"/> at design time.
    /// This is used by Entity Framework Core tools, such as migrations and database updates.
    /// </summary>
    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<AspNetCoreBackendBaseDbContext>
    {
        /// <summary>
        /// Creates a new instance of <see cref="AspNetCoreBackendBaseDbContext"/> with design-time options.
        /// </summary>
        /// <param name="args">Arguments passed to the factory. If "seed" is present, the database seeding process will be triggered.</param>
        /// <returns>
        /// An instance of <see cref="AspNetCoreBackendBaseDbContext"/> configured with design-time options.
        /// </returns>
        public AspNetCoreBackendBaseDbContext CreateDbContext(string[] args)
        {
            ILogger _logger = SeriLogger.GetSeriLogger;

            // Create a new instance of DbContextOptionsBuilder configured to use PostgreSQL.
            DbContextOptionsBuilder<AspNetCoreBackendBaseDbContext> dbContextOptionsBuilder = new();

            // Configure the DbContext to use PostgreSQL with the connection string from configuration.
            dbContextOptionsBuilder.UseNpgsql(Configuration.GetConnectionStringPostgresql);

            // Create the DbContext instance.
            var context = new AspNetCoreBackendBaseDbContext(dbContextOptionsBuilder.Options);

            // Check if the "seed" argument is present to determine if seeding should be performed.
            if (args.Contains("seed"))
            {
                // Check if the database can be connected to.
                if (!context.Database.CanConnect())
                {
                    _logger.Warning("The database does not exist. Please ensure the database is created before seeding.");
                }
                else
                {
                    // Check for any pending migrations before seeding.
                    var pendingMigrations = context.Database.GetPendingMigrations();
                    if (pendingMigrations.Any())
                    {
                        _logger.Warning("The database has pending migrations. Apply all migrations before seeding.");
                    }
                    else
                    {
                        _logger.Information("Seeding started!");

                        // Seed the database if it is up-to-date and the "seed" argument is present
                        SeedDatabase(context).GetAwaiter().GetResult();

                        _logger.Information("Seeding finished!");
                    }
                }
            }

            // Return the configured DbContext instance.
            return context;
        }

        /// <summary>
        /// Seeds the database with initial data.
        /// </summary>
        /// <param name="context">An instance of <see cref="AspNetCoreBackendBaseDbContext"/> to seed.</param>
        /// <returns>
        /// A <see cref="Task"/> representing the asynchronous operation.
        /// </returns>
        static async Task SeedDatabase(AspNetCoreBackendBaseDbContext context)
        {
            // Begin a new database transaction.
            using var transaction = await context.Database.BeginTransactionAsync();
            try
            {
                // Instantiate seed services for various entities.
                var routeSeederService = new RouteSeederService(context);
                var storageSeederService = new StorageSeederService(context);
                var extensionSeederService = new ExtensionSeederService(context);
                var endpointSeederService = new EndpointSeederService(context);

                // Perform seeding operations.
                await storageSeederService.SeedAsync();
                await routeSeederService.SeedAsync();
                await extensionSeederService.SeedAsync();
                await endpointSeederService.SeedAsync();

                // Additional seed services can be added here if needed.

                // Commit the transaction if all seeding operations succeed.
                await transaction.CommitAsync();
            }
            catch (Exception ex)
            {
                // Roll back the transaction if an error occurs during seeding.
                await transaction.RollbackAsync();

                // Optionally rethrow the exception to notify the caller of the failure
                throw new Exception("An error occurred during seeding: " + ex.Message);
            }
        }
    }
}
