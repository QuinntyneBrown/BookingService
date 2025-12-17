# Migration Guide: .NET Framework 4.6.2 to .NET 9

This document outlines the migration process from .NET Framework 4.6.2 to .NET 9 for the BookingService solution.

## Summary

This is a major architectural migration that involves:
- Migrating from OWIN to ASP.NET Core middleware pipeline
- Migrating from ASP.NET Web API to ASP.NET Core Web API
- Migrating from Entity Framework 6 to Entity Framework Core 9
- Updating all third-party libraries to .NET 9 compatible versions

## Completed Changes

### ✅ Project Files
- [x] Converted all .csproj files to SDK-style format
- [x] Updated TargetFramework to `net9.0`
- [x] Removed packages.config files
- [x] Migrated to PackageReference format

### ✅ Package Updates
- [x] AWSSDK.Core: 3.3.10.1 → 3.7.400.57
- [x] AWSSDK.S3: 3.3.5.10 → 3.7.408
- [x] EntityFramework 6.1.3 → Microsoft.EntityFrameworkCore 9.0.0
- [x] MediatR: 3.0.1 → 12.4.1
- [x] Newtonsoft.Json: 10.0.1 → 13.0.3
- [x] SendGrid: 9.1.0 → 9.29.3
- [x] Swashbuckle.Core 5.3.2 → Swashbuckle.AspNetCore 7.2.0
- [x] WindowsAzure.Storage 8.1.1 → Azure.Storage.Blobs 12.23.0
- [x] Unity: 4.0.1 → 5.11.10 (with Unity.Microsoft.DependencyInjection)
- [x] Added System.Runtime.Caching 9.0.0
- [x] Removed OWIN-specific packages (will be replaced with ASP.NET Core middleware)

### ✅ Code Changes
- [x] Replaced `System.Data.Entity` → `Microsoft.EntityFrameworkCore` 
- [x] Replaced `System.Web.Http` → `Microsoft.AspNetCore.Mvc` in controllers
- [x] Replaced `ApiController` → `ControllerBase`
- [x] Replaced `IHttpActionResult` → `IActionResult`
- [x] Replaced `[RoutePrefix]` → `[Route]`
- [x] Replaced `[FromUri]` → `[FromQuery]`
- [x] Replaced `[ResponseType]` → `[ProducesResponseType]`
- [x] Updated DbContext constructor to use `DbContextOptions<T>`
- [x] Updated MediatR handlers: `IAsyncRequestHandler` → `IRequestHandler`
- [x] Added `CancellationToken` parameter to all Handle methods
- [x] Replaced `Microsoft.Practices.Unity` → `Unity`
- [x] Replaced `Microsoft.WindowsAzure.Storage` → `Azure.Storage.Blobs`

## Remaining Changes Needed

### 🔧 OWIN to ASP.NET Core Middleware (Critical)

The following files need complete rewrites for ASP.NET Core:

#### 1. App.cs / Startup Configuration
**File**: `src/BookingService/App.cs`

**Current**: OWIN-based configuration
```csharp
public class ApiConfiguration
{
    public static void Install(HttpConfiguration config, IAppBuilder app)
    {
        // OWIN middleware setup
    }
}
```

**Needed**: ASP.NET Core Program.cs and Startup.cs pattern
```csharp
// Program.cs
var builder = WebApplication.CreateBuilder(args);

// Add services
builder.Services.AddDbContext<BookingServiceContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("BookingServiceContext")));
builder.Services.AddControllers();
builder.Services.AddSwaggerGen();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options => { /* JWT config */ });
builder.Services.AddSignalR();

// Register Unity or migrate to built-in DI
// Option 1: Use Unity
builder.Host.UseUnityServiceProvider();
// Option 2: Migrate to built-in DI (recommended)

var app = builder.Build();

// Configure middleware pipeline
app.UseSwagger();
app.UseSwaggerUI();
app.UseRouting();
app.UseCors();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.MapHub<NotificationHub>("/signalr");

app.Run();
```

#### 2. Middleware Files
**Files**:
- `src/BookingService/Features/Core/StatusMiddleware.cs`
- `src/BookingService/Features/Core/TenantMiddleware.cs`

**Current**: OWIN middleware with `OwinMiddleware` base class

**Needed**: ASP.NET Core middleware
```csharp
public class TenantMiddleware
{
    private readonly RequestDelegate _next;
    
    public TenantMiddleware(RequestDelegate next)
    {
        _next = next;
    }
    
    public async Task InvokeAsync(HttpContext context)
    {
        // Middleware logic here
        await _next(context);
    }
}

// Register in Program.cs
app.UseMiddleware<TenantMiddleware>();
```

#### 3. Authentication & Authorization
**Files**:
- `src/BookingService/Security/JwtOptions.cs`
- `src/BookingService/Security/JwtWriterFormat.cs`
- `src/BookingService/Security/OAuthOptions.cs`
- `src/BookingService/Security/OAuthProvider.cs`

**Current**: OWIN-based JWT bearer authentication

**Needed**: ASP.NET Core JWT Bearer Authentication
```csharp
// In Program.cs
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = configuration["Jwt:Issuer"],
            ValidAudience = configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(configuration["Jwt:Key"]))
        };
    });
```

### 🔧 Entity Framework 6 to EF Core (Medium Priority)

#### 1. Migration Infrastructure
**File**: `src/BookingService/Data/Migrations/Configuration.cs`

**Issue**: EF6 DbMigrationsConfiguration doesn't exist in EF Core

**Solution**: Remove this file and use EF Core migrations
```bash
# Remove old migrations
# Create new EF Core migrations
dotnet ef migrations add InitialCreate --project src/BookingService
```

#### 2. Entity Configuration
**Files**:
- `src/BookingService/Data/Migrations/UserConfiguration.cs`
- `src/BookingService/Data/Migrations/RoleConfiguration.cs`
- `src/BookingService/Data/Migrations/TenantConfiguration.cs`

**Current**: Inherits from `EntityTypeConfiguration<T>` (EF6)

**Needed**: Implement `IEntityTypeConfiguration<T>` (EF Core)
```csharp
public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("Users");
        builder.HasKey(u => u.Id);
        // Additional configuration
    }
}

// Register in DbContext
protected override void OnModelCreating(ModelBuilder modelBuilder)
{
    modelBuilder.ApplyConfiguration(new UserConfiguration());
    modelBuilder.ApplyConfiguration(new RoleConfiguration());
    modelBuilder.ApplyConfiguration(new TenantConfiguration());
}
```

#### 3. Soft Delete Implementation
**Files**:
- `src/BookingService/Data/Helpers/SoftDeleteAttribute.cs`
- `src/BookingService/Data/Helpers/SoftDeleteInterceptor.cs`
- `src/BookingService/Data/Helpers/SoftDeleteQueryVisitor.cs`

**Issue**: EF6 interceptors and conventions don't exist in EF Core

**Solution**: Use EF Core global query filters
```csharp
// In DbContext.OnModelCreating
foreach (var entityType in modelBuilder.Model.GetEntityTypes())
{
    if (typeof(ISoftDeletable).IsAssignableFrom(entityType.ClrType))
    {
        modelBuilder.Entity(entityType.ClrType)
            .HasQueryFilter(this.BuildSoftDeleteFilter(entityType.ClrType));
    }
}

// Override SaveChanges to handle soft delete
public override int SaveChanges()
{
    foreach (var entry in ChangeTracker.Entries()
        .Where(e => e.State == EntityState.Deleted && e.Entity is ISoftDeletable))
    {
        entry.State = EntityState.Modified;
        ((ISoftDeletable)entry.Entity).IsDeleted = true;
    }
    return base.SaveChanges();
}
```

### 🔧 Other Infrastructure Updates

#### 1. Unity DI Container
**File**: `src/BookingService/UnityConfiguration.cs`

**Decision Needed**: 
- Option A: Keep Unity and use `Unity.Microsoft.DependencyInjection` package
- Option B: Migrate to built-in ASP.NET Core DI (recommended for new .NET projects)

If keeping Unity:
```csharp
// In Program.cs
builder.Host.UseUnityServiceProvider();
```

#### 2. Filter Providers
**File**: `src/BookingService/Features/Core/WebApiUnityActionFilterProvider.cs`

**Issue**: Web API filter providers don't exist in ASP.NET Core

**Solution**: Use ASP.NET Core filter registration
```csharp
builder.Services.AddControllers(options =>
{
    options.Filters.Add<HandleErrorAttribute>();
});
```

#### 3. HTTP Request Extensions
**File**: `src/BookingService/Features/Core/HttpRequestMessageExtensions.cs`

**Issue**: `HttpRequestMessage` is different in ASP.NET Core

**Solution**: Use `HttpContext` instead
```csharp
// Old: request.Properties["key"]
// New: httpContext.Items["key"]
```

### 🔧 SignalR Migration
**Issue**: ASP.NET SignalR to ASP.NET Core SignalR

**Files Affected**: Any hub classes and SignalR configuration

**Solution**:
```csharp
// In Program.cs
builder.Services.AddSignalR();

// In app configuration
app.MapHub<YourHub>("/hubPath");
```

### 🔧 Azure Blob Storage API Changes
**File**: `src/BookingService/Features/DigitalAssets/AzureBlobStorageDigitalAssetCommand.cs`

**Current**: Uses `Microsoft.WindowsAzure.Storage` (deprecated)

**Needed**: Use `Azure.Storage.Blobs`
```csharp
// Old
var storageAccount = CloudStorageAccount.Parse(connectionString);
var blobClient = storageAccount.CreateCloudBlobClient();
var container = blobClient.GetContainerReference(containerName);

// New
var blobServiceClient = new BlobServiceClient(connectionString);
var containerClient = blobServiceClient.GetBlobContainerClient(containerName);
var blobClient = containerClient.GetBlobClient(blobName);
```

## Testing Strategy

After completing the migration:

1. **Build Verification**
   ```bash
   dotnet build BookingService.sln
   ```

2. **Run Tests** (if tests exist)
   ```bash
   dotnet test BookingService.sln
   ```

3. **Database Migration**
   ```bash
   # Create new migrations
   dotnet ef migrations add InitialMigration --project src/BookingService
   
   # Apply to database
   dotnet ef database update --project src/BookingService
   ```

4. **Manual Testing**
   - Start the application
   - Test authentication endpoints
   - Test CRUD operations
   - Test file upload (Azure/S3)
   - Test SignalR connections

## Configuration Changes

### appsettings.json
Create `appsettings.json` to replace `Web.config`:

```json
{
  "ConnectionStrings": {
    "BookingServiceContext": "Server=...;Database=...;..."
  },
  "Jwt": {
    "Key": "your-secret-key",
    "Issuer": "your-issuer",
    "Audience": "your-audience",
    "ExpiryInMinutes": 60
  },
  "AzureBlobStorage": {
    "ConnectionString": "...",
    "ContainerName": "..."
  },
  "AWS": {
    "AccessKey": "...",
    "SecretKey": "...",
    "BucketName": "..."
  },
  "SendGrid": {
    "ApiKey": "..."
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*"
}
```

## Breaking Changes

1. **Async/Await**: EF Core requires async operations in more places
2. **Configuration**: Move from Web.config/App.config to appsettings.json
3. **Dependency Injection**: Built into ASP.NET Core, Unity is optional
4. **Routing**: Attribute routing is required (no conventional routing)
5. **Filters**: Different registration and execution model
6. **Middleware**: Completely different pipeline model

## Estimated Effort

- **OWIN → ASP.NET Core**: 8-16 hours
- **EF6 → EF Core**: 4-8 hours  
- **Authentication**: 4-6 hours
- **Testing & Bug Fixes**: 8-16 hours
- **Total**: 24-46 hours

## Resources

- [Migrate from ASP.NET to ASP.NET Core](https://docs.microsoft.com/en-us/aspnet/core/migration/proper-to-2x/)
- [EF6 to EF Core Migration](https://docs.microsoft.com/en-us/ef/efcore-and-ef6/porting/)
- [ASP.NET Core Authentication](https://docs.microsoft.com/en-us/aspnet/core/security/authentication/)
- [ASP.NET Core Middleware](https://docs.microsoft.com/en-us/aspnet/core/fundamentals/middleware/)
