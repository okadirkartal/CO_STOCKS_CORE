using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Application.Core.Models;
using Application.Infrastructure;
using Application.Web.Extensions;
using Application.Web.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace Application.Web.Controllers
{
    [Authorize]
    [UserRequest]
    public class StocksController : BaseController
    {
        public StocksController(IConfiguration configuration) : base(configuration)
        {
        }


        private async Task<int> GetStockSettings()
        {
            StockSettings result = null;

            HttpResponseMessage response = await Client.GetAsync($"StockSettings/{User.GetUserId()}/");

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

            HttpResponseMessage response = await Client.GetAsync($"Stock/StockList/{User.GetUserId()}");
           
            if (response.IsSuccessStatusCode)
            {
                result = await response.Content.ReadAsAsync<IEnumerable<Stocks>>();
            }

            TempData["TickerSecond"] = await GetStockSettings();
            return View(result);
        }

        [HttpGet]
        public async Task<IActionResult> StockForm(string stockId)
        {
            ViewBag.Title = "Stock Form";
            Stocks result = null;

            HttpResponseMessage response = await Client.GetAsync($"Stock/Stock/{stockId}/{User.GetUserId()}");

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
          
            stock.UserId = User.GetUserId();
            
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
                return RedirectToAction("Index");
            }

            return View(stock);
        }

        [HttpGet]
        public async Task<IActionResult> DeleteStock(string stockId)
        {
            ViewBag.Title = "Delete Stock";

            Stocks result = null;

            HttpResponseMessage response = await Client.GetAsync($"Stock/Stock/{stockId}/{User.GetUserId()}");

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

            HttpResponseMessage response =
                await Client.DeleteAsync($"Stock?stockId={stock.Id}&userId={User.GetUserId()}");

            if (response.IsSuccessStatusCode)
            {
                result = await response.Content.ReadAsAsync<Result>();
                return RedirectToAction("Index");
            }

            return View(stock);
        }
    }
}