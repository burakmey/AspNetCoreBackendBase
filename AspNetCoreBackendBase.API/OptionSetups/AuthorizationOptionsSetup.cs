using AspNetCoreBackendBase.Application.Loggers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;
using System.Security.Claims;

namespace AspNetCoreBackendBase.API.OptionSetups
{
    public class AuthorizationOptionsSetup : IConfigureOptions<AuthorizationOptions>
    {
        public void Configure(AuthorizationOptions options)
        {
            options.AddPolicy("UserPolicy", policy =>
            {
                policy.RequireAuthenticatedUser(); // User must be authenticated
                policy.RequireClaim(ClaimTypes.Name); // User must have a name claim
                policy.RequireClaim(ClaimTypes.Role, "Roles[]");
                SeriLogger.GetSeriLogger.Information("UserPolicy");
            });

            options.AddPolicy("AdminPolicy", policy =>
            {
                policy.RequireAuthenticatedUser(); // Admin must be authenticated
                policy.RequireRole("Admin"); // Admin must have the Admin role
                SeriLogger.GetSeriLogger.Information("AdminPolicy");

            });
        }
    }
}
