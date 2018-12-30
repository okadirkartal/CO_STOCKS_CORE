using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Application.Infrastructure
{
    public class StockSettings
    {
        [BsonId, BsonElement]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("UserId")] public string UserId { get; set; }

        [BsonElement("TickerSecond")] public int TickerSecond { get; set; }
    }
}