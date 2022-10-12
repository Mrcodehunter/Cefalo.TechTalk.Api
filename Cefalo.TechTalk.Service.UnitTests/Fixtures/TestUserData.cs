using Cefalo.TechTalk.Database.Models;
using Cefalo.TechTalk.Service.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cefalo.TechTalk.Service.UnitTests.Fixtures
{
    public class TestUserData
    {
        private readonly List<User> users = new List<User>();

        public TestUserData()
        {
            for(int i = 0; i < 10; i++)
                users.Add(CreateDummyUser(i));
        }

        public virtual List<User> GetAllUsers() { return users; }

        public virtual UserUpdateDto GetUserUpdateDto()
        {
            UserUpdateDto user = new UserUpdateDto()
            {

                Name = "asdad",

                UserName = "asd",

                Password = "1234abcd",

                Email = "abcd@gmail.com",

            };

            return user;

        }

       public virtual UserDetailsDto CreateUserDetailsDtoObject(int id)
        {
            UserDetailsDto userDetailsDto = new UserDetailsDto()
            {
               Id = users[id].Id,

                Name = users[id].Name,

                UserName = users[id].UserName,

                Email = users[id].Email,

            };
            return userDetailsDto;
       }

        public virtual User GetUser(int id)
        {
            return users[id];
        }
        public User CreateDummyUser(int id)
        {
            User user = new User(){

                Id = id,

                Name = "asdad",

                UserName = "asd",

                PasswordHash = null,

                PasswordSalt = null,

                Email = "abcd@gmail.com",

                CreatedAt = DateTime.UtcNow,

                ModifiedAt = DateTime.UtcNow,

                PasswordChangedAt = DateTime.UtcNow
            };

            return user;
        }

        
    }
}
