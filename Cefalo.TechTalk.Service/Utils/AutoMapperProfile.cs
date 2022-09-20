using AutoMapper;
using Cefalo.TechTalk.Database.Models;
using Cefalo.TechTalk.Service.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cefalo.TechTalk.Service.Utils
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<UserSignUpDto, User>();
            CreateMap<User, UserDetailsDto>();
                
        }
    }
}
