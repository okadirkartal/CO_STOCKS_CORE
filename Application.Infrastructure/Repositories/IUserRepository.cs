using System.Collections.Generic;
using System.Threading.Tasks;
using Application.Core.Models;
using Application.Core.Models.ViewModels;

namespace Application.Infrastructure.Repositories
{
    public interface IUserRepository
    {
        Task<Result> Login(LoginViewModel loginViewModel);
                                                
        Task<Result> Register(RegisterViewModel registerViewModel);
 
    }
}