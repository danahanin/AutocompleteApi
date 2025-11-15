using MongoDB.Driver;
using AutocompleteApi.Constants;
using AutocompleteApi.Models;

namespace AutocompleteApi.Repositories;

public class StockRepository : IStockRepository
{
    private readonly IMongoCollection<Stock> _collection;

    public StockRepository(IMongoDatabase database)
    {
        _collection = database.GetCollection<Stock>(AppConstants.CollectionNames.Stocks);
    }

    public async Task<List<Stock>> SearchByTickerOrNameAsync(string query)
    {
        var filter = Builders<Stock>.Filter.Or(
            Builders<Stock>.Filter.Regex(s => s.Ticker, new MongoDB.Bson.BsonRegularExpression(query, "i")),
            Builders<Stock>.Filter.Regex(s => s.Name, new MongoDB.Bson.BsonRegularExpression(query, "i"))
        );

        return await _collection.Find(filter).ToListAsync();
    }
}

