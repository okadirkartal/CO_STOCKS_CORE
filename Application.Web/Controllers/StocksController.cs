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
       
        public StocksController(IConfiguration configuration) : base(configuration) { }
      
        
        public async Task<IActionResult> Index()
        {
            IEnumerable<Stocks> result = null;

            HttpResponseMessage response = await client.GetAsync("/Stocks/StockList/1");
            response.EnsureSuccessStatusCode();
                
            if (response.IsSuccessStatusCode)
            {
                result = await response.Content.ReadAsAsync<IEnumerable<Stocks>>();
            }
            
            return View(result);
        } 
        
        public async Task<IActionResult> Stocks()
        {
           Stocks result = null;

            HttpResponseMessage response = await client.GetAsync("/Stocks/Stocks/");
            response.EnsureSuccessStatusCode();
                
            if (response.IsSuccessStatusCode)
            {
                result = await response.Content.ReadAsAsync<Stocks>();
            }
            
            return View(result);
        } 
        
        public async Task<IActionResult> AddStock(Stocks stock)
        {

            Result result = null;
            HttpResponseMessage response = null;
            
            if(stock.Id>0)
            {
                 response = await client.PostAsJsonAsync("/Stocks/AddStock/",stock);
            }
            else
            {
                 response = await client.PostAsJsonAsync("/Stocks/UpdateStock/",stock);

            }
            response.EnsureSuccessStatusCode();
                
            if (response.IsSuccessStatusCode)
            {
                result = await response.Content.ReadAsAsync<Result>();
            }
            
            return View(result);
        } 
        
        public async Task<IActionResult> DeleteStock(int id)
        {
            Result result = null;

            HttpResponseMessage response = await client.PostAsJsonAsync("/Stocks/DeleteStock/",id);
            response.EnsureSuccessStatusCode();
                
            if (response.IsSuccessStatusCode)
            {
                result = await response.Content.ReadAsAsync<Result>();
            }
            
            return View(result);
        }    
    }
}