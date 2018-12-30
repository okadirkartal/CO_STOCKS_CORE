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
       

        public async Task<Result> Login(Users model)
        {
            var result = new Result() {IsSuccess = true};
            try
            {
                var user = Get(x => x.UserName == model.UserName &&
                                    x.Password == model.Password).Result
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
                result.Model = user; 
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return result;
        }

        public async Task<Result> Register(Users model)
        {
            var result = new Result() {IsSuccess = false};
            try
            {


                var user = await Get(x => x.UserName == model.UserName);
                   
                if (user?.FirstOrDefault() == null)
                {
                    var newUser = new Users
                    {
                        Name = model.Name, Password = model.Password, CreationDate = DateTime.Now,
                        SurName = model.SurName, UserName = model.UserName
                    };
                    
                     await Create(newUser);
                    result.IsSuccess = true;
                    result.ReturnMessage = "User added";
                    result.Model = user;
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
        }
    }
}