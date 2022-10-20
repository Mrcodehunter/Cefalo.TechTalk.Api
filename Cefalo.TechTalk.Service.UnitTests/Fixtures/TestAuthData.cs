using AutoMapper.Configuration.Conventions;
using Cefalo.TechTalk.Database.Models;
using Cefalo.TechTalk.Service.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cefalo.TechTalk.Service.UnitTests.Fixtures
{
    public class TestAuthData
    { 
        private readonly DateTime _dateTime;
        public TestAuthData()
        {
            _dateTime = DateTime.UtcNow;
        }

        public virtual DateTime GetDateTime() { return _dateTime; }

        public virtual Tuple<byte[], byte[]> GetAByteTuple()
        {
            return new Tuple<byte[], byte[]>(null, null);
        }

        public virtual UserDetailsDto GetUserDetailsDtoObject(int id)
        {
            UserDetailsDto userDetailsDto = new UserDetailsDto()
            {
                Id = id,

                Name = "asdad",

                UserName = "asd",

                Email = "abcd@gmail.com",

                Token = "A-Token"

            };
            return userDetailsDto;
        }

        public virtual User GetCallableUser()
        {
            User user = new User()
            {
                Id = 0,

                Name = "asdad",

                UserName = "asd",

                PasswordHash = null,

                PasswordSalt = null,

                Email = "abcd@gmail.com",

                CreatedAt = _dateTime,

                ModifiedAt = _dateTime,

                PasswordChangedAt = _dateTime
            };

            return user;
        }
        public virtual User GetReturnableUser(int id)
        {
            User user = new User()
            {

                Id = id,

                Name = "asdad",

                UserName = "asd",

                PasswordHash = null,

                PasswordSalt = null,

                Email = "abcd@gmail.com",

                CreatedAt = _dateTime,

                ModifiedAt = _dateTime,

                PasswordChangedAt = _dateTime
            };

            return user;
        }

        public virtual UserSignUpDto GetUserSignUpDto()
        {
            return new UserSignUpDto()
            {
                Name = "asdad",

                UserName = "asd",

                Password = "abcd1234",

                Email = "abcd@gmail.com"
            };
        }

        public virtual UserSignInDto GetUserSignInDto()
        {
            return new UserSignInDto()
            {
                UserName = "asd",

                Password = "abcd1234",
            };
        }

       
    }
}
