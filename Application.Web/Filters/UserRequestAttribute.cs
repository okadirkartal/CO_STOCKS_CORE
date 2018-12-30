using System;
using Application.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography.X509Certificates;
using Application.Infrastructure;
using Application.Web.Controllers;
using Application.Web.Extensions;
using Microsoft.Extensions.Primitives;
using  System.Linq;
using System.Security.Claims;
using System.Threading;

namespace Application.Web.Filters
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class UserRequestAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.HttpContext.User.Identity.IsAuthenticated)
                context.HttpContext.Response.Redirect("/Users/Index");

            var client = ((BaseController) context.Controller).Client;

            if (client.DefaultRequestHeaders.Authorization == null)
            {
                client.DefaultRequestHeaders.Add("Authorization", "Bearer " + context.HttpContext.Request.Cookies["Token"]);
            }
            
            

            HttpResponseMessage response = client.GetAsync($"TokenValidator").Result;

            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                var user = new Users
                {
                    UserName = context.HttpContext.User.GetUserName(),
                    Password = context.HttpContext.User.GetPassword()
                };

                response = client.PostAsJsonAsync("Users/Login", user).Result;


                if (response.IsSuccessStatusCode)
                {
                    var newUser = response.Content.ReadAsAsync<Users>().Result;
                    client.DefaultRequestHeaders.Remove("Authorization");
                    client.DefaultRequestHeaders.Add("Authorization", "Bearer " + newUser.Token);
                    
                    context.HttpContext.Response.Cookies.Delete("Token");
                    context.HttpContext.Response.Cookies.Append("Token",newUser.Token);

                }
            }

            base.OnActionExecuting(context);
        }
    }
}