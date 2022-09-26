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
using NLog.LayoutRenderers;
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
        private readonly BaseValidator<BlogDetailsDto> _blogDetailsDtoValidator;
        private readonly BaseValidator<BlogPostDto> _blogPostDtoValidator;
        private readonly BaseValidator<BlogUpdateDto> _blogUpdateDtoValidator;

        public BlogService(
            IBlogRepository blogRepository,
            IMapper mapper,
            IJwtHandler jwtHandler,
            BaseValidator<BlogDetailsDto> blogDetailsDtoValidator,
            BaseValidator<BlogPostDto> blogPostDtoValidator,
            BaseValidator<BlogUpdateDto> blogUpdateDtoValidator
            )
        {
            _blogRepository = blogRepository;
            _mapper = mapper;
            _jwtHandler = jwtHandler;
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
            blog1.CreatedAt = DateTime.UtcNow;
            blog1.ModifiedAt = DateTime.UtcNow;
           
            Blog blog2 = await _blogRepository.CreateBlogAsync(blog1);
            BlogDetailsDto blogDetails = _mapper.Map<BlogDetailsDto>(blog2);

            _blogDetailsDtoValidator.ValidateDto(blogDetails);
            return blogDetails;
        }
        public async Task<List<BlogDetailsDto>> GetAllAsync()
        {
            List<Blog> blogs = await _blogRepository.GetAllAsync();
            if (blogs == null) throw new NotFoundException("No Story Found!");
            List<BlogDetailsDto> blogDetails = _mapper.Map<List<BlogDetailsDto>>(blogs);

            foreach (var blog in blogDetails) _blogDetailsDtoValidator.ValidateDto(blog);

            return blogDetails;
        }
        public async Task<BlogDetailsDto> GetBlogByIdAsync(int id)
        {
            Blog blog = await _blogRepository.GetBlogByIdAsync(id);
            if (blog == null) throw new NotFoundException("No Story Found!");

            BlogDetailsDto blogDetails = _mapper.Map<BlogDetailsDto>(blog);
            _blogDetailsDtoValidator.ValidateDto(blogDetails);

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
        public async Task<BlogDetailsDto> GetBlogByAuthorAsync(string author)
        {
            var blog = await _blogRepository.GetBlogByAuthorAsync(author);
            if (blog == null) throw new NotFoundException("No Story Found!");

            BlogDetailsDto blogDetails = _mapper.Map<BlogDetailsDto>(blog);
            _blogDetailsDtoValidator.ValidateDto(blogDetails);

            return blogDetails;
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
            _blogDetailsDtoValidator.ValidateDto(blogDetails);

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
