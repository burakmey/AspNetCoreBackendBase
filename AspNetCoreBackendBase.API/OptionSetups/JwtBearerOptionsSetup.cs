using AspNetCoreBackendBase.Application.DTOs;
using AspNetCoreBackendBase.Infrastructure.Options;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace AspNetCoreBackendBase.API.OptionSetups
{
    public class JwtBearerOptionsSetup : IConfigureNamedOptions<JwtBearerOptions>
    {
        readonly TokenOptions _tokenOptions;

        public JwtBearerOptionsSetup(IOptions<TokenOptions> tokenOptions)
        {
            _tokenOptions = tokenOptions.Value;
        }

        public void Configure(string? name, JwtBearerOptions options)
        {
            if (name == null || name == JwtBearerDefaults.AuthenticationScheme)
            {
                // Default configuration
                Configure(options);
            }
            else if (name == "User")
            {
                ConfigureUserOptions(options);
            }
            else if (name == "Admin")
            {
                ConfigureAdminOptions(options);
            }
        }

        public void Configure(JwtBearerOptions options)
        {
            options.TokenValidationParameters = _tokenOptions.TokenValidationParameters;

            options.Events = new JwtBearerEvents
            {
                // This handles the 401 Unauthorized challenge
                OnChallenge = async context =>
                {
                    // Skip the default behavior
                    context.HandleResponse();

                    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    context.Response.ContentType = "application/json";

                    var result = JsonConvert.SerializeObject(new BaseResponse<object>
                    {
                        IsSuccessful = false,
                        Message = "Unauthorized access, please check your credentials."
                    }); 

                    await context.Response.WriteAsync(result);
                },
                // Handle other events like token validation failure
                OnAuthenticationFailed = context =>
                {
                    context.NoResult();

                    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    context.Response.ContentType = "application/json";

                    var result = JsonConvert.SerializeObject(new BaseResponse<object>
                    {
                        IsSuccessful = false,
                        Message = "Authentication failed, token is invalid or expired."
                    });

                    return context.Response.WriteAsync(result);
                }
            };
        }

        void ConfigureUserOptions(JwtBearerOptions options)
        {
            // Custom configurations can be done for scheme "User"
            Configure(options);
            options.Events.OnAuthenticationFailed = context =>
            {
                // Handle User authentication failures
                context.NoResult();
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                context.Response.ContentType = "application/json";

                var result = JsonConvert.SerializeObject(new
                {
                    IsSuccessful = false,
                    Message = "User authentication failed."
                });

                return context.Response.WriteAsync(result);
            };
        }

        void ConfigureAdminOptions(JwtBearerOptions options)
        {
            // Custom configurations can be done for scheme "Admin"
            Configure(options);
            options.Events.OnAuthenticationFailed = context =>
            {
                // Handle Admin authentication failures
                context.NoResult();
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                context.Response.ContentType = "application/json";

                var result = JsonConvert.SerializeObject(new
                {
                    IsSuccessful = false,
                    Message = "Admin authentication failed."
                });

                return context.Response.WriteAsync(result);
            };
        }
    }
}
