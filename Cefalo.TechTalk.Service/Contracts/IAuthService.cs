using Cefalo.TechTalk.Database.Models;
using Cefalo.TechTalk.Service.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Cefalo.TechTalk.Service.Contracts
{
    public interface IAuthService
    {
        Task<UserDetailsDto> SignUpAsync(UserSignUpDto userSignUpDto);
        Task<UserDetailsDto> SignInAsync(UserSignInDto userSignInDto);
        Task<UserDetailsDto> VerifyTokenAsync();
        Task<string> Logout();
        //Task<string> SignOutAsync();
    }
}
