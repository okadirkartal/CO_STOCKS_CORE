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
    public class StockSettingsRepository :DocumentDbRepository<StockSettings>,IStockSettingsRepository
    {

        public StockSettingsRepository()
        {
            base.Initialize();
            _collection = database.GetCollection<StockSettings>(nameof(StockSettings));
        }
        
        public async Task<StockSettings> GetStockSettings(Expression<Func<StockSettings, bool>> predicate)
        {
            try
            {
                IEnumerable<StockSettings> result = await Get(predicate);

                return result != null ? result.FirstOrDefault() : null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public async Task<Result> UpdateStockSettings(StockSettings stockSettings)
        {
            var result = new Result() {IsSuccess = false};
            try
            {
                StockSettings existingItem = await GetStockSettings(x => x.UserId == stockSettings.UserId);

                if (existingItem == null)
                {
                    await  Create(stockSettings);
                    result.IsSuccess = true;
                    result.ReturnMessage = "StockSettings added";
                    return result;
                }
                else
                {
                    var filter = Builders<StockSettings>.Filter.Eq(x => x.Id, stockSettings.Id);

                    await  Update(filter, stockSettings);


                    result.IsSuccess = true;
                    result.ReturnMessage = "StockSettings updated";
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