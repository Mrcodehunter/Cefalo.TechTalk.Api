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
        public async Task<ActionResult> GetAllAsync()
        {
            var users = await _userService.GetAllAsync();
            return Ok(users);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetUserByIdAsync(int id)
        {
            var user = await _userService.GetUserByIdAsync(id);
            return Ok(user);
        }

        [HttpPut("{id}"), Authorize]
        public async Task<ActionResult> UpdateUserAsync(UserUpdateDto userUpdateDto, int id)
        {
            var user = await _userService.UpdateUserByIdAsync(userUpdateDto, id);
            return Ok(user); 
        }

        [HttpGet("username/{userName}")]
        public async Task<ActionResult> GetUserByUserNameAsync(string userName)
        {
            var user = await _userService.GetUserByUserNameAsync(userName);
            return Ok(user);
        }



    }
}
