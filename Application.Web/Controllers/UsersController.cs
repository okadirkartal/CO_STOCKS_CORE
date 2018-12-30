using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Application.Core.Models;
using Application.Core.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace Application.Web.Controllers
{
    public class UsersController : BaseController
    {
       private readonly IConfiguration _configuration;

        public UsersController(IConfiguration configuration) : base(configuration) { }


        // GET
        public IActionResult Index()
        {
            return
                View();
        }

        public IActionResult Login()
        {
            return View(new LoginRegisterViewModel());
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> _LoginPartial(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                Result result = null;

                HttpResponseMessage response = await Client.PostAsJsonAsync("/Users/Login",model);
                response.EnsureSuccessStatusCode();
                
                if (response.IsSuccessStatusCode)
                {
                    result = await response.Content.ReadAsAsync<Result>();
                }

                if (result.IsSuccess)
                {
                    //  Current.User = new userSessionModel()
                    //    { userGUID = result.ReturnMessageList[0], userName = result.ReturnMessageList[1] };


                 /*   string url = !string.IsNullOrEmpty(Request.Query["returnUrl"][0])
                        ? Request.Query["returnUrl"]
                        : "/Stock/Index";

                    return Redirect(url);
               */ }

                ViewBag.Message = result.ReturnMessage;
            }

            return View(model);
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> _RegisterPartial(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                Result result = null;

                HttpResponseMessage response = await Client.PostAsJsonAsync("/Users/Register",model);
                response.EnsureSuccessStatusCode();
                if (response.IsSuccessStatusCode)
                {
                    result = await response.Content.ReadAsAsync<Result>();
                }

                if (result.IsSuccess)
                {
                    //      Current.User = new userSessionModel()
                    //        { userGUID = result.ReturnMessageList[0], userName = result.ReturnMessageList[1] };

                    return Redirect("/Stock/Index");
                }

                ViewBag.Message = result.ReturnMessage;
            }

            return View(model);
        }
    }
}