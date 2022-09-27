using Cefalo.TechTalk.Database.Models;
using Cefalo.TechTalk.Service.Contracts;
using Cefalo.TechTalk.Service.DTOs;
using Cefalo.TechTalk.Service.Services;
using Microsoft.AspNetCore.Authorization;
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
        public async Task<ActionResult<BlogDetailsDto>> GetAllAsync()
        {
            return Ok(await _blogService.GetAllAsync());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<BlogDetailsDto>> GetBlogByIdAsync(int id)
        {
            return Ok(await _blogService.GetBlogByIdAsync(id));
        }
        [HttpPost, Authorize]
        public async Task<ActionResult<BlogDetailsDto>> CreateBlogAsync(BlogPostDto blog)
        {

            var blog2 = await _blogService.CreateBlogAsync(blog);
            return Ok(blog2);
        }
        [HttpPut("{id}"), Authorize]
        public async Task<ActionResult<BlogDetailsDto>> UpdateBlogById(BlogUpdateDto blogUpdateDto,int id)
        {
            BlogDetailsDto blogDetailsDto = await _blogService.UpdateBlogByIdAsync(blogUpdateDto, id);
            return Ok(blogDetailsDto);
        }

        [HttpDelete("{id}"), Authorize]
        public async Task<ActionResult<BlogDetailsDto>> DeleteBlogByIdAsync(int id)
        {
            await _blogService.DeleteBlogByIdAsync(id);
            return Ok();
        }
    }
}
