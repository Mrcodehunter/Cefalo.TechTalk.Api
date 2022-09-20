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
        Task<List<Blog>> GetAllAsync();
        Task<Blog> CreateBlogAsync(Blog blog);
        Task<Blog> GetBlogByIdAsync(int id);
        Task<Blog> GetBlogByTitleAsync(string title);
        Task<Blog> GetBlogByAuthorAsync(string author);
        Task<Blog> UpdateBlogAsync(Blog blog); 
        Task<Blog> DeleteBlogByIdAsync(int id);
       
    }
}
