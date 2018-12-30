using System.Threading.Tasks;
using Application.Core.Models;
using Application.Core.Models.ViewModels;
using Application.Infrastructure.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Application.API.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUserRepository _userRepository;

        public UsersController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        [AllowAnonymous]
        [HttpPost, Route("Login")]
        public async Task<Result> Login([FromBody] LoginViewModel model)
        {
            return await _userRepository.Login(model);
        }

        [AllowAnonymous]
        [HttpPost, Route("Register")]
        public async Task<Result> Register([FromBody] RegisterViewModel model)
        {
            return await _userRepository.Register(model);
        }
    }
}