using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Application.Core.Models;
using MongoDB.Bson;

namespace Application.Infrastructure.Repositories
{
    public interface IStockRepository
    {
        Task<IEnumerable<Stocks>> GetAllStocks(string userId);

        Task<Stocks> GetStock(Expression<Func<Stocks, bool>> predicate);

        Task<Result> AddStock(Stocks stock);
                                                
        Task<Result> UpdateStock(string id,Stocks stock);

        Task<Result> DeleteStock(string userId, string stockId);

        bool StockIsExists(string id,string userId);
    }
}