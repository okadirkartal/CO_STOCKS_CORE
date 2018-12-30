using System.Net.Http;
using System.Threading.Tasks;
using Application.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace Application.Web.Controllers
{
    public class StockSettingsController : BaseController
    {
        public StockSettingsController(IConfiguration configuration) : base(configuration)
        {
        }


        [HttpGet]
        public async Task<IActionResult> Index(string userId, string tickerSecond)
        {
            ViewBag.Title = "Stock Settings";
            StockSettings result = null;

            HttpResponseMessage response = await Client.GetAsync($"StockSettings/{userId}/{tickerSecond}");

            if (response.IsSuccessStatusCode)
            {
                result = await response.Content.ReadAsAsync<StockSettings>();
            }

            return View(result);
        }


        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(StockSettings model)
        {
            StockSettings result = null;
            HttpResponseMessage response = null;


            response = await Client.PostAsJsonAsync("StockSettings", model);

            if (response.IsSuccessStatusCode)
            {
                result = await response.Content.ReadAsAsync<StockSettings>();
                return Redirect("/Stocks");
            }

            return View(model);
        }
    }
}