using System.ComponentModel.DataAnnotations;

namespace Application.Core.Models.ViewModels
{
    public class StockViewModel
    {
        public int Id { get; set; }

        [Required]
        [StringLength(15, ErrorMessage = "Code can be maximum 15 length")]  
        public string Code { get; set; }

        [StringLength(20, ErrorMessage = "Name can be maximum 20 length")]
        public string Name { get; set; }

        [DataType(DataType.Currency)]
        public int Quantity { get; set; }

        public int Price { get; set; }
        
        public string UserID { get; set; }
    }
}