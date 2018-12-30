using System;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Net;
using System.Net.Http;
using Application.Infrastructure;
using Application.Web.Controllers;
using Application.Web.Extensions;

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
                client.DefaultRequestHeaders.Add("Authorization", "Bearer " +
                                                                  (context.HttpContext.Session
                                                                       .GetObjectFromJson<string>("Token") ??
                                                                   context.HttpContext.User.GetToken()));
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
                    context.HttpContext.Session.SetObjectAsJson("Token", newUser.Token);
                    client.DefaultRequestHeaders.Remove("Authorization");
                    client.DefaultRequestHeaders.Add("Authorization", "Bearer " + newUser.Token);
                }
            }

            base.OnActionExecuting(context);
        }
    }
}