using System.Collections.Generic;
using System.Threading.Tasks;
using Application.Core.Models;
using Application.Infrastructure;
using Application.Infrastructure.DAL;
using Application.Infrastructure.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Application.API.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class StockController : Controller
    {

        private readonly IStockRepository _stockRepository;

        public StockController(IStockRepository stockRepository)
        {
            _stockRepository = stockRepository;
        }

        [HttpGet("{userId}"),Route("StockList")]
        public async Task<IEnumerable<Stocks>> StockList([FromQuery]string userId)
        {
            return await _stockRepository.GetAllStocks(userId);
        }
        
        [HttpGet]
        public async Task<Stocks> Stocks(string stockCode,string userId="")
        {
            return await _stockRepository.GetStock(stockCode,userId)?? new Stocks();
        }

        [HttpPost,Route("AddStock")]
        public async Task<Result> AddStock([FromBody] Stocks stock)
        {
            return await _stockRepository.AddStock(new Stocks
            {
                StockCode = stock.StockCode, Price = stock.Price, IsActive = stock.IsActive, UserId = stock.UserId
            });
        }
        
        [HttpPut,Route("UpdateStock")]
        public async Task<Result> UpdateStock([FromBody] Stocks stock)
        {
            return await _stockRepository.UpdateStock(new Stocks
            {
                StockCode = stock.StockCode, Price = stock.Price, IsActive = stock.IsActive, UserId = stock.UserId
            });
        }
        
        [HttpPost,Route("DeleteStock")]
        public async Task<Result> DeleteStock([FromBody] Stocks stock)
        {
            return await _stockRepository.DeleteStock(stock.UserId , stock.StockCode);
        }
    }
}