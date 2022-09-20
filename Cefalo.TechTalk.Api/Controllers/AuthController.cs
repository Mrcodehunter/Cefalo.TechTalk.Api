using AutoMapper;
using Cefalo.TechTalk.Service.Contracts;
using Cefalo.TechTalk.Service.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Cefalo.TechTalk.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
      

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost]
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
    }
}
