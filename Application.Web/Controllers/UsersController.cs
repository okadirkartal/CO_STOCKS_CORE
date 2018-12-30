using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Threading.Tasks;
using Application.Core.Models;
using Application.Core.Models.ViewModels;
using Application.Infrastructure;
using Application.Web.Extensions;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting.Internal;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace Application.Web.Controllers
{
    public class UsersController : BaseController
    {
        private readonly IConfiguration _configuration;

        public UsersController(IConfiguration configuration) : base(configuration)
        {
        }


        // GET


        [HttpGet]
        public IActionResult Index()
        {
            return View(new LoginRegisterViewModel());
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> _LoginPartial(LoginViewModel model)
        {
            ViewBag.Title = "Login";
            if (ModelState.IsValid)
            {
                Users user = null;

                HttpResponseMessage response = await Client.PostAsJsonAsync("Users/Login", model);


                if (response.IsSuccessStatusCode)
                {
                    user = await response.Content.ReadAsAsync<Users>();

                    if (user!=null)
                    {
                        var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);
                        identity.AddClaim(new Claim("UserId", user.Id));
                        identity.AddClaim(new Claim("UserName", user.UserName));
                        identity.AddClaim(new Claim("Password", user.Password));
                        identity.AddClaim(new Claim("Token", user.Token));
                        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, 
                            new ClaimsPrincipal(identity));
                             
                      HttpContext.Session.SetObjectAsJson("Token",user.Token);
                       
                        return Redirect("/Stocks");
                    }
                }

               // ViewBag.Message = result?.ReturnMessage;
            }

            return View("Index",new LoginRegisterViewModel(){ loginViewModel = model});
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> _RegisterPartial(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                Result result = null;

                
                HttpResponseMessage response = await Client.PostAsJsonAsync("/Users/Register", model);
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

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            HttpContext.Response.Cookies.Delete("Token");
            await HttpContext.SignOutAsync("Cookies");
            return RedirectToAction("Index");

        }
    }
}

