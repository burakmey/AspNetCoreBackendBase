## Application Layer

The **Application Layer** manages the interactions between the core domain and services. It uses abstractions like interfaces to define how services and repositories should work. This layer keeps the **Domain Layer** independent from the **Infrastructure Layer** and **Persistence Layer**, making the system more flexible and easier to maintain.

### CQRS Pattern

`Command Query Responsibility Segregation (CQRS)` is a design pattern that separates the reading and writing of data into distinct models. This separation enhances performance, security, and scalability.

**Command :** Commands are used to change the state of the application. They represent actions that alter data and typically return feedback on the result, such as whether the operation was successful or if there were any errors.

**Query :** Queries are used to retrieve data without changing the state of the application. They represent requests for information and return data based on the specified criteria, providing results directly from the query.

**Example :** `LoginCommandRequest`, represents the command to perform a login operation. `LoginCommandHandler`, processes the command request and interacts with the authentication service. `LoginCommandResponse` contains the result of the login operation.

```csharp
public class LoginCommandRequest : IRequest<BaseResponse<LoginCommandResponse>>
{
    public required string UsernameOrEmail { get; set; }
    public required string Password { get; set; }
}
```

```csharp
public class LoginCommandHandler : IRequestHandler<LoginCommandRequest, BaseResponse<LoginCommandResponse>>
{
    readonly IInternalAuthentication _authService;
    public LoginCommandHandler(IInternalAuthentication authService)
    {
        _authService = authService;
    }
    public async Task<BaseResponse<LoginCommandResponse>> Handle(LoginCommandRequest request, CancellationToken cancellationToken)
    {
        return await _authService.LoginAsync(request);
    }
}
```

```csharp
public class LoginCommandResponse
{
    public required Token Token { get; set; }
}
```

### Mediator Pattern

The `Mediator Pattern` is a design pattern where a central object, called the mediator, handles communication between different objects. Instead of objects interacting directly with each other, they send their requests to the mediator. This approach promotes **_Loose Coupling_** by centralizing interactions, making the system more manageable and flexible.

`MediatR` is a .NET library that implements the `Mediator Pattern`. It helps streamline communication within an application by sending commands and queries through a central mediator. This keeps components decoupled and simplifies code management. The pattern supports **_Separation of Concerns_** by allowing controllers to pass requests to handlers, which handle the actual processing.

```csharp
[Route("api/[controller]/[action]")]
[ApiController]
public class AuthController : ControllerBase
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

**Request Handling :** The controller's action method receives an HTTP request (e.g., `POST` or `GET`). Instead of handling business logic directly, the controller sends a command or query using `MediatR`.

**Sending Commands/Queries :** Inside the controller, the request object (like `LoginCommandRequest`) is sent to `MediatR` through the `_mediator.Send()` method. This method then calls the appropriate handler for that command or query.

**Handler Processing :** The command handler (e.g., `LoginCommandHandler`) processes the request. It contains the business logic or calls services (such as an authentication service) to perform the requested action.

**Response :** After processing, the handler returns a response object (like `LoginCommandResponse`), which the controller then sends back to the client.

### DTOs (Data Transfer Objects)

In the **Application Layer**, `DTOs` are used to transfer data between different parts of the system, such as between the service layer and the client (API consumers). DTOs ensure that only the necessary data is exposed externally, maintaining a clear separation between internal domain models and the data shared with external clients.

DTOs are simple classes designed to carry data. They consist of properties that represent the specific data being transferred but do not include business logic or behavior. Their main role is to structure and control the movement of data across different layers and boundaries within the application.

**Example :** The `BaseResponse<T>` class is a generic DTO used to standardize responses across the application. It wraps the results of an operation and provides context about its success or failure. This approach ensures a consistent format for handling responses.

```csharp
public class BaseResponse<T>
{
    public bool IsSuccessful { get; set; }
    public T? Data { get; set; }
    public string? Message { get; set; }
}
```

- **IsSuccessful :** A `bool` that indicates whether the operation was successful. This helps API or service consumers quickly understand the outcome without needing to inspect the data itself.

- **Data :** Contains the result of the operation. The type of `Data` is determined by the generic type `T`, which can be any object, such as a DTO, a list, or `null` if no data is available.

- **Message :** Provides additional information about the operation, such as error messages or success notifications. This field is used to offer context or details about the result.

### Repository Pattern

The **Repository Pattern** is a design pattern that abstracts data access, keeping business logic separate from data management. This pattern promotes modularity, testability, and easier maintenance by defining a consistent interface for data operations while isolating data access logic from the rest of the application.

In the application, three main types of repository interfaces are used to manage data operations:

**IRepository :** This generic interface defines a standard way to access a `DbSet<T>` for CRUD operations. It serves as the base for repositories handling entities of type `BaseEntity<TKey>` with a key of type `TKey`.

```csharp
public interface IRepository<T, TKey> where T : BaseEntity<TKey>
{
    DbSet<T> Table { get; }
}
```

**IReadRepository :** Focuses on reading data (GET operations). It provides methods for querying and retrieving data from the database.

```csharp
public interface IReadRepository<T, TKey> : IRepository<T, TKey> where T : BaseEntity<TKey>
{
    IQueryable<T> GetAll(bool tracking = true);
    IQueryable<T> GetWhere(Expression<Func<T, bool>> method, bool tracking = true);
    Task<T?> GetSingleAsync(Expression<Func<T, bool>> method, bool tracking = true);
    Task<T?> GetByIdAsync(TKey id, bool tracking = true);
}
```

**IWriteRepository :** Focuses on modifying data (CREATE, UPDATE, DELETE operations). It provides methods for adding, updating, and removing entities.

```csharp
public interface IWriteRepository<T, TKey> : IRepository<T, TKey> where T : BaseEntity<TKey>
{
    Task<bool> AddAsync(T model);
    Task<bool> AddRangeAsync(List<T> models);
    bool Remove(T model);
    bool RemoveRange(List<T> models);
    Task<bool> RemoveAsync(TKey id);
    bool Update(T model);
    Task<int> SaveAsync();
}
```

### Services

**Service Interfaces** in the **Application Layer** define the expected behaviors for various services within the application. They outline the operations and actions that services should support, but they do not provide implementation details. The actual implementation of these services is handled in the **Infrastructure Layer** and **Persistence Layer**. This separation ensures that business logic remains distinct from implementation details, which improves code organization and makes it easier to manage and test.

**Example :** The `IAuthService` interface includes methods related to authentication. It combines multiple authentication-related interfaces (`IExternalAuthentication` and `IInternalAuthentication`) into a single service interface.
`IExternalAuthentication` defines methods for external authentication providers like Google or Facebook. `IInternalAuthentication` defines methods for internal authentication, such as login and token refresh.

```csharp
public interface IAuthService : IExternalAuthentication, IInternalAuthentication
{
    Task<BaseResponse<object>> ResetPasswordAsync(ResetPasswordCommandRequest model);
    Task<BaseResponse<object>> VerifyResetTokenAsync(VerifyResetTokenCommandRequest model);
    Task UpdateRefreshTokenAsync(string refreshToken, User user, DateTime accessTokenExpiration);
}
```

```csharp
public interface IExternalAuthentication
{
    Task<BaseResponse<LoginGoogleCommandResponse>> GoogleLoginAsync(LoginGoogleCommandRequest model);
    // Additional external authentication methods can be added here
}
```

```csharp
public interface IInternalAuthentication
{
    Task<BaseResponse<LoginCommandResponse>> LoginAsync(LoginCommandRequest model);
    Task<BaseResponse<RefreshCommandResponse>> RefreshAsync(RefreshCommandRequest model);
}
```
