using Microsoft.AspNetCore.Mvc;

namespace Application.Web.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}