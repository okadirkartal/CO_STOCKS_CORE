using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Application.Core.Models;
using Application.Infrastructure.DAL;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Application.Infrastructure.Repositories
{
    public class StockRepository : DocumentDbRepository<Stocks>, IStockRepository
    {
        public StockRepository()
        {
            base.Initialize();
            _collection = database.GetCollection<Stocks>(nameof(Stocks));
        }

        public async Task<IEnumerable<Stocks>> GetAllStocks(string userId)
        {
            try
            {
                return await Get(x => x.UserId == userId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<Stocks> GetStock(Expression<Func<Stocks, bool>> predicate)
        {
            try
            {
                IEnumerable<Stocks> result = await Get(predicate);

                return result != null ? result.FirstOrDefault() : null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public async Task<Result> AddStock(Stocks stock)
        {
            var result = new Result() {IsSuccess = false};
            try
            {
                var existingCode = await GetStock(x => x.Code == stock.Code);

                if (existingCode != null)
                {
                    result.ReturnMessage = "this code is exists";
                    return result;
                }


                await Create(stock);
                result.IsSuccess = true;
                result.ReturnMessage = "Stock added";
            }
            catch (Exception ex)
            {
                result.ReturnMessage = ex.Message;
                result.IsSuccess = false;
            }

            return result;
        }

        public async Task<Result> UpdateStock(string id, Stocks stock)
        {
            var result = new Result() {IsSuccess = false};
            try
            {
                var existingCode = await GetStock(x => x.Code == stock.Code && x.Id != id);

                if (existingCode != null)
                {
                    result.ReturnMessage = "this code is exists";
                    return result;
                }


                var filter = Builders<Stocks>.Filter.Eq(x => x.Id, id);

                await Update(filter, stock);


                result.IsSuccess = true;
                result.ReturnMessage = "Stock updated";
            }
            catch (Exception ex)
            {
                result.ReturnMessage = ex.Message;
                result.IsSuccess = false;
            }

            return result;
        }


        public async Task<Result> DeleteStock(string userId, string stockId)
        {
            var result = new Result() {IsSuccess = false};
            try
            {
                await Remove(new ObjectId(stockId),
                    x => x.Id == stockId && x.UserId == userId);

                result.IsSuccess = true;
                result.ReturnMessage = "Stock deleted";
            }
            catch (Exception ex)
            {
                result.ReturnMessage = ex.Message;
                result.IsSuccess = false;
            }

            return result;
        }


        public bool StockIsExists(string id, string userId)
        {
            return Get(x => x.Id == id && x.UserId == userId).Result.Any();
        }
    }
}