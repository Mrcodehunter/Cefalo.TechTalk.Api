using Cefalo.TechTalk.Database.Context;
using Cefalo.TechTalk.Database.Models;
using Cefalo.TechTalk.Repository.Contracts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cefalo.TechTalk.Repository.Repositories
{
    public class BlogRepository : IBlogRepository
    {
        private readonly TechTalkContext _techTalkContext;
        public BlogRepository(TechTalkContext techTalkContext)
        {
            _techTalkContext = techTalkContext;
        }

        public async Task<Blog> CreateBlogAsync(Blog blog)
        {
            _techTalkContext.Blogs.Add(blog);
            await _techTalkContext.SaveChangesAsync();
            return blog;
        }
        public async Task<List<Blog>> GetAllAsync()
        {
            return await _techTalkContext.Blogs.ToListAsync();
        }

        public async Task<Blog> GetBlogByIdAsync(int id)
        {
            return await (_techTalkContext.Blogs.FindAsync(id));
        }
        public async Task<Blog> GetBlogByTitleAsync(string title)
        {
            return await (_techTalkContext.Blogs.FirstOrDefaultAsync(x => x.Title == title));
        }
        public async Task<Blog> GetBlogByAuthorAsync(string author)
        {
            return await (_techTalkContext.Blogs.FirstOrDefaultAsync(x => x.AuthorName == author));
            //return await (_techTalkContext.Blogs.(x => x.Author == author));
        }
        public async Task<Blog> UpdateBlogAsync(Blog blog)
        {
            Blog blog1 = await _techTalkContext.Blogs.FirstOrDefaultAsync(x => x.Id == blog.Id);
            blog1.Title = blog.Title;   
            blog1.AuthorName = blog.AuthorName; 
            blog1.Body = blog.Body;
            await _techTalkContext.SaveChangesAsync();
            return blog1;
        }
        public async Task<Blog> DeleteBlogByIdAsync(int id)
        {
            var blog = await _techTalkContext.Blogs.FindAsync(id);
            _techTalkContext.Blogs.Remove(blog);
            await _techTalkContext.SaveChangesAsync();
            return null;
        }
    }
}
