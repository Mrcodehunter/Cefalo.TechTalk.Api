
using Cefalo.TechTalk.Database.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Cefalo.TechTalk.Repository.Contracts
{
    public interface IUserRepository
    {

        Task<User> CreateUserAsync(User user);
        Task<List<User>> GetAllAsync();
        Task<User> GetUserByIdAsync(int id);
       // Task<User> UpdateUserAsync(User user);


        Task<User> GetUserByNameAsync(string name);
        Task<User> GetUserByEmailAsync(string email);   
        Task<User> GetUserByUserNameAsync(string userName);

       
    }
}
