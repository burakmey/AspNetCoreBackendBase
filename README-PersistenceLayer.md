## Persistence Layer

The **Persistence Layer** manages all database-related operations. It handles tasks such as database configurations, data access, migrations, and data seeding. This layer includes the concrete implementations of repository interfaces defined in the **Application Layer**, ensuring that database interactions are separated from the business logic. By isolating database concerns, the application maintains a clean architecture, enabling flexibility and ease of maintenance, similar to the **_Separation of Concerns_** seen in the **Infrastructure Layer**.

### DbContext

The `DbContext` class in **Entity Framework Core** (EF Core) is responsible for managing database interactions. It serves as a bridge between the application's domain entities and the database, handling tasks like CRUD (Create, Read, Update, Delete) operations and tracking changes in entity states to ensure data consistency.

In the template project, the `AspNetCoreBackendBaseDbContext` class inherits from `IdentityDbContext`, a specialized version of `DbContext` provided by **ASP.NET Core Identity**, to manage user roles and authentication.

**DbSets :** Each `DbSet<T>` represents a collection of entities that can be queried and persisted to the database.

**OnModelCreating :** The `OnModelCreating` method is overridden to configure entity mappings and relationships. By using `ApplyConfigurationsFromAssembly`, the method automatically scans and applies all configurations from the assembly, ensuring a modular and scalable approach to entity configurations.

**SaveChangesAsync :**

The `SaveChangesAsync` method is overridden to automatically manage timestamp properties (`CreatedAt`, `UpdatedAt`) for entities that inherit from `BaseEntity<T>`. It ensures that:

- **Added** entities have their `CreatedAt` property set to the current UTC time.
- **Modified** entities have their `UpdatedAt` property updated to the current UTC time.

**Identity Integration :** Since the `AspNetCoreBackendBaseDbContext` extends `IdentityDbContext`, it includes pre-configured tables and logic for handling users, roles, and authentication features without needing extra configurations.

```csharp
 public class AspNetCoreBackendBaseDbContext : IdentityDbContext<User, Role, Guid>
 {
    public DbSet<Extension> Extensions { get; set; }
    public DbSet<File> Files { get; set; }
    public DbSet<Storage> Storages { get; set; }
    public DbSet<Endpoint> Endpoints { get; set; }
    public DbSet<Route> Routes { get; set; }
    public DbSet<RoleEndpoint> RoleEndpoints { get; set; }
    // Additional DbSet properties for domain entities can be added here as needed

    public AspNetCoreBackendBaseDbContext(DbContextOptions<AspNetCoreBackendBaseDbContext> options) : base(options)
    {
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        base.OnModelCreating(modelBuilder);
    }
    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var datas = ChangeTracker.Entries<BaseEntity<object>>();
        foreach (var data in datas)
        {
            _ = data.State switch
            {
                EntityState.Added => data.Entity.CreatedAt = DateTime.UtcNow,
                EntityState.Modified => data.Entity.UpdatedAt = DateTime.UtcNow,
                _ => DateTime.UtcNow
            };
        }
        return await base.SaveChangesAsync(cancellationToken);
    }
 }
```

### Configurations

In **Entity Framework Core** (EF Core), configurations define how domain entities map to the underlying database schema. This includes specifying relationships, keys, column properties, and constraints. EF Core uses the `IEntityTypeConfiguration<T>` interface to separate entity configuration from the domain models, promoting a clean **_Separation of Concerns_**.

#### Applying Configurations

Configurations are applied in the `OnModelCreating` method of the `DbContext` class. This method utilizes the **_Fluent API_** to configure the entity models, ensuring that the database schema aligns with the application’s needs. In the template project, the `ApplyConfigurationsFromAssembly` method is used to automatically load all configuration classes that implement `IEntityTypeConfiguration<T>` from the assembly:

```csharp
protected override void OnModelCreating(ModelBuilder modelBuilder)
{
    modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    base.OnModelCreating(modelBuilder);
}
```

This method scans the current assembly for entity configurations and applies them, streamlining the setup of entity mappings.

#### Example Configuration

```csharp
public class RoleEndpointConfigurations : IEntityTypeConfiguration<RoleEndpoint>
{
    public void Configure(EntityTypeBuilder<RoleEndpoint> builder)
    {
        builder.Ignore(re => re.UpdatedAt);
        builder.Ignore(re => re.Id);
        builder.HasKey(re => new { re.RoleId, re.EndpointId });

        builder.HasOne(re => re.Role)
                .WithMany(r => r.RoleEndpoints)
                .HasForeignKey(re => re.RoleId)
                .IsRequired();

        builder.HasOne(re => re.Endpoint)
                .WithMany(e => e.RoleEndpoints)
                .HasForeignKey(re => re.EndpointId)
                .IsRequired();
    }
}
```

### Migrations

Migrations in **Entity Framework Core** (EF Core) are a mechanism for managing changes to the database schema over time. They facilitate evolving the database schema as the application's data model changes, ensuring that the database structure stays in sync with the domain model.

- **Migration :** A migration represents a set of changes to the database schema. Each migration contains code to apply or revert these changes.
- **Migration History Table :** EF Core uses a table in the database to track applied migrations. This table helps EF Core determine which migrations need to be applied or rolled back.

#### Creating Migrations

Migrations can be created using the **EF Core CLI** or the **Package Manager Console** (PMC) in **Visual Studio**. These commands generate migration files that outline the changes to be applied to the database.

- **EF Core CLI**

```bash
# Creating a Migration
dotnet ef migrations add <MigrationName>

# Applying Migrations
dotnet ef database update

# Reverting Migrations
dotnet ef database update <PreviousMigration>
```

- **Package Manager Console**

```powershell
# Creating a Migration
Add-Migration <MigrationName>

# Applying Migrations
Update-Database

# Reverting Migrations
Update-Database <PreviousMigration>

```

### DesignTimeDbContextFactory

The `DesignTimeDbContextFactory` class is used by **Entity Framework Core** tools to create instances of `AspNetCoreBackendBaseDbContext` at design time. This is particularly useful for performing tasks such as running migrations and seeding the database. The factory ensures that the `DbContext` is correctly configured with the appropriate settings for design-time operations.

#### CreateDbContext

This method creates a new instance of `AspNetCoreBackendBaseDbContext` with design-time configuration. It also handles optional seeding of the database based on command-line arguments.

#### Seeding the Database

To seed the database, use the --seed argument with migration or update commands. This triggers the seeding process if the database is up-to-date and all pending migrations have been applied. The --seed argument can be used with both migration and update commands.

```bash
dotnet ef migrations add <MigrationName> --seed
# or
dotnet ef database update --seed
```

### Seed Services

Seed services in **Entity Framework Core** (EF Core) are used to populate the database with initial data. They ensure that when a database is first created or reset, essential data such as configuration values, predefined roles, or static content is automatically inserted.

Seed services in **Entity Framework Core** (EF Core) are used to populate the database with initial data. This ensures that essential data, such as configuration values, predefined roles, or static content, is automatically inserted when the database is first created or reset.

#### How Seed Services Work

Seed services implement the `ISeedService<T, TKey>` interface, which includes the `SeedAsync()` method for seeding data. The service logic checks for existing data and adds new entries if necessary, allowing the seeding process to be rerun without creating duplicate records.

The common functionality for seeding, such as logging and database interactions, is handled by the `BaseSeederService<T, TKey>` class. Concrete seed services inherit from this abstract class and focus on entity-specific details.

#### Execution of Seed Services

Seed services are invoked during application startup or as part of a migration process. In this template project, they are called within the `DesignTimeDbContextFactory` when the --seed parameter is provided in the command line.

Concrete seed services inherit from the `BaseSeederService<T, TKey>` class, implementing the `ISeedService<T, TKey>` interface for seeding entities derived from `BaseEntity<TKey>`.

```csharp
static async Task SeedDatabase(AspNetCoreBackendBaseDbContext context)
{
    using var transaction = await context.Database.BeginTransactionAsync();
    try
    {
        var routeSeederService = new RouteSeederService(context);
        var storageSeederService = new StorageSeederService(context);
        var extensionSeederService = new ExtensionSeederService(context);
        var endpointSeederService = new EndpointSeederService(context);

        await storageSeederService.SeedAsync();
        await routeSeederService.SeedAsync();
        await extensionSeederService.SeedAsync();
        await endpointSeederService.SeedAsync();

        await transaction.CommitAsync();
    }
    catch (Exception ex)
    {
        await transaction.RollbackAsync();
        throw new Exception("An error occurred during seeding: " + ex.Message);
    }
}
```

### Repositories

In the **Persistence Layer**, concrete implementations of repository interfaces are provided. These implementations interact directly with the database and follow the interfaces defined in the **Application Layer**. This ensures a clear separation between data access and business logic.

#### Concrete Repository

Each concrete repository is responsible for managing data access for a specific entity. These repositories implement the interfaces like `IReadRepository<T, TKey>` and `IWriteRepository<T, TKey>` from the **Application Layer**, providing the actual code for performing CRUD (Create, Read, Update, Delete) operations. By following these interfaces, they ensure that all data operations are handled correctly and consistently.

Example for the `Endpoint` entity repository:

- ReadRepository:

```csharp
public class EndpointReadRepository : ReadRepository<Endpoint, int>, IEndpointReadRepository
{
    public EndpointReadRepository(AspNetCoreBackendBaseDbContext context) : base(context)
    {
    }
}
```

- WriteRepository:

```csharp
public class EndpointWriteRepository : WriteRepository<Endpoint, int>, IEndpointWriteRepository
{
    public EndpointWriteRepository(AspNetCoreBackendBaseDbContext context) : base(context)
    {
    }
}
```

### Services

In the **Persistence Layer**, concrete services are responsible for implementing database-related operations. These services provide specific implementations for the interfaces defined in the **Application Layer** and handle tasks such as querying and manipulating data within the database. Unlike services in the **Infrastructure Layer**, which manage interactions with external systems, these services focus exclusively on database operations and data management.

#### Concrete Service

Concrete services manage data access and manipulation by integrating with repositories. They perform tasks such as querying and updating entities, ensuring a unified and consistent approach to data management within the application.
The `EndpointAuthorizationService` demonstrates how concrete services operate within the **Persistence Layer**:

```csharp
public class EndpointAuthorizationService : IEndpointAuthorizationService
{
    readonly IEndpointReadRepository _endpointReadRepository;
    readonly IEndpointWriteRepository _endpointWriteRepository;
    readonly RoleManager<Role> _roleManager;

    public EndpointAuthorizationService(IEndpointReadRepository endpointReadRepository, IEndpointWriteRepository endpointWriteRepository, RoleManager<Role> roleManager)
    {
        _endpointReadRepository = endpointReadRepository;
        _endpointWriteRepository = endpointWriteRepository;
        _roleManager = roleManager;
    }

    public async Task<BaseResponse<object>> AssignRolesToEndpointAsync(AssignRolesToEndpointCommandRequest model)
    {
        //...
        await _endpointWriteRepository.SaveAsync();
        return new BaseResponse<object>
        {
            IsSuccessful = true,
            Message = "Roles assigned successfully."
        };
    }
    public async Task<BaseResponse<GetRolesForEndpointQueryResponse>> GetRolesForEndpointAsync(GetRolesForEndpointQueryRequest model)
    {
        Expression<Func<Endpoint, bool>> condition = e => e.Code == model.Code && e.Route!.Name == model.Route;
        IQueryable<Endpoint> query = _endpointReadRepository.GetWhere(condition, tracking: false)
            .Include(e => e.Route)
            .Include(e => e.RoleEndpoints)
            .ThenInclude(re => re.Role);
        Endpoint? endpoint = await query.FirstOrDefaultAsync();
        //...
        return new BaseResponse<GetRolesForEndpointQueryResponse>
        {
            IsSuccessful = true,
            Data = new() { Roles = roles }
        };
    }
}
```
