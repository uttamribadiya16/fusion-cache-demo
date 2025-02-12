# fusion-cache-demo

# Solution Overview

This solution contains the following projects:

1. **FusionCacheDemo.Api**: This project is the API layer of the application.
2. **FusionCacheDemo.Core**: This project contains the core business logic and entities.
3. **FusionCacheDemo.Infrastructure**: This project handles data access and infrastructure concerns.

## Setup Instructions

### Prerequisites

- .NET SDK
- Docker (if using containers)
- SQL Server (or any other database you are using)
- Redis (if using Redis for caching)

### Configuration

Replace the connection strings in the `appsettings.json` file with your actual connection strings in FusionCacheDemo.Api project:

```json
"ConnectionStrings": {
  "DefaultConnection": "YourActualConnectionStringHere",
  "Redis": "YourActualConnectionStringHere"
}
```

### Restore Dependencies
```sh
dotnet restore
```

### Build the Solution
```sh
dotnet build
```

### Run the Application
```sh
dotnet run --project FusionCacheDemo.Api
```

## Docker Setup (if applicable)

### Build the Docker Image
```sh
docker build -t your-image-name .
```

### Run the Docker Container
```sh
docker run -d -p 8080:80 your-image-name
```

## Database Setup
Ensure your database is running and accessible. Update the connection string in the `appsettings.json` file accordingly.

## Redis Setup
Ensure your redis is running and accessible. Update the connection string in the `appsettings.json` file accordingly.

## Additional Notes
- Ensure that your firewall settings allow access to the necessary ports.
- Check the application logs for any errors or issues.

## Swagger API Documentation
After running the application, you can access the Swagger UI to explore the available APIs:

### Swagger UI URL
```
http://localhost:8080/swagger/index.html
```

## Project Details

### FusionCacheDemo.Api
**Description:** This project is the API layer of the application.  
**Technologies Used:** ASP.NET Core, Swagger, etc.

### FusionCacheDemo.Core
**Description:** This project contains the core business logic and entities.  
**Technologies Used:** .NET Standard, Entity Framework Core, etc.

### FusionCacheDemo.Infrastructure
**Description:** This project handles data access and infrastructure concerns.  
**Technologies Used:** Entity Framework Core, SQL Server, Redis, etc.
