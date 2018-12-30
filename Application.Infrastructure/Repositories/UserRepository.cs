using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Core.Models;
using Application.Core.Models.ViewModels;
using Application.Infrastructure.DAL;
using MongoDB.Driver;

namespace Application.Infrastructure.Repositories
{
    public class UserRepository : DocumentDbRepository<Users>, IUserRepository
    {
        public UserRepository()
        {
            base.Initialize();
            _collection = database.GetCollection<Users>(nameof(Users));
        }

        public async Task<Result> Login(LoginViewModel loginViewModel)
        {
            var result = new Result() {IsSuccess = false};
            try
            {
                var user = Get(x => x.UserName == loginViewModel.username &&
                                    x.Password == loginViewModel.password).Result
                    ?.FirstOrDefault();
                if (user == null)
                {
                    result.ReturnMessage = "User can not found";
                    result.IsSuccess = false;
                    return result;
                }

                user.LastLoginDate = DateTime.Now;

                var filter = Builders<Users>.Filter.Eq(x => x.Id, user.Id);

                await Update(filter, user);

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
            /*try
            {
                if (!model.password.Equals(model.passwordRepeat))
                {
                    result.ReturnMessage = "Password and password repeat did not match";
                    result.IsSuccess = false;
                    return result;
                }

                var user = DocumentDbRepository<Users>.GetItemsAsync(x => x.UserName == model.username).Result
                    ?.FirstOrDefault();
                if (user == null)
                {
                    await DocumentDbRepository<Users>.CreateItemAsync(new Users
                    {
                        Name = model.name, Password = model.password, CreationDate = DateTime.Now,
                        SurName = model.surname, UserName = model.username
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
            }*/
            return result;
        }
    }
}