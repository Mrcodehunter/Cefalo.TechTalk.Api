using Cefalo.TechTalk.Database.Models;
using Cefalo.TechTalk.Service.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cefalo.TechTalk.Service.Contracts
{
    public interface IBlogService
    {
        Task<List<BlogDetailsDto>> GetAllAsync();
        Task<BlogDetailsDto> CreateBlogAsync(BlogPostDto blog);
        Task<BlogDetailsDto> GetBlogByIdAsync(int id);
        Task<BlogDetailsDto> GetBlogByTitleAsync(string title);
        Task<BlogDetailsDto> GetBlogByAuthorAsync(string author);
        Task<BlogDetailsDto> UpdateBlogByIdAsync(BlogUpdateDto blog,int id);
        Task<Boolean> DeleteBlogByIdAsync(int id);
    }
}
