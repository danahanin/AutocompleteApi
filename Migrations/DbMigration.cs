using System.Text.Json;
using MongoDB.Driver;
using AutocompleteApi.Constants;
using AutocompleteApi.Models;

namespace AutocompleteApi.Migrations;

public class DbMigration : IDbMigration
{
    private readonly IMongoDatabase _database;
    private readonly IConfiguration _configuration;
    private readonly ILogger<DbMigration> _logger;

    public DbMigration(IMongoDatabase database, IConfiguration configuration, ILogger<DbMigration> logger)
    {
        _database = database;
        _configuration = configuration;
        _logger = logger;
    }

    public async Task MigrateAsync()
    {
        var stocksCollection = _database.GetCollection<Stock>(AppConstants.CollectionNames.Stocks);
        var expertsCollection = _database.GetCollection<Expert>(AppConstants.CollectionNames.Experts);

        // Skip if data exists
        if (await stocksCollection.CountDocumentsAsync(FilterDefinition<Stock>.Empty) > 0)
        {
            _logger.LogInformation("Data already exists. Skipping migration.");
            return;
        }

        // Load stocks
        var stocksPath = Path.Combine(Directory.GetCurrentDirectory(), _configuration["DataFiles:StocksPath"]);
        var stocksJson = await File.ReadAllTextAsync(stocksPath);
        var stocks = JsonSerializer.Deserialize<List<Stock>>(stocksJson, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        await stocksCollection.InsertManyAsync(stocks);
        _logger.LogInformation($"Loaded {stocks.Count} stocks");

        // Load experts
        var expertsPath = Path.Combine(Directory.GetCurrentDirectory(), _configuration["DataFiles:ExpertsPath"]);
        var expertsJson = await File.ReadAllTextAsync(expertsPath);
        var experts = JsonSerializer.Deserialize<List<Expert>>(expertsJson, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        await expertsCollection.InsertManyAsync(experts);
        _logger.LogInformation($"Loaded {experts.Count} experts");

        // Create indexes
        await stocksCollection.Indexes.CreateOneAsync(new CreateIndexModel<Stock>(Builders<Stock>.IndexKeys.Ascending(s => s.Ticker)));
        await stocksCollection.Indexes.CreateOneAsync(new CreateIndexModel<Stock>(Builders<Stock>.IndexKeys.Ascending(s => s.Name)));
        await expertsCollection.Indexes.CreateOneAsync(new CreateIndexModel<Expert>(Builders<Expert>.IndexKeys.Ascending(e => e.Name)));
    }
}

