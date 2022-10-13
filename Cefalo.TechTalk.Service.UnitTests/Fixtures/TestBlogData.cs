using Cefalo.TechTalk.Database.Models;
using Cefalo.TechTalk.Service.DTOs;
using NPOI.SS.Formula.Functions;
using NPOI.XWPF.UserModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cefalo.TechTalk.Service.UnitTests.Fixtures
{
    public class TestBlogData
    {
        private readonly List<Blog> BlogList = new List<Blog>();
        
        private readonly List<BlogDetailsDto> BlogDetailsDtoList = new List<BlogDetailsDto>();

        private readonly DateTime _dateTime;

        public TestBlogData()
        {
            _dateTime = DateTime.UtcNow;
            for (int i = 0; i < 10; i++)
            {
                BlogList.Add(CreateBlogObject(i));
                BlogDetailsDtoList.Add(CreateBlogDetailsDtoObject(i)); 
            }
               
        }

        public virtual DateTime GetDateTime() { return _dateTime; }

        public virtual Blog GetBlog(int id)
        {
            return BlogList[id];
        }

        public virtual List<Blog> GetAllBlog()
        {
            return BlogList;
        }

        public virtual BlogDetailsDto GetBlogDetailsDto(int id)
        {
            return BlogDetailsDtoList[id];
        }

        public virtual List<BlogDetailsDto> GetAllBlogDetailsDto()
        {
            return BlogDetailsDtoList;
        }

        private Blog CreateBlogObject(int id)
        {
            Blog blog = new Blog()
            {


                Id = id,
                AuthorName = "New Author",
                AuthorId = 1,

                Title = "A New Title.",
                Body = "A New Description",

                CreatedAt = _dateTime,

                ModifiedAt = _dateTime,

                User = null
            };
            return blog;
        }
        private BlogDetailsDto CreateBlogDetailsDtoObject(int id)
        {
            BlogDetailsDto blogDetailsDto = new BlogDetailsDto()
            {

                Id = BlogList[id].Id,
                AuthorName = BlogList[id].AuthorName,

                Title = BlogList[id].Title,
                Body = BlogList[id].Body,

                CreatedAt = BlogList[id].CreatedAt,

                ModifiedAt = BlogList[id].ModifiedAt,

            };
            return blogDetailsDto;
        }
        public virtual BlogPostDto GetBlogPostDtoObject()
        {
            BlogPostDto blogPostDto = new BlogPostDto()
            {
                Title = BlogList[1].Title,
                Body = BlogList[1].Body,
            };
            return blogPostDto;
        }
        public virtual BlogUpdateDto GetBlogUpdateDtoObject()
        {
            BlogUpdateDto blogUpdateDto = new BlogUpdateDto()
            {
                Title = BlogList[1].Title,
                Body = BlogList[1].Body,
            };
            return blogUpdateDto;
        }
    }
}
