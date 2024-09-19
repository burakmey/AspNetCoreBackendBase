## Infrastructure Layer

The **Infrastructure Layer** handles services and processes that are external to the core application, except for database-related tasks. It deals with interactions like sending emails, sending SMS messages, processing payments, handling file storage (e.g., Firebase, AWS, Azure), and generating tokens (such as JWT or OAuth). This layer allows the application to communicate with external systems, such as cloud providers or third-party APIs, without mixing these processes with business logic. By keeping these external integrations in a separate layer, the application remains flexible, modular and maintains a clean **_Separation of Concerns_**.

### Services

In the **Infrastructure Layer**, concrete services implement the interfaces defined in the **Application Layer**. These services handle the actual operations required to interact with external systems, such as sending emails, processing payments, or managing file storage.

Registration of **Infrastructure Layer** services to **_Inversion of Control_** (IoC) container:

```csharp
public static class ServiceRegistration
{
    public static void AddInfrastructureServices(this IServiceCollection services)
    {
        services.AddScoped<IStorageService, StorageService>();
        services.AddScoped<ITokenService, TokenService>();
        services.AddScoped<IMailService, MailService>();
        services.Configure<TokenOptions>(Configuration.GetConfigurationRoot.GetSection(TokenOptions.Options));
    }
}
```

Integrating middleware for **Infrastructure Layer** services in Program.cs:

```csharp
public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        //...
        builder.Services.AddInfrastructureServices();
        //...
        var app = builder.Build();
        //...
        app.Run();
    }
}
```

#### Mail Service

Implements the `IMailService` interface from the **Application Layer**, providing the logic for sending emails through external providers like SMTP or third-party services. This ensures that the business logic can send emails without needing to manage the details of the email provider directly.

#### Storage Service

Implements the `IStorageService` interface from the **Application Layer**. It connects the application to different storage systems like AWS, Azure, or local storage, handling file operations according to the chosen storage type.

- Storage Type: The `StorageService` identifies the type of storage system being used based on the `StorageType` enum. This allows it to interact with the correct storage provider.

- File Management: It handles file operations such as uploading, deleting, and listing files by using the appropriate storage implementation.

- Generic Registration: `AddStorage<T>` to register a custom implementation of `IStorage` by specifying the type parameter `T`.

- Storage Type Registration: `AddStorage(StorageType enum)` to register the storage service based on the `StorageType` enum.

```csharp
public static class ServiceRegistration
{
    public static void AddInfrastructureServices(this IServiceCollection services)
    {
        //...
        services.AddStorage<AzureStorageService>();
        // or
        services.AddStorage(StorageType.Azure);
        //...
    }
    public static void AddStorage<T>(this IServiceCollection services) where T : Storage, IStorage
    {
        services.AddScoped<IStorage, T>();
    }
    public static void AddStorage(this IServiceCollection services, StorageType storageType)
    {
        switch (storageType)
        {
            case StorageType.Azure:
                services.AddScoped<IStorage, AzureStorageService>();
                break;
            case StorageType.Local:
                services.AddScoped<IStorage, LocalStorageService>();
                break;
            // Additional cases for other storage types can be added here
            default:
                // No action for unhandled types
                break;
        }
    }
}

```

#### Token Service

Implements the `ITokenService` interface from the **Application Layer**. This service manages token generation and validation for authentication purposes, such as creating JWT tokens. It ensures that tokens used for user authentication are generated and validated according to the application's security requirements, without exposing the underlying details of token management.
