using System.Collections.Generic;
using System.Threading.Tasks;
using Application.Infrastructure;
using Application.Infrastructure.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Application.API.Controllers
{
    [Produces("application/json")]
    [Route("api/Stock")]
    [ApiController]
    public class StockController : ControllerBase
    {
        private readonly IStockRepository _stockRepository;

        public StockController(IStockRepository stockRepository)
        {
            _stockRepository = stockRepository;
        }


        [HttpGet("{userId}"), Route("StockList/{userId}")]
        public async Task<IEnumerable<Stocks>> StockList(string userId)
        {
            return await _stockRepository.GetAllStocks(userId);
        }

        [HttpGet, Route("Stock/{stockId}/{userId}")]
        public async Task<Stocks> Stock([FromRoute] string stockId, [FromRoute] string userId)
        {
            return await _stockRepository.GetStock(x => x.UserId == userId && x.Id == stockId) ?? new Stocks();
        }

        [HttpPost, Route("AddStock")]
        public async Task<IActionResult> AddStock([FromBody] Stocks stock)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var result = await _stockRepository.AddStock(new Stocks
            {
                Code = stock.Code, Name = stock.Name, Price = stock.Price, Piece = stock.Piece,
                IsActive = stock.IsActive, UserId = stock.UserId
            });
            return Ok(result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateStock([FromRoute] string id, [FromBody] Stocks stock)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            if (!_stockRepository.StockIsExists(id, stock.UserId))
                return NotFound();

            var result = await _stockRepository.UpdateStock(id, new Stocks
            {
                Id = id, Code = stock.Code, Name = stock.Name, Price = stock.Price, Piece = stock.Piece,
                IsActive = stock.IsActive,
                UserId = stock.UserId
            });

            return Ok(result);
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteStock([FromQuery] string stockId, [FromQuery] string userId)
        {
            if (_stockRepository.StockIsExists(stockId, userId))
            {
                var result = await _stockRepository.DeleteStock(userId, stockId);
                return Ok(result);
            }

            return NotFound();
        }
    }
}