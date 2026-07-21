# DataToolkit.Bootstrap

**DataToolkit.Bootstrap** is a lightweight, convention-based dependency injection registration library for .NET.

Instead of manually registering every service in `Program.cs`, Bootstrap discovers implementations, reads their registration metadata, and registers them automatically.

Designed with **performance**, **simplicity**, and **maintainability** in mind.

---

# Features

* Convention-based service registration
* Automatic assembly scanning
* Interface registration
* Optional self registration
* Service priorities
* Scoped, Singleton and Transient lifetimes
* Minimal reflection
* No LINQ in the discovery pipeline
* Low memory allocations
* Diagnostic console output
* Zero configuration files

---

# Why Bootstrap?

Traditional registration:

```csharp
services.AddScoped<IUserService, UserService>();
services.AddScoped<IClientService, ClientService>();
services.AddScoped<IProviderService, ProviderService>();
services.AddScoped<IOrderService, OrderService>();
services.AddScoped<IInvoiceService, InvoiceService>();
```

With Bootstrap:

```csharp
builder.Services.AddBootstrap(
(
    typeof(Application.AssemblyReference).Assembly,
    "Application",
    "Services"
));
```

Bootstrap discovers every service automatically.

---

# Installation

Reference the library from your project.

```xml
<ProjectReference Include="DataToolkit.Bootstrap.csproj" />
```

or install it from NuGet (when available).

---

# Getting Started

## 1. Register Bootstrap

```csharp
builder.Services.AddBootstrap(
(
    typeof(Application.AssemblyReference).Assembly,
    "Application",
    "Services"
));
```

Each module is defined by:

* Assembly
* Root namespace
* Target namespace

---

## 2. Create a service

```csharp
public interface IUserService
{
}
```

```csharp
public sealed class UserService : IUserService
{
    public static KeyValuePair<string, string>[] ConfigureServices() =>
    [
        new("Lifetime", "Scoped")
    ];
}
```

Bootstrap will automatically register:

```text
IUserService → UserService
```

---

# ConfigureServices

Every service can optionally expose a static method named:

```csharp
ConfigureServices()
```

Example:

```csharp
public static KeyValuePair<string, string>[] ConfigureServices() =>
[
    new("Lifetime", "Singleton"),
    new("Priority", "100"),
    new("AsSelf", "true")
];
```

Supported options:

| Key      | Description                            |
| -------- | -------------------------------------- |
| Lifetime | Scoped, Singleton or Transient         |
| Priority | Registration order                     |
| AsSelf   | Registers the implementation as itself |
| Exclude  | Prevents registration                  |

---

# Lifetimes

Scoped

```csharp
new("Lifetime", "Scoped")
```

Singleton

```csharp
new("Lifetime", "Singleton")
```

Transient

```csharp
new("Lifetime", "Transient")
```

---

# Register as Self

```csharp
public static KeyValuePair<string, string>[] ConfigureServices() =>
[
    new("AsSelf", "true")
];
```

Result:

```text
IUserService → UserService

UserService → UserService
```

---

# Priority

Lower values are registered first.

```csharp
new("Priority", "10")
```

Default:

```text
1000
```

---

# Exclude

Skip registration completely.

```csharp
new("Exclude", "true")
```

Useful for abstract implementations, prototypes or services registered manually.

---

# Console Diagnostics

Bootstrap prints a registration summary during startup.

Example:

```text
» STARTING: DataToolkit Bootstrap

🧩 IUserService -> UserService (Scoped)
🧩 IProviderService -> ProviderService (Scoped)
🧩 IClientService -> ClientService (Scoped)

--------------- Summary ----------------
Registered : 3
Skipped    : 0
Elapsed    : 2.24 ms
----------------------------------------
```

---

# Design Goals

Bootstrap was created with the following principles:

* Keep the API simple.
* Prefer conventions over configuration.
* Minimize startup overhead.
* Reduce reflection cost.
* Avoid unnecessary allocations.
* Produce deterministic registrations.
* Keep the library easy to understand and maintain.

---

# Requirements

* .NET 9 or later

---

# License

Copyright © DataToolkit [Fernando Poveda].

License information to be defined.
