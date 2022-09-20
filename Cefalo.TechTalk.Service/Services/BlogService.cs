using Cefalo.TechTalk.Database.Context;
using Cefalo.TechTalk.Database.Models;
using Cefalo.TechTalk.Repository.Contracts;
using Cefalo.TechTalk.Service.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cefalo.TechTalk.Service.Services
{
    public class BlogService : IBlogService
    {
        private readonly IBlogRepository _blogRepository;

        public BlogService(IBlogRepository blogRepository)
        {
            _blogRepository = blogRepository;   
        }

        public async Task<Blog> CreateBlogAsync(Blog blog)
        {
            Blog blog2 = await _blogRepository.CreateBlogAsync(blog);
            return blog2;
        }
        public async Task<List<Blog>> GetAllAsync()
        {
            return await _blogRepository.GetAllAsync();   
        }
        public async Task<Blog> GetBlogByIdAsync(int id)
        {
            return await _blogRepository.GetBlogByIdAsync(id);
        }
        public async Task<Blog> GetBlogByTitleAsync(string title)
        {
            var blog = await _blogRepository.GetBlogByTitleAsync(title);
            return blog;
        }
        public async Task<Blog> GetBlogByAuthorAsync(string author)
        {
            var blog = await _blogRepository.GetBlogByAuthorAsync(author);
            return blog;
        }
        public async Task<Blog> DeleteBlogByIdAsync(int id)
        {
            return await _blogRepository.DeleteBlogByIdAsync(id);   
        }
    }
}
