using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace InvestmentManager.Shared.Utilities.Abstractions;

public class BaseEntity
{
    [BsonId]
    [BsonRepresentation(BsonType.String)]
    public string Id { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}
