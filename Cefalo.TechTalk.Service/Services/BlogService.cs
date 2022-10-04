using AutoMapper;
using Cefalo.TechTalk.Database.Context;
using Cefalo.TechTalk.Database.Models;
using Cefalo.TechTalk.Repository.Contracts;
using Cefalo.TechTalk.Repository.Repositories;
using Cefalo.TechTalk.Service.Contracts;
using Cefalo.TechTalk.Service.DTOs;
using Cefalo.TechTalk.Service.Utils.Contracts;
using Cefalo.TechTalk.Service.Utils.CustomErrorHandler;
using Cefalo.TechTalk.Service.Utils.DtoValidators;
using Cefalo.TechTalk.Service.Utils.Services;
using Cefalo.TechTalk.Service.Utils.Wrappers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NLog.LayoutRenderers;
using Org.BouncyCastle.Asn1.Ocsp;
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
        private readonly IMapper _mapper;
        private readonly IJwtHandler _jwtHandler;
        private readonly IUriService _uriService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly BaseValidator<BlogDetailsDto> _blogDetailsDtoValidator;
        private readonly BaseValidator<BlogPostDto> _blogPostDtoValidator;
        private readonly BaseValidator<BlogUpdateDto> _blogUpdateDtoValidator;

        public BlogService(
            IBlogRepository blogRepository,
            IMapper mapper,
            IJwtHandler jwtHandler,
            IUriService uriService,
            IHttpContextAccessor httpContextAccessor,
            BaseValidator<BlogDetailsDto> blogDetailsDtoValidator,
            BaseValidator<BlogPostDto> blogPostDtoValidator,
            BaseValidator<BlogUpdateDto> blogUpdateDtoValidator
            )
        {
            _blogRepository = blogRepository;
            _mapper = mapper;
            _jwtHandler = jwtHandler;
            _uriService = uriService;
            _httpContextAccessor = httpContextAccessor;
            _blogDetailsDtoValidator = blogDetailsDtoValidator;
            _blogUpdateDtoValidator = blogUpdateDtoValidator;
            _blogPostDtoValidator = blogPostDtoValidator;
        }

        public async Task<BlogDetailsDto> CreateBlogAsync(BlogPostDto blog)
        {
            _blogPostDtoValidator.ValidateDto(blog);

            Blog blog1 = _mapper.Map<Blog>(blog);
            blog1.AuthorName = _jwtHandler.GetClaimName();
            blog1.AuthorId = Convert.ToInt32(_jwtHandler.GetClaimId());
            blog1.CreatedAt = DateTime.UtcNow.AddMinutes(-1) ;
            blog1.ModifiedAt = DateTime.UtcNow.AddMinutes(-1);
           
            Blog blog2 = await _blogRepository.CreateBlogAsync(blog1);
            BlogDetailsDto blogDetails = _mapper.Map<BlogDetailsDto>(blog2);

            
            return blogDetails;
        }
        public async Task<PagedResponse<List<BlogDetailsDto>>> GetAllAsync(PaginationFilter filter)
        {
            var route = _httpContextAccessor.HttpContext.Request.Path.Value;
            var validFilter = new PaginationFilter(filter.PageNumber, filter.PageSize);

            List<Blog> blogs = await _blogRepository.GetAllAsync(validFilter.PageNumber, validFilter.PageSize);
            if (blogs == null) throw new NotFoundException("No Story Found!");
            List<BlogDetailsDto> blogDetails = _mapper.Map<List<BlogDetailsDto>>(blogs);
            var totalRecords = await _blogRepository.CountAsync();

            var pagedReponse = PaginationHandler.CreatePagedReponse<BlogDetailsDto>(blogDetails, validFilter, totalRecords, _uriService, route);
            return pagedReponse;
        }
        public async Task<BlogDetailsDto> GetBlogByIdAsync(int id)
        {
            Blog blog = await _blogRepository.GetBlogByIdAsync(id);
            if (blog == null) throw new NotFoundException("No Story Found!");

            BlogDetailsDto blogDetails = _mapper.Map<BlogDetailsDto>(blog);
            

            return blogDetails;
        }
        public async Task<BlogDetailsDto> GetBlogByTitleAsync(string title)
        {
            var blog = await _blogRepository.GetBlogByTitleAsync(title);
            if (blog == null) throw new NotFoundException("No Story Found!");

            BlogDetailsDto blogDetails = _mapper.Map<BlogDetailsDto>(blog);
            _blogDetailsDtoValidator.ValidateDto(blogDetails);

            return blogDetails;
        }
        public async Task<PagedResponse<List<BlogDetailsDto>>> GetBlogsOfAuthorAsync(string username, PaginationFilter filter)
        {
            var route = _httpContextAccessor.HttpContext.Request.Path.Value;
            var validFilter = new PaginationFilter(filter.PageNumber, filter.PageSize);

            List<Blog> blogs = await _blogRepository.GetBlogsOfAuthorAsync(username,  validFilter.PageNumber, validFilter.PageSize);
            
            if (blogs == null) throw new NotFoundException("No Story Found!");
            List<BlogDetailsDto> blogDetails = _mapper.Map<List<BlogDetailsDto>>(blogs);
            var totalRecords = await _blogRepository.CountBlogsOfUserAsync(username);

            var pagedReponse = PaginationHandler.CreatePagedReponse<BlogDetailsDto>(blogDetails, validFilter, totalRecords, _uriService, route);
            return pagedReponse;

        }

        public async Task<BlogDetailsDto> UpdateBlogByIdAsync(BlogUpdateDto blog,int id)
        {



            if (!_jwtHandler.HttpContextExist()) throw new UnAuthorizedException("Not Authorized");
            Blog blog1 = await _blogRepository.GetBlogByIdAsync(id);
            if (blog1 == null) throw new NotFoundException("No Story Found With such id!");

            if (blog1.AuthorId.ToString() != _jwtHandler.GetClaimId()) throw new UnAuthorizedException("Not Authorized");

            _blogUpdateDtoValidator.ValidateDto(blog);

            Blog blog2 = _mapper.Map<Blog>(blog);
            Blog updatedBlog = await _blogRepository.UpdateBlogByIdAsync(blog2, id);

            BlogDetailsDto blogDetails = _mapper.Map<BlogDetailsDto>(updatedBlog);
            

            return blogDetails;
        }
        public async Task<Boolean> DeleteBlogByIdAsync(int id)
        {
            if (!_jwtHandler.HttpContextExist()) throw new UnAuthorizedException("Not Authorized");
            Blog blog = await _blogRepository.GetBlogByIdAsync(id);
            if (blog == null) throw new NotFoundException("No Story Found With such id!");
            if (blog.AuthorId.ToString() != _jwtHandler.GetClaimId()) throw new UnAuthorizedException("Not Authorized");
            await _blogRepository.DeleteBlogAsync(blog);
            return true;
            
        }

    }
}
