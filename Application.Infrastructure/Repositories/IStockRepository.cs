using System.Collections.Generic;
using System.Threading.Tasks;
using Application.Core.Models;

namespace Application.Infrastructure.Repositories
{
    public interface IStockRepository
    {
        Task<IEnumerable<Stocks>> GetAllStocks(string userId);

        Task<Stocks> GetStock(string stockCode, string userId);

        Task<Result> AddStock(Stocks stock);
                                                
        Task<Result> UpdateStock(Stocks stock);

        Task<Result> DeleteStock(string userId, string stockId); 
    }
}