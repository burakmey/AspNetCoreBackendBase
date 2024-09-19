using AspNetCoreBackendBase.Application.DTOs;
using Microsoft.AspNetCore.Diagnostics;
using System.Net.Mime;
using System.Text.Json;

namespace AspNetCoreBackendBase.API.Extensions
{
    public static class ExceptionHandlerExtension
    {
        // Configures global exception handling for the application.
        public static void ConfigureExceptionHandler<T>(this WebApplication app)
        {
            // Retrieves the logger from the application services.
            ILogger<T> logger = app.Services.GetRequiredService<ILogger<T>>();

            // Configures the exception handling middleware.
            app.UseExceptionHandler(builder =>
            {
                // Defines what to do when an exception occurs.
                builder.Run(async context =>
                {
                    // Sets the response content type to JSON.
                    context.Response.ContentType = MediaTypeNames.Application.Json;

                    // Retrieves the exception feature from the context.
                    var contextFeature = context.Features.Get<IExceptionHandlerFeature>();

                    if (contextFeature != null)
                    {
                        // Logs the exception message using the logger.
                        logger.LogError(contextFeature.Error.Message);

                        // Creates a BaseResponse object to return as the response.
                        var response = new BaseResponse<object>
                        {
                            IsSuccessful = false,
                            Message = contextFeature.Error.Message,
                        };

                        // Serializes the BaseResponse object to JSON and writes it to the response.
                        await context.Response.WriteAsync(JsonSerializer.Serialize(response));
                    }
                });
            });
        }
    }
}

