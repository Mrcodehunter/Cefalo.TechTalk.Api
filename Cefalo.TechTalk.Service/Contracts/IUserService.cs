using Cefalo.TechTalk.Database.Models;
using Cefalo.TechTalk.Service.DTOs;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cefalo.TechTalk.Service.Contracts
{
    public interface IUserService
    {
        Task<List<UserDetailsDto>> GetAllAsync();
        Task<UserDetailsDto> GetUserByIdAsync(int id);
        Task<UserDetailsDto> UpdateUserByIdAsync(UserUpdateDto user,int id);
        Task<UserDetailsDto> GetUserByNameAsync(string name);
        Task<UserDetailsDto> GetUserByEmailAsync(string email);
        Task<UserDetailsDto> GetUserByUserNameAsync(string userName);

    }
}
