namespace Application.Infrastructure
{
    public class Stocks
    {
        public int Id { get; set; }

        public string StockCode { get; set; }

        public int Price { get; set; }

        public bool IsActive { get; set; }
        
        public string UserId { get; set; }
    }
}