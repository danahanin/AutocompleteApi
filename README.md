# Autocomplete API

A simple autocomplete API built with ASP.NET Core 8 and MongoDB that provides search functionality for stocks and experts.

## How to Run Locally

### Prerequisites
- .NET 8 SDK
- MongoDB (local installation or MongoDB Atlas connection string)

### Steps

1. **Install MongoDB** (if not already installed)
   - Local: Download from [mongodb.com](https://www.mongodb.com/try/download/community)
   - Cloud: Use MongoDB Atlas and update connection string in `appsettings.json`

2. **Configure MongoDB Connection**
   - Update `appsettings.json` with your MongoDB connection string:
   ```json
   {
     "MongoDB": {
       "ConnectionString": "mongodb://localhost:27017",
       "DatabaseName": "AutocompleteDB"
     }
   }
   ```

3. **Place JSON Files**
   - Ensure `stocks.json` and `experts.json` are in the parent directory of the project folder

4. **Run the Application**
   ```bash
   cd AutocompleteApi
   dotnet restore
   dotnet run
   ```

   The API will be available at:
   - HTTP: `http://localhost:5000`
   - HTTPS: `https://localhost:5001`
   - Swagger UI: `https://localhost:5001/swagger`

5. **Test the API**
   ```bash
   curl "http://localhost:5000/api/autocomplete?query=a"
   ```

## Assumptions or Design Decisions

### Technology Choices
- **MongoDB**: Chosen for flexible schema and easy JSON data storage. Indexed searches provide fast autocomplete performance.
- **Repository Pattern**: Data access is abstracted through repositories (`IStockRepository`, `IExpertRepository`) for better testability and maintainability.
- **Clean Architecture**: Separation of concerns with distinct layers (Models, Repositories, Services, Controllers, Migrations).

### Search Logic
- **Case-insensitive**: Uses MongoDB regex with case-insensitive option for user-friendly search.
- **Sorting Priority**: Results are sorted by relevance:
  1. Exact ticker match (for stocks)
  2. Exact name match (for stocks and experts)
  3. Starts with match
  4. Contains match
- **Result Limit**: Returns up to 10 results total (mix of stocks and experts).

### Database Migration
- **Automatic Migration**: Database migration runs automatically on application startup.
- **Idempotent**: Checks if data already exists before migrating to avoid duplicate data.
- **Indexes**: Creates indexes on `Ticker` and `Name` fields for optimal search performance.

### Code Organization
- **Constants**: All hardcoded strings (result types, collection names, etc.) are centralized in `AppConstants` for easy maintenance.
- **DTO Pattern**: Uses `AutocompleteResult` DTO for clean API responses.
- **Error Handling**: Basic error handling in controller with logging.
