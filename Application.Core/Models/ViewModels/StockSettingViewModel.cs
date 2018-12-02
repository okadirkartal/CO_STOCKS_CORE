using System.ComponentModel.DataAnnotations;

namespace Application.Core.Models.ViewModels
{
    public class StockSettingViewModel
    {
        [Required]
        [DataType(DataType.Currency)]
        public int ticker_minute { get; set; }

        public string user_guid { get; set; }
    }
}