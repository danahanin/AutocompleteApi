# Autocomplete API

A simple autocomplete API built with ASP.NET Core 8 and MongoDB that provides search functionality for stocks and experts.

## How to Run Locally

### Prerequisites

- .NET 8 SDK
- Node.js 16+
- MongoDB running on `localhost:27017`

### Backend (API)

```bash
cd AutocompleteApi
dotnet restore
dotnet run
```

API runs at: `http://localhost:5000`  
Swagger UI: `http://localhost:5000/swagger`

### Frontend (React)

```bash
cd frontend
npm install
npm start
```

UI runs at: `http://localhost:3000`

## Assumptions or Design Decisions

### Technology Choices

- **MongoDB**: Chosen for flexible schema and easy JSON data storage. Indexed searches provide fast autocomplete performance.
- **Repository Pattern**: Data access is abstracted through repositories (`IStockRepository`, `IExpertRepository`) for better testability and maintainability.
- **Clean Architecture**: Separation of concerns with distinct layers (Models, Repositories, Services, Controllers, Migrations).

### Database Migration

- **Automatic Migration**: Database migration runs automatically on application startup.
- **Idempotent**: Checks if data already exists before migrating to avoid duplicate data.
- **Indexes**: Creates indexes on `Ticker` and `Name` fields for optimal search performance.

### Code Organization

- **Constants**: All hardcoded strings (result types, collection names, etc.) are centralized in `AppConstants` for easy maintenance.
- **DTO Pattern**: Uses `AutocompleteResult` DTO for clean API responses.
- **Error Handling**: Basic error handling in controller with logging.
