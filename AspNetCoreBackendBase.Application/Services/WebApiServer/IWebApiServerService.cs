using AspNetCoreBackendBase.Application.DTOs;

namespace AspNetCoreBackendBase.Application.Services
{
    /// <summary>
    /// Defines methods for interacting with and managing web API server configurations.
    /// </summary>
    public interface IWebApiServerService
    {
        /// <summary>
        /// Retrieves a list of authorized definition endpoints for a specified type.
        /// </summary>
        /// <param name="type">The <see cref="Type"/> representing the class for which to retrieve authorized endpoints.</param>
        /// <returns>
        /// A <see cref="List{Menu}"/> containing the authorized definition endpoints for the specified type.
        /// </returns>
        List<Menu> GetAuthorizeDefinitionEndpoints(Type type);
    }
}
