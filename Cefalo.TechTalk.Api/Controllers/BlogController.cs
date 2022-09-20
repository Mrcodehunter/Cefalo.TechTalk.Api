using Cefalo.TechTalk.Database.Models;
using Cefalo.TechTalk.Service.Contracts;
using Cefalo.TechTalk.Service.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Cefalo.TechTalk.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlogController : ControllerBase
    {
        private readonly IBlogService _blogService;

        public BlogController(IBlogService blogService)
        {
            _blogService = blogService;
        }

        [HttpGet]
        public async Task<ActionResult<Blog>> GetAllAsync()
        {
            return Ok(await _blogService.GetAllAsync());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Blog>> GetBlogByIdAsync(int id)
        {
            return Ok(await _blogService.GetBlogByIdAsync(id));
        }
        [HttpPost]
        public async Task<ActionResult<Blog>> CreateBlogAsync(Blog blog)
        {

            var blog2 = await _blogService.CreateBlogAsync(blog);
            return Ok(blog2);
        }
    }
}
