using System.Threading.Tasks;
using Application.Infrastructure;
using Application.Infrastructure.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Application.API.Controllers
{
    [Authorize(Policy = "Member")]
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


        [HttpPost("{userId}")]
        public async Task<IActionResult> UpdateTickerSecond([FromRoute]string userId,[FromBody] StockSettings model)
        {
            var result = await _stockSettingsRepository.UpdateStockSettings(new StockSettings
            {
                Id = model.Id, TickerSecond = model.TickerSecond, UserId = userId
            });
            return Ok(result);
        }
    }
}