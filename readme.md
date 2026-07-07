# SomeCrud

A simple ASP.NET Core Web API built with .NET 10.

## Requirements

### Running with .NET SDK

- .NET 10 SDK
- A database supported by the project (configure the connection string before running)

### Running with Docker

- Docker 20+
- (Optional) Docker Compose

---

# Configuration

The application reads its configuration from `appsettings.json` and environment variables.

Environment variables override values from `appsettings.json`.

Example:

```bash
ConnectionStrings__DefaultConnection="Server=localhost;Database=SomeCrud;User Id=sa;Password=your_password;"
```

---

# Running without Docker

## Restore dependencies

```bash
dotnet restore
dotnet tool restore
```

## Apply database migrations

```bash
dotnet ef database update
```

## Run the application

```bash
dotnet run --project some-crud.webApi
```

The API will start on the configured ports.

---

# Running with Docker

## Build the image

```bash
docker build -t some-crud .
```

## Build a volume
```bash
docker volume create some-crud-data
```


## Run the container

```bash
docker run \
    -p 8080:8080 \
    -e ConnectionStrings__database="Data Source=/app/data/database.sql;" \
    -v some-crud-data:/app/data \
    some-crud
```
---