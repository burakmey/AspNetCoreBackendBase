# Presentation Layer

The **Presentation Layer** manages interactions with external users or clients through web applications, APIs, or other interfaces. It processes user inputs and returns outputs but contains no business logic. Instead, it delegates requests to the **Application Layer**, which coordinates operations and communicates with the **Persistence Layer** and **Infrastructure Layer** to execute business logic and perform tasks such as database operations or external API calls.

This separation ensures that the **Presentation Layer** remains focused solely on user interaction and communication, keeping it independent from core business logic, making the system modular, maintainable, and easier to extend.

## Controllers

**Controllers** are responsible for handling incoming HTTP requests and returning the appropriate responses. They act as an interface between the client (e.g., web, mobile, or API consumers) and the internal layers of the application. In this template, controllers inherit from `ControllerBase` and are decorated with attributes to define their routes, behavior, and request handling.

### MediatR Integration

`MediatR` is used to implement the `Mediator Pattern`, which promotes **_Loose Coupling_** between controllers and business logic. **Controllers** send commands or queries to `MediatR` instead of calling service methods directly. `MediatR` then forwards these requests to the appropriate handler, ensuring that **Controllers** remain lightweight and focused on their primary functio handling HTTP requests

`MediatR` is used to implement the `Mediator Pattern`, which promotes **_Loose Coupling_** between controllers and business logic. Instead of calling service methods directly, controllers send commands or queries to `MediatR`, which forwards them to the appropriate handler. This keeps controllers lightweight and focused on their main role that is handling HTTP requests.

#### Example Controller Using MediatR

```csharp
[Route("api/[controller]/[action]")]
[ApiController]
public class AuthController : ControllerBases
{
    readonly IMediator _mediator;
    public AuthController(IMediator mediator)
    {
        _mediator = mediator;
    }
    [HttpPost]
    public async Task<IActionResult> Login(LoginCommandRequest request)
    {
        BaseResponse<LoginCommandResponse> response = await _mediator.Send(request);
        return Ok(response);
    }
}
```

## Global Exception Handling

**Global Exception Handling** ensures that unhandled exceptions are managed consistently across the application, providing clients with a standardized error response. In `ASP.NET Core`, this is typically achieved using middleware that catches exceptions during request processing.

### Exception Handling Middleware

The middleware catches any errors that happen while processing a request and returns a consistent error message to the client. This approach makes error handling easier to manage across the entire application and helps prevent it from crashing due to unexpected errors.

### ExceptionHandlerExtension

In this template, global exception handling is set up using middleware as shown below:

```csharp
public static class ExceptionHandlerExtension
{
    public static void ConfigureExceptionHandler<T>(this WebApplication app)
    {
        ILogger<T> logger = app.Services.GetRequiredService<ILogger<T>>();
        app.UseExceptionHandler(builder =>
        {
            builder.Run(async context =>
            {
                context.Response.ContentType = MediaTypeNames.Application.Json;
                var contextFeature = context.Features.Get<IExceptionHandlerFeature>();
                if (contextFeature != null)
                {
                    logger.LogError(contextFeature.Error.Message);
                    var response = new BaseResponse<object>
                    {
                        IsSuccessful = false,
                        Message = contextFeature.Error.Message,
                    };
                    await context.Response.WriteAsync(JsonSerializer.Serialize(response));
                }
            });
        });
    }
}
```

### Integrating Middleware in Program.cs

To activate global exception handling, the middleware is added to the Program.cs file:

```csharp
public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        //...
        var app = builder.Build();
        //...
        app.ConfigureExceptionHandler<Program>();
        //...
        app.Run();
    }
}
```

## Authorization

In `ASP.NET Core`, authentication is essential for securing applications. The `JwtBearer` authentication scheme is widely used to handle `JSON Web Tokens` (JWT), which are used to authenticate users in modern web applications.

### Integrating JWT Bearer Authentication in Program.cs

To set up `JWT Bearer` authentication, configure the authentication middleware in the Program.cs file. The example below demonstrates how to configure:

```csharp
public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        //...
        builder.Services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer()
        .AddJwtBearer("User")
        .AddJwtBearer("Admin");
        // Adding schemes

        builder.Services.ConfigureOptions<JwtBearerOptionsSetup>();
        var app = builder.Build();
        //...
        app.Run();
    }
}
```

### Usage of Schemes

In the `JwtBearerOptionsSetup` class, various schemes can be configured for different purposes:

- Default Scheme: The default JWT authentication scheme is used for general authentication tasks.

- Named Schemes: Define custom configurations for specific named schemes like "User" and "Admin" to handle different authentication needs or roles.

### Configuring JwtBearerOptionsSetup

The `JwtBearerOptionsSetup` class is responsible for configuring the `JwtBearerOptions` used in JWT authentication. This setup allows for different configurations based on named options.

```csharp
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
        //...
    }

    public void Configure(JwtBearerOptions options)
    {
        options.TokenValidationParameters = _tokenOptions.TokenValidationParameters;
        options.Events = new JwtBearerEvents
        {
            // This handles the 401 Unauthorized challenge
            OnChallenge = async context =>
            {
                context.HandleResponse();
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                context.Response.ContentType = "application/json";
                var result = JsonConvert.SerializeObject(new BaseResponse<object>
                {
                    IsSuccessful = false,
                    Message = "Please check your credentials."
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
        // Custom configurations can be done for scheme "Custom"
        Configure(options);
    }
}
```

### Using the Authorize Attribute for Schemes in Controllers

The `[Authorize]` attribute is used to apply authorization requirements to controllers or actions. By specifying the `AuthenticationSchemes` property, the attribute can restrict access based on different authentication schemes.

In this example, the `UserController` is configured to require authentication using the `User` scheme. Only requests that are authenticated with this scheme will be allowed access to the controller's actions.

```csharp
[Route("api/[controller]")]
[ApiController]
[Authorize(AuthenticationSchemes = "User")]
public class UserController : ControllerBase
{
    //...
}
```

## Role Permission Filter

The **Role Permission Filter** is an action filter that manages role-based authorization for specific endpoints. It ensures that only users with the appropriate roles can access certain actions. The `RolePermissionFilter` verifies user roles and permissions before granting access to a given endpoint.

### Integrating Middleware in Program.cs

To enable the **Role Permission Filter**, configure the necessary services in the Program.cs file and register the filter:

```csharp
public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        //...
        builder.Services.AddPresentationServices();
        //...
        var app = builder.Build();
        //...
        app.Run();
    }
}
```

In the `ServiceRegistration` class, add the `RolePermissionFilter` as a scoped service:

```csharp
public static class ServiceRegistration
{
    public static void AddPresentationServices(this IServiceCollection services)
    {
        services.AddScoped<RolePermissionFilter>();
    }
}
```

### Role-Based Endpoints

`Endpoint` entities in the application are designed to store the roles required for access to endpoint which has `AuthorizeDefinitionAttribute`. Each one is associated with a specific code and route, which are used to validate user permissions.

```csharp
public class Endpoint : BaseEntity<int>
{
    public override int Id { get; set; }
    public required string Code { get; set; }
    public required int RouteId { get; set; }
    public Route Route { get; set; } = null!;
    public ICollection<RoleEndpoint> RoleEndpoints { get; set; } = [];
}
```

### Usage of [AuthorizeDefinition]

The `[AuthorizeDefinition]` attribute is used to specify the roles required for accessing a method. Decorate controller actions with this attribute to enforce role-based authorization.

```csharp
[AttributeUsage(AttributeTargets.Method, Inherited = false, AllowMultiple = false)]
public class AuthorizeDefinitionAttribute(ActionType actionType, string definition, string route) : Attribute
{
    public ActionType ActionType { get; set; } = actionType;
    public string Definition { get; set; } = definition;
    public string Route { get; set; } = route.RemoveControllerSuffix();
}
```

Example of applying `[AuthorizeDefinition]` in a controller:

```csharp
[Route("api/[controller]")]
[ApiController]
[Authorize]
public class UserController : ControllerBase
{
    readonly IMediator _mediator;
    public UserController(IMediator mediator)
    {
        _mediator = mediator;
    }
    [HttpGet("get-user-roles/{UserNameOrUserId}")]
    [AuthorizeDefinition(ActionType.Read, nameof(GetUserRoles),  nameof(UserController))]
    public async Task<IActionResult> GetUserRoles([FromRoute] GetUserRolesQueryRequest request)
    {
        BaseResponse<GetUserRolesQueryResponse> response = await _mediator.Send(request);
        return Ok(response);
    }
}
```
