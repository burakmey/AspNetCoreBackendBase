using AspNetCoreBackendBase.Application;
using AspNetCoreBackendBase.Application.Services;
using AspNetCoreBackendBase.Domain.Enums;
using AspNetCoreBackendBase.Infrastructure.Options;
using AspNetCoreBackendBase.Infrastructure.Services;
using Microsoft.Extensions.DependencyInjection;

namespace AspNetCoreBackendBase.Infrastructure
{
    /// <summary>
    /// Provides extension methods for registering infrastructure services with the dependency injection container.
    /// </summary>
    public static class ServiceRegistration
    {
        /// <summary>
        /// Registers infrastructure services with the dependency injection container.
        /// These services are registered with a scoped lifetime, meaning they are created once per request.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/> to which the services are added.</param>
        public static void AddInfrastructureServices(this IServiceCollection services)
        {
            // This service will be resolved whenever IStorageService is requested.
            services.AddScoped<IStorageService, StorageService>();

            // This service will be resolved whenever ITokenService is requested.
            services.AddScoped<ITokenService, TokenService>();

            // This service will be resolved whenever IMailService is requested.
            services.AddScoped<IMailService, MailService>();

            services.Configure<TokenOptions>(Configuration.GetConfigurationRoot.GetSection(TokenOptions.Options));

            //services.AddStorage<AzureStorageService>();
            //services.AddStorage(StorageType.Azure);

        }

        /// <summary>
        /// Registers a custom implementation of <see cref="IStorage"/> with the dependency injection container.
        /// </summary>
        /// <typeparam name="T">The type of the custom storage implementation that derives from <see cref="Storage"/> and implements <see cref="IStorage"/>.</typeparam>
        /// <param name="services">The <see cref="IServiceCollection"/> to which the service is added.</param>
        public static void AddStorage<T>(this IServiceCollection services) where T : Storage, IStorage
        {
            services.AddScoped<IStorage, T>();
        }


        /// <summary>
        /// Registers a default storage implementation based on the specified <see cref="StorageType"/>.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/> to which the service is added.</param>
        /// <param name="storageType">The <see cref="StorageType"/> indicating the type of storage implementation to register.</param>
        public static void AddStorage(this IServiceCollection services, StorageType storageType)
        {
            switch (storageType)
            {
                case StorageType.Azure:
                    services.AddScoped<IStorage, AzureStorageService>();
                    break;
                case StorageType.AWS:
                    break;
                case StorageType.Local:
                    services.AddScoped<IStorage, LocalStorageService>();
                    break;
                default:
                    // No action is taken for unhandled storage types.
                    break;
            }
        }
    }
}
