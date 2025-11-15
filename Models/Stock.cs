using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace AutocompleteApi.Models;

public class Stock
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }
    public string Ticker { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public long MarketCap { get; set; }
}

