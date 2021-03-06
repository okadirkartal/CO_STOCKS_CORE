using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Application.Core.Models;

namespace Application.Infrastructure.Repositories
{
    public interface IStockSettingsRepository
    {
        Task<StockSettings> GetStockSettings(Expression<Func<StockSettings, bool>> predicate);

        Task<Result> UpdateStockSettings(StockSettings stockSettings);
    }
}