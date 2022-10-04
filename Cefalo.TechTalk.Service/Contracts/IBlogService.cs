using Cefalo.TechTalk.Database.Models;
using Cefalo.TechTalk.Service.DTOs;
using Cefalo.TechTalk.Service.Utils.Wrappers;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cefalo.TechTalk.Service.Contracts
{
    public interface IBlogService
    {
        Task<PagedResponse<List<BlogDetailsDto>>> GetAllAsync(PaginationFilter filter);
        Task<BlogDetailsDto> CreateBlogAsync(BlogPostDto blog);
        Task<BlogDetailsDto> GetBlogByIdAsync(int id);
        Task<BlogDetailsDto> GetBlogByTitleAsync(string title);
        Task<PagedResponse<List<BlogDetailsDto>>> GetBlogsOfAuthorAsync(string username, PaginationFilter filter);
        Task<BlogDetailsDto> UpdateBlogByIdAsync(BlogUpdateDto blog,int id);
        Task<Boolean> DeleteBlogByIdAsync(int id);
    }
}
