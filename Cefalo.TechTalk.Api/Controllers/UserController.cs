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

        [HttpPut("{userName}"), Authorize]
        public async Task<ActionResult<UserDetailsDto>> UpdateUserAsync(UserUpdateDto userUpdateDto, string userName)
        {
            var user = await _userService.UpdateUserAsync(userUpdateDto, userName);
            return Ok(user); 
        }
        
       

    }
}
