using Microsoft.Extensions.DependencyInjection;

namespace AspNetCoreBackendBase.Application
{
    /// <summary>
    /// Provides extension methods for registering application services in the dependency injection container.
    /// </summary>
    public static class ServiceRegistration
    {
        /// <summary>
        /// Registers application services including MediatR in the dependency injection container.
        /// This method scans the assembly for MediatR handlers and requests and registers them.
        /// </summary>
        public static void AddApplicationServices(this IServiceCollection services)
        {
            services.AddMediatR(configuration => configuration.RegisterServicesFromAssembly(typeof(ServiceRegistration).Assembly));
        }
    }
}
