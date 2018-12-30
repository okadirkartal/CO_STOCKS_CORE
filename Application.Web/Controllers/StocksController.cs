using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Application.Core.Models;
using Application.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace Application.Web.Controllers
{
    public class StocksController : BaseController
    {
        public StocksController(IConfiguration configuration) : base(configuration)
        {
        }


        private async Task<int> GetStockSettings(string userId)
        {
            StockSettings result = null;

            HttpResponseMessage response = await Client.GetAsync($"StockSettings/{userId}/");

            if (response.IsSuccessStatusCode)
            {
                result = await response.Content.ReadAsAsync<StockSettings>();
                return result.TickerSecond;
            }

            return 0;
        }


        [HttpGet]
        public async Task<IActionResult> Index()
        {
            ViewBag.Title = "Stock List";
            IEnumerable<Stocks> result = null;

            HttpResponseMessage response = await Client.GetAsync("Stock/StockList/1");
            //response.EnsureSuccessStatusCode();

            if (response.IsSuccessStatusCode)
            {
                result = await response.Content.ReadAsAsync<IEnumerable<Stocks>>();
            }

            TempData["TickerSecond"] = await GetStockSettings("1");
            return View(result);
        }

        [HttpGet]
        public async Task<IActionResult> StockForm(string userId, string stockId)
        {
            ViewBag.Title = "Stock Form";
            Stocks result = null;

            HttpResponseMessage response = await Client.GetAsync($"Stock/Stock/{stockId}/{userId}");

            if (response.IsSuccessStatusCode)
            {
                result = await response.Content.ReadAsAsync<Stocks>();
            }

            return View(result);
        }


        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> StockForm(Stocks stock)
        {
            Stocks result = null;
            HttpResponseMessage response = null;

            if (stock.Id == null)
            {
                response = await Client.PostAsJsonAsync("Stock/AddStock/", stock);
            }
            else
            {
                response = await Client.PutAsJsonAsync($"Stock/{stock.Id}", stock);
            }

            if (response.IsSuccessStatusCode)
            {
                result = await response.Content.ReadAsAsync<Stocks>();
                return RedirectToAction("Index");
            }

            return View(stock);
        }

        [HttpGet]
        public async Task<IActionResult> DeleteStock(string userId, string stockId)
        {
            ViewBag.Title = "Delete Stock";

            Stocks result = null;

            HttpResponseMessage response = await Client.GetAsync($"Stock/Stock/{stockId}/{userId}");

            if (response.IsSuccessStatusCode)
            {
                result = await response.Content.ReadAsAsync<Stocks>();
            }

            return View(result);
        }


        [HttpPost]
        public async Task<IActionResult> DeleteStock(Stocks stock)
        {
            ViewBag.Title = "Delete Stock";
            Result result = null;

            HttpResponseMessage response = await Client.DeleteAsync($"Stock?stockId={stock.Id}&userId={stock.UserId}");

            if (response.IsSuccessStatusCode)
            {
                result = await response.Content.ReadAsAsync<Result>();
                return RedirectToAction("Index");
            }

            return View(stock);
        }
    }
}