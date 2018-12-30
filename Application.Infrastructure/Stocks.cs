using System.ComponentModel.DataAnnotations;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Application.Infrastructure
{
    public class Stocks
    {
        [Display(Name = "Stock Id")]
        [BsonId, BsonElement]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        
        [Display(Name = "Stock Code")]
        [BsonElement("Code")]
        public string Code { get; set; }

        [Display(Name = "Stock Name")]
        [BsonElement("Name")]
        public string Name { get; set; }

        [Display(Name = "Stock Price")]
        [BsonElement("Price")]
        public decimal Price { get; set; }
        
        [Display(Name = "Piece")]
        [BsonElement("Piece")]
        public int Piece { get; set; }

        [Display(Name = "Is Active")]
        [BsonElement("IsActive")]
        public string IsActive { get; set; }

        [Display(Name = "User ID")]
        [BsonElement("UserId")]
        public string UserId { get; set; }
    }
}