using System;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Headers;
using Application.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace Application.Web.Controllers
{
    public class BaseController : Controller
    {
        private  readonly HttpClient _client = new HttpClient();

        private readonly IConfiguration _configuration;

        public BaseController(IConfiguration configuration)
        {
            _configuration = configuration;

            _client.BaseAddress = new Uri(_configuration.GetSection("ApplicationSettings:BaseApiUrl").Value);
            _client.DefaultRequestHeaders.Accept.Clear();
            
          
            
            _client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
          
            
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel {RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier});
        }
        
        public HttpClient Client
        {
            get
            {
                return  _client;
            }
        }
    }
}