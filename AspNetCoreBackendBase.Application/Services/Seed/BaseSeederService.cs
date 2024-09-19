using AspNetCoreBackendBase.Application.Loggers;
using AspNetCoreBackendBase.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace AspNetCoreBackendBase.Application.Services
{
    /// <summary>
    /// Base class for seeding entities of type <typeparamref name="T"/> into the database.
    /// </summary>
    /// <typeparam name="T">The type of the entity to be seeded, which derives from <see cref="BaseEntity{TKey}"/>.</typeparam>
    /// <typeparam name="TKey">The type of the unique identifier for the entity, which is used in <see cref="BaseEntity{TKey}"/>.</typeparam>
    public abstract class BaseSeederService<T, TKey> : ISeedService<T, TKey> where T : BaseEntity<TKey>
    {
        private readonly DbContext _dbContext;
        private readonly Func<Task<List<T>>> _getNewEntities;
        private readonly ILogger _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseSeederService{T, TKey}"/> class.
        /// </summary>
        /// <param name="dbContext">The <see cref="DbContext"/> used to interact with the database.</param>
        /// <param name="getNewEntities">A function that retrieves new entities to be added to the database.</param>
        protected BaseSeederService(DbContext dbContext, Func<Task<List<T>>> getNewEntities)
        {
            _dbContext = dbContext;
            _getNewEntities = getNewEntities;
            _logger = SeriLogger.GetSeriLogger;
        }

        /// <summary>
        /// Asynchronously seeds the database with new entities if they do not already exist.
        /// </summary>
        /// <returns>
        /// A <see cref="Task"/> that represents the asynchronous operation.
        /// </returns>
        public async Task SeedAsync()
        {
            try
            {
                // Retrieve new entities to be added to the database
                var newEntities = await _getNewEntities();

                if (newEntities.Count != 0)
                {
                    // Log the type of new entities that will be added
                    _logger.Information($"The following new {typeof(T).Name} entities will be added to the database:");
                    foreach (var entity in newEntities)
                    {
                        _logger.Information(GetEntityInfo(entity));
                    }

                    // Add new entities to the database
                    await _dbContext.Set<T>().AddRangeAsync(newEntities);
                    await _dbContext.SaveChangesAsync();
                    _logger.Information($"{typeof(T).Name} seeding completed successfully.");
                }
                else
                {
                    // Log that no new entities were found
                    _logger.Information($"No new {typeof(T).Name} to add. Database is already up-to-date.");
                }
            }
            catch (Exception ex)
            {
                // Log any errors that occur during the seeding process
                _logger.Error(ex, "An error occurred while seeding the database.");
                throw;
            }
        }

        /// <summary>
        /// Abstract method to retrieve information about an entity for logging purposes.
        /// </summary>
        /// <param name="entity">The entity for which information is to be retrieved.</param>
        /// <returns>
        /// A <see langword="string"/> that represents information about the entity.
        /// </returns>
        protected abstract string GetEntityInfo(T entity);
    }
}
