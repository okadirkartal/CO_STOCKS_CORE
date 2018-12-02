using System.ComponentModel.DataAnnotations;
using MongoDB.Bson.Serialization.Attributes;

namespace Application.Core.Models.ViewModels
{
    public class StockViewModel
    {
        public int Id { get; set; }

        [Required]
        [StringLength(15, ErrorMessage = "Code can be maximum 15 length")]  
        [BsonElement("Code")]
        public string Code { get; set; }

        [StringLength(20, ErrorMessage = "Name can be maximum 20 length")]
        [BsonElement("Name")]
        public string Name { get; set; }

        [DataType(DataType.Currency)]
        [BsonElement("Quantity")]
        public int Quantity { get; set; }

        [BsonElement("Price")]
        public int Price { get; set; }
        
        [BsonElement("UserID")]
        public string UserID { get; set; }
    }
}