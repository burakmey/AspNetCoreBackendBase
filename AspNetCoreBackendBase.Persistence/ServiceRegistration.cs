using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Identity;
using AspNetCoreBackendBase.Persistence.Context;
using AspNetCoreBackendBase.Domain.Entities;
using AspNetCoreBackendBase.Application.Repositories;
using AspNetCoreBackendBase.Persistence.Repositories;
using AspNetCoreBackendBase.Application.Services;
using AspNetCoreBackendBase.Persistence.Services;
using Microsoft.EntityFrameworkCore;
using AspNetCoreBackendBase.Application;


namespace AspNetCoreBackendBase.Persistence
{
    /// <summary>
    /// Provides extension methods for registering persistence services with the dependency injection container.
    /// </summary>
    public static class ServiceRegistration
    {
        /// <summary>
        /// Registers persistence services with dependency injection container.
        /// This includes configuring the database context, identity services, and repository services.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/> to which the services are added.</param>
        public static void AddPersistenceServices(this IServiceCollection services)
        {
            // Configures the DbContext to use PostgreSQL with the connection string from configuration.
            services.AddDbContext<AspNetCoreBackendBaseDbContext>(options =>
                options.UseNpgsql(Configuration.GetConnectionStringPostgresql));

            // Configures ASP.NET Core Identity with custom password options.
            // Uses the DbContext for storing identity-related data and provides default token providers.
            services.AddIdentity<User, Role>(options =>
            {
                // Sets password policy options for Identity.
                options.Password.RequiredLength = 3;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;

            }).AddEntityFrameworkStores<AspNetCoreBackendBaseDbContext>()
            .AddDefaultTokenProviders();

            // Registers repositories with a scoped lifetime, meaning they are created once per request.
            services.AddScoped<IFileReadRepository, FileReadRepository>();
            services.AddScoped<IFileWriteRepository, FileWriteRepository>();
            services.AddScoped<IEndpointReadRepository, EndpointReadRepository>();
            services.AddScoped<IEndpointWriteRepository, EndpointWriteRepository>();

            // Registers service implementations with a scoped lifetime, meaning they are created once per request.
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IExternalAuthentication, AuthService>();
            services.AddScoped<IInternalAuthentication, AuthService>();
            services.AddScoped<IRoleService, RoleService>();
            services.AddScoped<IEndpointAuthorizationService, EndpointAuthorizationService>();
        }
    }
}
