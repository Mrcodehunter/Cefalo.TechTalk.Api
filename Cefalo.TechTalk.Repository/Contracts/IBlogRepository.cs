using Cefalo.TechTalk.Database.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cefalo.TechTalk.Repository.Contracts
{
    public interface IBlogRepository
    {
        Task<List<Blog>> GetAllAsync(int pageNumber,int pageSize);
        Task<Blog> CreateBlogAsync(Blog blog);
        Task<Blog> GetBlogByIdAsync(int id);
        Task<Blog> GetBlogByTitleAsync(string title);
        Task<List<Blog>> GetBlogsOfAuthorAsync(string username, int pageNumber, int pageSize);
        Task<Blog> UpdateBlogByIdAsync(Blog blog,int id); 
        Task<Boolean> DeleteBlogAsync(Blog blog);
        Task<int> CountAsync();
        Task<int> CountBlogsOfUserAsync(string username);
       
    }
}
