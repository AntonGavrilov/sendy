# Sendy

**Sendy** is a small, lightweight .NET library for implementing the **CQRS** pattern (Command Query Responsibility Segregation).  
It enables clean separation between write and read operations with minimal ceremony, and supports both synchronous and asynchronous handlers.
A simple, fast alternative to MediatR — no magic, just CQRS
---

## 🚀 Installation

Install via the .NET CLI:

```bash
dotnet add package Sendy
```

Or download it from [NuGet.org](https://www.nuget.org/packages/Sendy)

---

## ⚙️ Configuration

Register Sendy in your `Startup.cs` or `Program.cs`:

```csharp
public void ConfigureServices(IServiceCollection services)
{
    services.AddSendy(typeof(Program).Assembly);
}
```

---

## 🧱 Defining Commands & Queries

Implement one of the following interfaces:

| Purpose        | Interface                        |
|----------------|----------------------------------|
| Command        | `IRequest`, `IAsyncRequest`       |
| Query (with result) | `IRequest<TResult>`, `IAsyncRequest<TResult>` |

### Example: Creating an Order

```csharp
public class CreateOrderCommand : IRequest<Guid>
{
    public string CustomerId { get; set; }
    public List<string> ProductIds { get; set; }
}
```

### Example: Querying Top Products

```csharp
public class GetTopSellingProductsQuery : IAsyncRequest<List<ProductDto>>
{
    public int TopCount { get; set; } = 5;
}
```

---

## 🧰 Implementing Handlers

### Synchronous Command Handler

```csharp
public class CreateOrderHandler : IHandler<CreateOrderCommand, Guid>
{
    public Guid Handle(CreateOrderCommand request)
    {
        // Simulate saving to DB
        var orderId = Guid.NewGuid();
        Console.WriteLine($"Order {orderId} created for Customer {request.CustomerId}");
        return orderId;
    }
}
```

### Asynchronous Query Handler

```csharp
public class GetTopSellingProductsHandler : IAsyncHandler<GetTopSellingProductsQuery, List<ProductDto>>
{
    public async Task<List<ProductDto>> Handle(GetTopSellingProductsQuery request)
    {
        // Simulate async DB call
        await Task.Delay(100);

        return new List<ProductDto>
        {
            new("Product A", 128),
            new("Product B", 115),
            new("Product C", 98)
        }.Take(request.TopCount).ToList();
    }
}
```

---

## ✉️ Dispatching Commands and Queries

### Synchronous

```csharp
var orderId = _sendy.Send(new CreateOrderCommand
{
    CustomerId = "cust-123",
    ProductIds = new List<string> { "prod-1", "prod-2" }
});
```

### Asynchronous

```csharp
var topProducts = await _sendy.SendAsync(new GetTopSellingProductsQuery
{
    TopCount = 3
});
```

---

## 🧪 DTOs Used in Examples

```csharp
public record ProductDto(string Name, int UnitsSold);
```

---

## ✅ Why Sendy?

- 🔹 Clean separation of responsibilities (CQRS)
- 🔹 Supports both sync and async flows
- 🔹 Minimal boilerplate
- 🔹 Testable and modular
- 🔹 Fast to set up and extend

---

## 📄 License

This project is licensed under the MIT License.
