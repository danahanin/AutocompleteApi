using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace AutocompleteApi.Models;

public class Expert
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Firm { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
}

