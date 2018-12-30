using System.Threading.Tasks;
using Application.Core.Models;
namespace Application.Infrastructure.Repositories
{
    public interface IUserRepository
    {
        Task<Result> Login(Users model);

        Task<Result> Register(Users model);
    }
}