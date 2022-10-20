using Cefalo.TechTalk.Service.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cefalo.TechTalk.Api.UnitTests.Fixtures
{
    public class TestUserControllerData
    {
        private readonly List<UserDetailsDto> userList =new List<UserDetailsDto>();
        public TestUserControllerData() 
        {
           for(int i = 0; i < 10; i++)
                userList.Add( this.GetUserDetailsDto(i) );
        }

        public virtual List<UserDetailsDto> GetUserList()
        {
            return userList;
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
        public virtual UserDetailsDto GetUserDetailsDtoByUserNameAndId(int id, string userName)
        {
            return new UserDetailsDto()
            {
                Id = id,

                Name = "asdad",

                UserName = userName,

                Email = "abcd@gmail.com",

                Token = "A-Token"

            };

        }


        public virtual UserUpdateDto GetUserUpdateDto()
        {
            UserUpdateDto user = new UserUpdateDto()
            {

                Name = "asdad",

                UserName = "asd",

                Password = null,

                Email = "abcd@gmail.com",

            };

            return user;

        }


    }
}
