using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Application.Core.Models;
using Application.Core.Models.ViewModels;
using Application.Infrastructure.DAL;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Application.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly StockContext _context = null;

        public StockRepository(IOptions<Settings> settings)
        {
            _context = new StockContext(settings);
        }

        public async Task<IEnumerable<Stocks>> GetAllStocks(string userId)
        {
            try
            {
                if (!string.IsNullOrEmpty(userId))
                {
                    return await _context.Stocks.Find(x => x.UserId == userId).ToListAsync();
                }

                return null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<Result> Login(LoginViewModel loginViewModel)
        {
            var result = new Result() {IsSuccess = false};
            try
            {
                var user = await _context.Users.Find(x => x.UserName == loginViewModel.username &&
                                                          x.Password == loginViewModel.password).FirstOrDefaultAsync();
                if (user == null)
                {
                    result.ReturnMessage = "User can not found";
                    result.IsSuccess = false;
                    return result;
                }

                user.LastLoginDate = DateTime.Now;
                ReplaceOneResult actionResult =
                    await _context.Users.ReplaceOneAsync(n => n.Id.Equals(user.Id), user,
                        new UpdateOptions {IsUpsert = true});
                result.ReturnMessageList = new List<string>();
                result.ReturnMessageList.Add(user.Id.ToString());
                result.ReturnMessageList.Add(user.UserName);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return result;
        }

        public async Task<Result> Register(RegisterViewModel model)
        {
            var result = new Result() {IsSuccess = false};
            try
            {
                if (!model.password.Equals(model.passwordRepeat))
                {
                    result.ReturnMessage = "Password and password repeat did not match";
                    result.IsSuccess = false;
                    return result;
                }

                var user = await _context.Users.Find(x => x.UserName == model.username).FirstOrDefaultAsync();
                if (user == null)
                {
                    await _context.Stocks.InsertOneAsync(new Users
                    {
                        Name = model.name, Password = model.password, CreationDate = DateTime.Now,
                        SurName = model.surname,UserName = model.username
                    });
                    result.IsSuccess = true;
                    result.ReturnMessage = "Stock added";
                }
                else
                {
                    result.ReturnMessage = "This username is exists";
                    result.IsSuccess = false;
                }

                return result;

            }
            catch (Exception ex)
            {
                throw ex;
            }

            return result;
        }
    }
}