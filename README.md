# ASP.NET Core RESTful API with CRUD and Batch Processing

This project demonstrates a clean architecture implementation of a RESTful API using ASP.NET Core, Entity Framework Core, FluentValidation, AutoMapper, and async batch processing capabilities.

## Features

- CRUD operations for `Product` resources
- Request validation using FluentValidation
- Repository pattern with EF Core
- AutoMapper for entity-DTO mapping
- Middleware for logging and exception handling
- Asynchronous batch processing with cancellation support
- Unit tests for services and controllers
- Swagger documentation

---

## Technologies Used

- ASP.NET Core
- Entity Framework Core (SQL Server)
- AutoMapper
- FluentValidation
- Swagger / Swashbuckle
- Moq & xUnit for testing

---

## Getting Started

### Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download)
- SQL Server instance
- Visual Studio or VS Code

### Setup Instructions

1. **Clone the repository**

```bash
git clone https://github.com/tdresch/curotec-dotnet-challenge.git
cd your-repo
```

2. **Update SQL Server connection string**

In `appsettings.json`, update the `DefaultConnection`:

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=YOUR_SERVER;Database=ProductDb;Trusted_Connection=True;MultipleActiveResultSets=true"
}
```

3. **Apply EF Core Migrations**

```bash
dotnet ef migrations add InitialCreate
dotnet ef database update
```

4. **Run the Application**

```bash
dotnet run
```

API will be available at: `hhttps://localhost:7297;http://localhost:5212`

---

## Available Endpoints

- `GET /api/products` - Get all products
- `GET /api/products/{id}` - Get product by ID
- `POST /api/products` - Create new product
- `PUT /api/products/{id}` - Update product
- `DELETE /api/products/{id}` - Delete product
- `POST /api/products/batch` - Submit a batch of products for processing

---

## Swagger Documentation

Once the app is running, visit:

```
https://localhost:7297/swagger
```

You can interact with the API directly from the Swagger UI.

---

## Middleware

- **ExceptionHandlingMiddleware**: Catches unhandled exceptions and returns consistent error responses.
- **ProcessingMetricsMiddleware**: Tracks timing and logs metrics per request.
- **CancellationTrackingMiddleware**: Supports cancellation token tracking for long-running requests.

---

## Testing

Run unit tests:

```bash
dotnet test
```

---

## License

This project is licensed under the MIT License.
