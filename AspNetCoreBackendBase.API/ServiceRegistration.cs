using AspNetCoreBackendBase.API.Filters;

namespace AspNetCoreBackendBase.API
{
    public static class ServiceRegistration
    {
        public static void AddPresentationServices(this IServiceCollection services)
        {
            services.AddScoped<RolePermissionFilter>();
        }
    }
}
