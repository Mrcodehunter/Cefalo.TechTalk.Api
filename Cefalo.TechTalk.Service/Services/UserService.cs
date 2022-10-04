﻿using AutoMapper;
using Cefalo.TechTalk.Database.Context;
using Cefalo.TechTalk.Database.Models;
using Cefalo.TechTalk.Repository.Contracts;
using Cefalo.TechTalk.Service.Contracts;
using Cefalo.TechTalk.Service.DTOs;
using Cefalo.TechTalk.Service.Utils.Contracts;
using Cefalo.TechTalk.Service.Utils.CustomErrorHandler;
using Cefalo.TechTalk.Service.Utils.DtoValidators;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Cefalo.TechTalk.Service.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly IPasswordHandler _passwordHandler;
        private readonly IJwtHandler _jwtHandler;
        private readonly BaseValidator<UserDetailsDto> _userDetailsDtoValidator;
        private readonly BaseValidator<UserUpdateDto> _userUpdateDtoValidator;


        public UserService(
            IUserRepository userRepository,
            IMapper mapper,
            IPasswordHandler passwordHandler,
            IJwtHandler jwtHandler,
            BaseValidator<UserDetailsDto> userDetailsDtoValidator,
            BaseValidator<UserUpdateDto> userUpdateDtoValidator
            )
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _passwordHandler = passwordHandler;
            _jwtHandler = jwtHandler;
            _userDetailsDtoValidator = userDetailsDtoValidator;
            _userUpdateDtoValidator = userUpdateDtoValidator;
        }


        public async Task<List<UserDetailsDto>> GetAllAsync()
        {
            List<User> users = await _userRepository.GetAllAsync();
            if (users == null) throw new NotFoundException("No User Found!");

            List<UserDetailsDto> userDetails = _mapper.Map<List<UserDetailsDto>>(users);
            foreach(var user in userDetails) _userDetailsDtoValidator.ValidateDto(user);

            return userDetails;
        }

        public async Task<UserDetailsDto> GetUserByIdAsync(int id)
        {
            User user = await (_userRepository.GetUserByIdAsync(id));
            if (user == null) throw new NotFoundException("No user Exists!");

            UserDetailsDto userDetails = _mapper.Map<UserDetailsDto>(user);
            _userDetailsDtoValidator.ValidateDto(userDetails);

            return userDetails;
           
        }

        public async Task<UserDetailsDto> UpdateUserByIdAsync(UserUpdateDto user, int id)
        {
            if (!_jwtHandler.HttpContextExist()) throw new UnAuthorizedException("Unauthorized");
            if (id.ToString() != _jwtHandler.GetClaimId() ) throw new UnAuthorizedException("Unauthorized");

            _userUpdateDtoValidator.ValidateDto(user);


            User user2 = _mapper.Map<User>(user);

            if (user.Password != null) {
                _passwordHandler.CreatePasswordHash(user.Password, out byte[] passwordHash, out byte[] passwordSalt);
                user2.PasswordSalt = passwordSalt;
                user2.PasswordHash = passwordHash;
                user2.PasswordChangedAt = DateTime.UtcNow;
                
            }
            user2.ModifiedAt = DateTime.UtcNow;

            User updatedUser = await _userRepository.UpdateUserByIdAsync(user2,id);
           
            UserDetailsDto userDetailsDto = _mapper.Map<UserDetailsDto>(updatedUser);
            userDetailsDto.Token = _jwtHandler.CreateToken(updatedUser);

            _userDetailsDtoValidator.ValidateDto(userDetailsDto);

            return userDetailsDto;
         }



        public async Task<UserDetailsDto> GetUserByNameAsync(string name)
        {
            User user = await (_userRepository.GetUserByNameAsync(name));
            UserDetailsDto userDetails = _mapper.Map<UserDetailsDto>(user);
            _userDetailsDtoValidator.ValidateDto(userDetails);
            return userDetails;
        }
        public async Task<UserDetailsDto> GetUserByEmailAsync(string email)
        {
           User user = await (_userRepository.GetUserByEmailAsync(email));
            UserDetailsDto userDetails = _mapper.Map<UserDetailsDto>(user);
            _userDetailsDtoValidator.ValidateDto(userDetails);
            return userDetails;
        }
        public async Task<UserDetailsDto> GetUserByUserNameAsync(string userName)
        {
           User user = await (_userRepository.GetUserByUserNameAsync(userName));
            UserDetailsDto userDetails = _mapper.Map<UserDetailsDto>(user);
            _userDetailsDtoValidator.ValidateDto(userDetails);
            return userDetails;
        }
    }
}
