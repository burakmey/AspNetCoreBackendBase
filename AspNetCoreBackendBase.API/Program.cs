using AspNetCoreBackendBase.API.Extensions;
using AspNetCoreBackendBase.Application;
using AspNetCoreBackendBase.Persistence;
using AspNetCoreBackendBase.Infrastructure;
using AspNetCoreBackendBase.Infrastructure.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using NpgsqlTypes;
using Serilog.Sinks.PostgreSQL;
using Serilog;
using Serilog.Core;
using AspNetCoreBackendBase.API.Serilog;
using Microsoft.AspNetCore.HttpLogging;
using AspNetCoreBackendBase.API.OptionSetups;
using Serilog.Context;

namespace AspNetCoreBackendBase.API;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddCors(options =>
        {
            options.AddDefaultPolicy(policy =>
            {
                policy.WithOrigins(Configuration.GetOrigin).AllowAnyMethod().AllowAnyHeader().AllowCredentials();
            });
        });

        Logger log = new LoggerConfiguration()
            .MinimumLevel.Information()
            .WriteTo.Console()
            .WriteTo.File("Serilog/logs/log.txt", rollingInterval: RollingInterval.Day)
            .WriteTo.PostgreSQL(Configuration.GetConnectionStringPostgresql, "logs", needAutoCreateTable: true, columnOptions: new Dictionary<string, ColumnWriterBase>
            {
                { "message", new RenderedMessageColumnWriter(NpgsqlDbType.Text) },
                { "message_template", new MessageTemplateColumnWriter(NpgsqlDbType.Text) },
                { "level", new LevelColumnWriter(true , NpgsqlDbType.Varchar) },
                { "time_stamp", new TimestampColumnWriter(NpgsqlDbType.Timestamp) },
                { "exception", new ExceptionColumnWriter(NpgsqlDbType.Text) },
                { "log_event", new LogEventSerializedColumnWriter(NpgsqlDbType.Json) },
                { "user_name", new UserNameColumnWriter(Configuration.GetSerilogPropertyUserName) }
            }).Enrich.FromLogContext().CreateLogger();

        builder.Host.UseSerilog(log);

        builder.Services.AddHttpLogging(logging =>
        {
            logging.LoggingFields = HttpLoggingFields.All;
            logging.RequestHeaders.Add("sec-ch-ua");
            logging.MediaTypeOptions.AddText("application/javascript");
            logging.RequestBodyLogLimit = 4096;
            logging.ResponseBodyLogLimit = 4096;
        });

        builder.Services.AddInfrastructureServices();

        builder.Services.AddPersistenceServices();

        builder.Services.AddApplicationServices();

        builder.Services.AddPresentationServices();

        builder.Services.AddStorage<AzureStorageService>();

        builder.Services.AddControllers().ConfigureApiBehaviorOptions(options => options.SuppressModelStateInvalidFilter = true);

        builder.Services.AddAuthentication(options =>
        {
            //DefaultAuthenticateScheme: This specifies the default scheme to use when authentication
            //is required but not explicitly defined.In this case, it's JwtBearerDefaults.AuthenticationScheme.
            // Set JWT as the default scheme
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            //DefaultChallengeScheme: This specifies the scheme used when the app challenges the user
            //(e.g., if an unauthenticated user tries to access a protected resource).
            // Set JWT for challenges
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer() //.AddJwtBearer(JwtBearerDefaults.AuthenticationScheme);
        .AddJwtBearer("User")
        .AddJwtBearer("Admin");

        builder.Services.ConfigureOptions<JwtBearerOptionsSetup>();

        //builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        //.AddScheme<AuthenticationSchemeOptions, CustomAuthenticationHandler>("CustomScheme", null);


        // Configure policies
        //builder.Services.AddAuthorization();
        //builder.Services.ConfigureOptions<AuthorizationOptionsSetup>();

        builder.Services.AddEndpointsApiExplorer();

        var app = builder.Build();

        app.ConfigureExceptionHandler<Program>();

        app.UseHttpsRedirection();

        app.UseCors();

        app.UseRouting();

        app.UseAuthentication();

        app.UseAuthorization();

        app.Use(async (context, next) =>
        {
            var username = context.User?.Identity?.IsAuthenticated != null ? context.User.Identity.Name : null;
            LogContext.PushProperty("user_name", username);
            await next();
        });

        app.MapControllers();

        app.Run();
    }
}
