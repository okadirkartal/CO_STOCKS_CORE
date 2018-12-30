using System.Threading.Tasks;
using Application.Infrastructure;
using Application.Infrastructure.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Application.API.Controllers
{
    [Authorize(Policy = "Member")]
    [Produces("application/json")]
    [Route("api/TokenValidator")]
    [ApiController]
    public class TokenValidatorController : ControllerBase
    {
     
        

        [HttpGet]
        public  void Get()
        {
             
        }
 
    }
}