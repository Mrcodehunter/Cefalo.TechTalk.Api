using AutoMapper;
using Cefalo.TechTalk.Database.Context;
using Cefalo.TechTalk.Database.Models;
using Cefalo.TechTalk.Repository.Contracts;
using Cefalo.TechTalk.Service.Contracts;
using Cefalo.TechTalk.Service.DTOs;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cefalo.TechTalk.Service.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        

        public UserService(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }


        public async Task<List<UserDetailsDto>> GetAllAsync()
        {
            List<User> users = await _userRepository.GetAllAsync();
            List<UserDetailsDto> userDetails = _mapper.Map<List<UserDetailsDto>>(users);
            return userDetails;
        }

        public async Task<UserDetailsDto> GetUserByIdAsync(int id)
        {
            User user = await (_userRepository.GetUserByIdAsync(id));
            return _mapper.Map<UserDetailsDto>(user);
           
        }



        public async Task<UserDetailsDto> GetUserByNameAsync(string name)
        {
            User user = await (_userRepository.GetUserByNameAsync(name));
            return _mapper.Map<UserDetailsDto>(user);
        }
        public async Task<UserDetailsDto> GetUserByEmailAsync(string email)
        {
           User user = await (_userRepository.GetUserByEmailAsync(email));
            return _mapper.Map<UserDetailsDto>(user);
        }
        public async Task<UserDetailsDto> GetUserByUserNameAsync(string userName)
        {
           User user = await (_userRepository.GetUserByUserNameAsync(userName));
           return _mapper.Map<UserDetailsDto>(user);
        }
    }
}
