using System.Threading.Tasks;
using Application.Infrastructure;
using Application.Infrastructure.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Application.API.Controllers
{
    [Produces("application/json")]
    [Route("api/StockSettings")]
    [ApiController]
    public class StockSettingsController : ControllerBase
    {
        private readonly IStockSettingsRepository _stockSettingsRepository;

        public StockSettingsController(IStockSettingsRepository stockSettingsRepository)
        {
            _stockSettingsRepository = stockSettingsRepository;
        }


        [HttpGet("{userId}")]
        public async Task<StockSettings> GetSettings([FromRoute] string userId)
        {
            return await _stockSettingsRepository.GetStockSettings(x => x.UserId == userId);
        }


        [HttpPost]
        public async Task<IActionResult> UpdateTickerSecond([FromBody] StockSettings model)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var result = await _stockSettingsRepository.UpdateStockSettings(new StockSettings
            {
                Id = model.Id, TickerSecond = model.TickerSecond, UserId = model.UserId
            });
            return Ok(result);
        }
    }
}