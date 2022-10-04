using Cefalo.TechTalk.Database.Context;
using Cefalo.TechTalk.Database.Models;
using Cefalo.TechTalk.Repository.Contracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cefalo.TechTalk.Repository.Repositories
{
   
    public class UserRepository : IUserRepository
    {
        private readonly TechTalkContext _techTalkContext;
        public UserRepository(TechTalkContext techTalkContext)
        {
            _techTalkContext = techTalkContext;
        }


        public async Task<User> CreateUserAsync(User user)
        {
            _techTalkContext.Users.Add(user);
            await _techTalkContext.SaveChangesAsync();
            return user;
        }

        public async Task<List<User>> GetAllAsync()
        {
            return await _techTalkContext.Users.ToListAsync();
        }

        public async Task<User> GetUserByIdAsync(int id)
        {
            return await _techTalkContext.Users.FindAsync(id);
        }

        public async Task<User> UpdateUserByIdAsync(User user, int id)
        {
            User user2 = await _techTalkContext.Users.FindAsync(id);
            user2.Name = user.Name;
            user2.UserName = user.UserName;
            user2.Email = user.Email;
            if(user.PasswordChangedAt != null)
            {
                user2.PasswordHash = user.PasswordHash;
                user2.PasswordSalt = user.PasswordSalt;
                user2.PasswordChangedAt = user.PasswordChangedAt;
            }
            user2.ModifiedAt = user.ModifiedAt;
            

            await _techTalkContext.SaveChangesAsync();

            return user2;
        }

       


        public async Task<User> GetUserByNameAsync(string name)
        {
           return await _techTalkContext.Users.FirstOrDefaultAsync(x => x.Name == name);
        }
        public async Task<User> GetUserByEmailAsync(string email)
        {
            return await _techTalkContext.Users.FirstOrDefaultAsync(x => x.Email == email);
        }
        public async Task<User> GetUserByUserNameAsync(string userName)
        {
            return await _techTalkContext.Users.FirstOrDefaultAsync(x => x.UserName == userName);
        }

    }
}
