using AutoMapper;
using Cefalo.TechTalk.Database.Models;
using Cefalo.TechTalk.Repository.Contracts;
using Cefalo.TechTalk.Service.Contracts;
using Cefalo.TechTalk.Service.DTOs;
using Cefalo.TechTalk.Service.Utils.Contracts;
using Cefalo.TechTalk.Service.Utils.CustomErrorHandler;
using Cefalo.TechTalk.Service.Utils.DtoValidators;
using Microsoft.AspNetCore.Http;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using NPOI.OpenXmlFormats.Dml;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Cefalo.TechTalk.Service.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly ILoggerManager _loggerManager;
      
        private readonly IPasswordHandler _passwordHandler;
        private readonly IJwtHandler _jwtHandler;
        private readonly ICookieHandler _cookieHandler;
        private readonly BaseValidator<UserSignInDto> _userSignInDtoValidator;
        private readonly BaseValidator<UserSignUpDto> _userSignUpDtoValidator;
        private readonly BaseValidator<UserDetailsDto> _userDetailsDtoValidator;


        // private readonly BaseValidator<UserSignUpDto> _userSignUpDtoValidator;

        public AuthService(
            IUserRepository userRepository,
            IMapper mapper,
            ILoggerManager loggerManager,
            IPasswordHandler passwordHandler,
            IJwtHandler jwtHandler,
            ICookieHandler cookieHandler,
            BaseValidator<UserSignInDto> userSignInDtoValidator,
            BaseValidator<UserSignUpDto> userSignUpDtoValidator,
            BaseValidator<UserDetailsDto> userDetailsDtoValidator
            )

        {
            _userRepository = userRepository;
            _mapper = mapper;
            _loggerManager = loggerManager;
            _passwordHandler = passwordHandler;
            _jwtHandler = jwtHandler;
            _cookieHandler = cookieHandler;
            _userSignInDtoValidator = userSignInDtoValidator;
            _userDetailsDtoValidator = userDetailsDtoValidator;
            _userSignUpDtoValidator = userSignUpDtoValidator;
        }

       

        

        public async Task<UserDetailsDto> SignUpAsync(UserSignUpDto userSignUpDto)
        {

            _userSignUpDtoValidator.ValidateDto(userSignUpDto);

            _passwordHandler.CreatePasswordHash(userSignUpDto.Password, out byte[] passwordHash, out byte[] passwordSalt);
            
            User user = _mapper.Map<User>(userSignUpDto);

            user.CreatedAt = DateTime.UtcNow;
            user.ModifiedAt = DateTime.UtcNow;
            user.PasswordChangedAt = DateTime.UtcNow;
            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;
            
            User userResponse = await (_userRepository.CreateUserAsync(user));

            UserDetailsDto userDetailsDto = _mapper.Map<UserDetailsDto>(userResponse);
            userDetailsDto.Token = _jwtHandler.CreateToken(user);

            _cookieHandler.Set("Token", userDetailsDto.Token);

            _userDetailsDtoValidator.ValidateDto(userDetailsDto);

            return userDetailsDto;

        }
      
        public async Task<UserDetailsDto> SignInAsync(UserSignInDto userSignInDto)
        {
           _userSignInDtoValidator.ValidateDto(userSignInDto);

            User user = await _userRepository.GetUserByUserNameAsync(userSignInDto.UserName);
            if (_passwordHandler.VerifyPasswordHash(userSignInDto.Password, user.PasswordHash, user.PasswordSalt))
            {
                UserDetailsDto userDetailsDto = _mapper.Map<UserDetailsDto>(user);
                userDetailsDto.Token = _jwtHandler.CreateToken(user);

                _userDetailsDtoValidator.ValidateDto(userDetailsDto);
                _cookieHandler.Set("Token", userDetailsDto.Token);

                return userDetailsDto;
            }

            throw new BadRequestException("Username or password is incorrect");
        }

        public async Task<UserDetailsDto> VerifyTokenAsync()
        {

            if (!_jwtHandler.HttpContextExist()) throw new BadRequestException("No Token Provided");

            string userName = _jwtHandler.GetClaimName();
            User user = await _userRepository.GetUserByUserNameAsync(userName);
            //_cookieHandler.Set("Token", _cookieHandler.Get("Token"));
            UserDetailsDto userDetailsDto = _mapper.Map<UserDetailsDto>(user); 
            _userDetailsDtoValidator.ValidateDto(userDetailsDto);
            return userDetailsDto;
        }

        public Task<string> Logout()
        {
            _cookieHandler.Remove("Token");
            return null;
        }



    }
}
