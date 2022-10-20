using Cefalo.TechTalk.Service.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cefalo.TechTalk.Api.UnitTests.Fixtures
{
    public class TestAuthControllerData
    {
        public TestAuthControllerData() { }

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

        public virtual UserDetailsDto GetUserDetailsDto(int id)
        {
            return new UserDetailsDto()
            {
                Id = id,

                Name = "asdad",

                UserName = "asd",

                Email = "abcd@gmail.com",

                Token = "A-Token"

            };
            
        }
    }
}
