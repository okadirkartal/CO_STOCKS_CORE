using System.ComponentModel;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace Application.Infrastructure.DAL
{
    public class StockContext
    {
        private readonly IMongoDatabase _database = null;

        public StockContext(IOptions<Settings> settings)
        {
            var client= new MongoClient(settings.Value.ConnectionString);
                 _database = client.GetDatabase(settings.Value.Database);
        }

        public IMongoCollection<Stocks> Stocks => _database.GetCollection<Stocks>("Stocks");

        public IMongoCollection<Users> Users => _database.GetCollection<Users>("Users");
        
        public IMongoCollection<StockSettings> StockSettings => _database.GetCollection<StockSettings>("StockSettings");

    }
}