using AutoMapper;
using Cefalo.TechTalk.Service.Contracts;
using Cefalo.TechTalk.Service.DTOs;
using Cefalo.TechTalk.Service.Utils.Contracts;
using Cefalo.TechTalk.Service.Utils.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Cefalo.TechTalk.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly ICookieHandler _cookieHandler;
        private readonly IJwtHandler _jwtHandler;

        public AuthController(IAuthService authService, ICookieHandler cookieHandler, IJwtHandler jwtHandler)
        {
            _authService = authService;
            _cookieHandler = cookieHandler;
            _jwtHandler = jwtHandler;   
        }

        [HttpPost("signup")]
        public async Task<ActionResult<UserDetailsDto>> SignUpAsync(UserSignUpDto userSignUpDto)
        {
            var user = await _authService.SignUpAsync(userSignUpDto);
            return Ok(user);
        }

        [HttpPost("signin")]
        public async Task<ActionResult<UserDetailsDto>> SignInAsync(UserSignInDto userSignInDto)
        {
            var user = await _authService.SignInAsync(userSignInDto);
            return Ok(user);
        }

        [HttpPost("verify"),Authorize]
        public async Task<ActionResult<UserDetailsDto>> VerifyToken()
        {
           
            UserDetailsDto userDetailsDto = await _authService.VerifyTokenAsync();
            return Ok(userDetailsDto);
        }
        [HttpPost("logout"),Authorize]
        public async Task<ActionResult<string>> Logout()
        {
            
            return Ok(_authService.Logout());
        }
    }
}
