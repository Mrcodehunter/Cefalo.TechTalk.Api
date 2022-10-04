using Cefalo.TechTalk.Database.Models;
using Cefalo.TechTalk.Service.Contracts;
using Cefalo.TechTalk.Service.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Cefalo.TechTalk.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }
        [HttpGet]
        public async Task<ActionResult<UserDetailsDto>> GetAllAsync()
        {
            return Ok(await _userService.GetAllAsync());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<UserDetailsDto>> GetUserByIdAsync(int id)
        {
            return Ok(await _userService.GetUserByIdAsync(id));
        }

        [HttpPut("{id}"), Authorize]
        public async Task<ActionResult<UserDetailsDto>> UpdateUserAsync(UserUpdateDto userUpdateDto, int id)
        {
            var user = await _userService.UpdateUserByIdAsync(userUpdateDto, id);
            return Ok(user); 
        }

        [HttpGet("username/{userName}")]
        public async Task<ActionResult<UserDetailsDto>> GetUserByUserNameAsync(string userName)
        {
            return Ok(await _userService.GetUserByUserNameAsync(userName));
        }



    }
}
