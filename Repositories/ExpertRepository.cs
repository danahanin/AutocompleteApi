using MongoDB.Driver;
using AutocompleteApi.Constants;
using AutocompleteApi.Models;

namespace AutocompleteApi.Repositories;

public class ExpertRepository : IExpertRepository
{
    private readonly IMongoCollection<Expert> _collection;

    public ExpertRepository(IMongoDatabase database)
    {
        _collection = database.GetCollection<Expert>(AppConstants.CollectionNames.Experts);
    }

    public async Task<List<Expert>> SearchByNameAsync(string query)
    {
        var filter = Builders<Expert>.Filter.Regex(e => e.Name, new MongoDB.Bson.BsonRegularExpression(query, "i"));
        return await _collection.Find(filter).ToListAsync();
    }
}

