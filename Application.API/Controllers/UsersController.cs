using System.Threading.Tasks;
using Application.API.Jwt;
using Application.Core.Models;
using Application.Infrastructure;
using Application.Infrastructure.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Application.API.Controllers
{
    [Produces("application/json")]
    [Route("api/Users")]
    public class UsersController : ControllerBase
    {
        private readonly IUserRepository _userRepository;

        public UsersController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        [AllowAnonymous]
        [HttpPost, Route("Login")]
        public async Task<IActionResult> Login([FromBody] Users model)
        {      
            var result= await _userRepository.Login(model);
            Users user = null;
            if (result != null)
            {
                 user = (Users) result.Model;
                var token = new JwtTokenBuilder()
                    .AddSecurityKey(JwtSecurityKey.Create("key-value-token-expires"))
                    .AddSubject(user.UserName)
                    .AddIssuer("issuerTest")
                    .AddAudience("bearerTest")
                    .AddClaim("MembershipId", user.Id.ToString())
                    .AddExpiry(1)
                    .Build();
                user.Token = token.Value;
                
            }

            return Ok(user);
        }

        [AllowAnonymous]
        [HttpPost, Route("Register")]
        public async Task<IActionResult> Register([FromBody] Users model)
        {
            var result = await _userRepository.Register(model);
            return Ok(result);
        }
    }
}