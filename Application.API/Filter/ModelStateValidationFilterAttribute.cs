using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters; 

namespace Application.API.Filter
{
    public class ModelStateValidationFilterAttribute :ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        { 
            if (context.HttpContext.Request.Method=="POST"
            || context.HttpContext.Request.Method=="PUT" 
            && !context.ModelState.IsValid)
                context.Result = new BadRequestObjectResult(context.ModelState);
            base.OnActionExecuting(context);
        }
    }
}