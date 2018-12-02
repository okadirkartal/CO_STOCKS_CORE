using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Application.Core.Models;
using Application.Infrastructure.DAL;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Application.Infrastructure.Repositories
{
    public class StockRepository : IStockRepository
    {
        private readonly StockContext _context = null;

        public StockRepository(IOptions<Settings> settings)
        {
            _context=new StockContext(settings);
        }

        public async Task<IEnumerable<Stocks>> GetAllStocks(string userId)
        {
            try
            {
                if (!string.IsNullOrEmpty(userId))
                {
                    return await _context.Stocks.Find(x => x.UserId == userId).ToListAsync();
                    
                }

                return null;
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        public async Task<Stocks> GetStock(string stockCode, string userId="")
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(userId))  
                  return await _context.Stocks.Find(x => x.StockCode == stockCode && x.UserId==userId).FirstOrDefaultAsync();
                   
                return  await _context.Stocks.Find(x => x.StockCode == stockCode).FirstOrDefaultAsync();
              
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        

        public async Task<Result> AddStock(Stocks stock)
        {
            var result = new Result() { IsSuccess = false };
            try
            {
          
                var existingCode = await GetStock(stock.StockCode);

                if (existingCode!=null)
                {
                    result.ReturnMessage = "this code is exists";
                    return result;
                }

                
               await _context.Stocks.InsertOneAsync(stock);
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

        public async Task<Result> UpdateStock(Stocks stock)
        {
            var result = new Result() { IsSuccess = false };
            try
            {
          
                   var existingCode = await GetStock(stock.StockCode);

                    if (existingCode!=null && existingCode.Id!=stock.Id)
                    {
                        result.ReturnMessage = "this code is exists";
                        return result;
                    }

                ReplaceOneResult actionResult =
                    await _context.Stocks.ReplaceOneAsync(n => n.Id.Equals(stock.Id), stock,
                        new UpdateOptions {IsUpsert = true});
                 
                if (actionResult.IsAcknowledged && actionResult.ModifiedCount > 0)
                {
                    result.IsSuccess = true;
                    result.ReturnMessage = "Stock updated";
                }
                    
            }
            catch (Exception ex)
            {
                result.ReturnMessage = ex.Message;
                result.IsSuccess = false;
            }

            return result;
        }


        public async  Task<Result> DeleteStock(string userId, string stockCode)
        {
            var result = new Result() { IsSuccess = false };
            try
            {

                var stock = await GetStock(stockCode, userId);
                DeleteResult actionResult =
                    await _context.Stocks.DeleteManyAsync(stock.ToBsonDocument());
                 
                if (actionResult.IsAcknowledged && actionResult.DeletedCount > 0)
                {
                    result.IsSuccess = true;
                    result.ReturnMessage = "Stock deleted";
                }
                    
            }
            catch (Exception ex)
            {
                result.ReturnMessage = ex.Message;
                result.IsSuccess = false;
            }

            return result;
        }
    }
}