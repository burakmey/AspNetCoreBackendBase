## Domain Layer

The **Domain Layer** defines the core entities and enums that represent the essential data structures of the application. This layer focuses on pure domain models, without any business logic, containing only basic properties and validation rules. Its purpose is to ensure that the core entities are well-defined, maintainable, and isolated from external systems or services. By keeping the **Domain Layer** independent from other layers, it preserves the integrity of the application's core data models.

### Entities

**BaseEntity<TKey> :** Abstract class that serves as the base for all entities in the application. It uses a generic type `TKey` to allow flexibility in defining the type of the entity's identifier (e.g., `int`, `Guid`). This base class provides common properties like `Id`, `CreatedAt`, and `UpdatedAt`, ensuring that each entity has a unique identifier and tracks creation and modification timestamps.

```csharp
public abstract class BaseEntity<TKey>
{
    public abstract TKey Id { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}
```

**User :** Represents an application user by extending `IdentityUser<Guid>`. This class builds upon the basic identity model by adding custom properties for additional user-specific information. It is designed to be customizable to meet specific project needs and requirements.

```csharp
public class User : IdentityUser<Guid>
{
    public required string Name { get; set; }
    public required string Surname { get; set; }
    public string? RefreshToken { get; set; }
    public DateTime? RefreshTokenExpiration { get; set; }
}
```

**Role :** Represents a role in the system by extending `IdentityRole<Guid>`. This class extends the basic role model to include additional functionality for managing `Role` and `Endpoint` entity relation. The `RoleEndpoints` property represents the collection of associations between this role and endpoints that are authorized for this role.

```csharp
public class Role : IdentityRole<Guid>
{
    public Role(string roleName) : base(roleName)
    {
        Id = Guid.NewGuid();
        Name = roleName;
        NormalizedName = roleName;
    }
    public override string? Name
    {
        get => base.Name;
        set => base.Name = value ?? throw new ArgumentNullException(nameof(Name), "Role name cannot be null.");
    }
    public override string? NormalizedName
    {
        get => base.NormalizedName;
        set => base.NormalizedName = value?.ToUpper() ?? throw new ArgumentNullException(nameof(NormalizedName), "Normalized role name cannot be null.");
    }
    public ICollection<RoleEndpoint> RoleEndpoints { get; set; } = [];
}
```

**And More** Additional entities can be added to meet specific needs of the application. These entities help to cover other important aspects of the system and adapt to different requirements.

### Enums

**StorageType :** Represents storage solutions, such as `AWS`, `Azure`, and `Local`. It helps configure and select the right storage service. It can also be used to seed storage types into the database, ensuring a consistent set of options.

**ExtensionType :** Represents supported file extensions, such as `jpeg`, `png`, `docx`, and `pdf`. It helps validate file types and ensures only supported formats are processed. It can also be used to seed these extensions into the database, maintaining a consistent set of acceptable file types.

**RouteType :** Categorizes different types of routes (Controllers), such as `Auth`, `EndpointAuthorization`, `Role`, and `User`. It helps manage access permissions and security for routes. It can also be used to seed route types into the database, ensuring consistent route management.

**And More :** Other enums can be added to represent additional categories or statuses in the application. They help seed data into the database, ensuring consistency and proper organization.
