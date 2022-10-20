using AutoMapper.Execution;
using Cefalo.TechTalk.Database.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Cefalo.TechTalk.Repository.UnitTests.Fixtures
{
    public class TestUserRepositoryData
    {
        private readonly DateTime _dateTime;
        public TestUserRepositoryData() {
            _dateTime =  DateTime.UtcNow;
        }

        public virtual User CreateUser(int id)
        {
            return new User()
            {

                Id = id,

                Name = "asdad",
                
                UserName = "asd",
                
                PasswordHash =  Enumerable.Repeat((byte)0x20,  100 ).ToArray(),

                PasswordSalt = Enumerable.Repeat((byte)0x20, 100).ToArray(),

                Email = "abcd@gmail.com",

                CreatedAt = _dateTime,

                ModifiedAt = _dateTime,

                PasswordChangedAt = _dateTime
            };
        }
    }
}
