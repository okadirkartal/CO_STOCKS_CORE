using System.Net.Http;
using System.Threading.Tasks;
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
    public class StockSettingsController : BaseController
    {
        public StockSettingsController(IConfiguration configuration) : base(configuration)
        {
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            ViewBag.Title = "Stock Settings";
            StockSettings result = null;
            HttpResponseMessage response = await Client.GetAsync($"StockSettings/{User.GetUserId()}");

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

            response = await Client.PostAsJsonAsync($"StockSettings/{User.GetUserId()}", model);

            if (response.IsSuccessStatusCode)
            {
                result = await response.Content.ReadAsAsync<StockSettings>();
                return Redirect("/Stocks");
            }

            return View(model);
        }
    }
}