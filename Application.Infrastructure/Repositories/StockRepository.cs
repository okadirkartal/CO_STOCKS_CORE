using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Core.Models;
using Application.Infrastructure.DAL;

namespace Application.Infrastructure.Repositories
{
    public class StockRepository : IStockRepository
    {
        public async Task<IEnumerable<Stocks>> GetAllStocks(string userId)
        {
            try
            {
                if (!string.IsNullOrEmpty(userId))
                {
                    return await DocumentDbRepository<Stocks>.GetItemsAsync(x => x.UserId == userId) ??
                           new List<Stocks>();
                }

                return null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<Stocks> GetStock(string stockCode, string userId = "")
        {
            try
            {
                IEnumerable<Stocks> result;
                if (!string.IsNullOrWhiteSpace(userId))
                    result = await DocumentDbRepository<Stocks>.GetItemsAsync(x =>
                        x.StockCode == stockCode && x.UserId == userId);

                result = await DocumentDbRepository<Stocks>.GetItemsAsync(x => x.StockCode == stockCode);

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
                var existingCode = await GetStock(stock.StockCode);

                if (existingCode != null)
                {
                    result.ReturnMessage = "this code is exists";
                    return result;
                }


                await DocumentDbRepository<Stocks>.CreateItemAsync(stock);
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
            var result = new Result() {IsSuccess = false};
            try
            {
                var existingCode = await GetStock(stock.StockCode);

                if (existingCode != null && existingCode.Id != stock.Id)
                {
                    result.ReturnMessage = "this code is exists";
                    return result;
                }


                await DocumentDbRepository<Stocks>.UpdateItemAsync(stock);


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
                var document = DocumentDbRepository<Stocks>.GetDocument(stockId);

                await DocumentDbRepository<Stocks>.DeleteDocumentAsync(document);

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
    }
}