using AutoMapper;
using Cefalo.TechTalk.Database.Models;
using Cefalo.TechTalk.Repository.Contracts;
using Cefalo.TechTalk.Repository.Repositories;
using Cefalo.TechTalk.Service.DTOs;
using Cefalo.TechTalk.Service.Services;
using Cefalo.TechTalk.Service.UnitTests.Fixtures;
using Cefalo.TechTalk.Service.Utils.Contracts;
using Cefalo.TechTalk.Service.Utils.CustomErrorHandler;
using Cefalo.TechTalk.Service.Utils.DtoValidators;
using Cefalo.TechTalk.Service.Utils.Services;
using Cefalo.TechTalk.Service.Utils.Wrappers;
using FakeItEasy;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cefalo.TechTalk.Service.UnitTests.Systems
{
    public class TestBlogService
    {

        private static bool CheckObjectEquality<T1, T2>(T1 obj1, T2 obj2)
        {
            try
            {
                obj1.Should().BeOfType<T2>();
                obj1.Should().BeEquivalentTo(obj2);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        //public async Task<BlogDetailsDto> CreateBlogAsync(BlogPostDto blog)
        [Fact]
        public async void CreateBlogAsync_WithNoHttpContext_ReturnsUnAuthorizedException()
        {

            //Arrange
            var config = new MapperConfiguration(cfg =>
                {
                    cfg.CreateMap<Blog, BlogDetailsDto>();
                    cfg.CreateMap<BlogPostDto, Blog>();
                }
            );
            var _mapperStub = new Mapper(config);

            var _blogRepositoryStub = A.Fake<IBlogRepository>();
            var _jwtHandlerStub = A.Fake<IJwtHandler>();
            var _uriServiceStub = A.Fake<IUriService>();
            var _httpContextAccessorStub = A.Fake<IHttpContextAccessor>();
            var _dateTimeHandlerStub = A.Fake<IDateTimeHandler>();

            var _blogDetailsDtoValidatorStub = A.Fake<BaseValidator<BlogDetailsDto>>();
            var _blogPostDtoValidatorStub = A.Fake<BaseValidator<BlogPostDto>>();
            var _blogUpdateDtoValidatorStub = A.Fake<BaseValidator<BlogUpdateDto>>();

            var testBlogDataObject = new TestBlogData();
            var testBlogPostDto = testBlogDataObject.GetBlogPostDtoObject();

            A.CallTo(() => _jwtHandlerStub.HttpContextExist()).Returns(false);

            var blogService = new BlogService(_blogRepositoryStub, _mapperStub, _jwtHandlerStub, _uriServiceStub, _httpContextAccessorStub, _dateTimeHandlerStub, _blogDetailsDtoValidatorStub, _blogPostDtoValidatorStub, _blogUpdateDtoValidatorStub);

            var expectedException = new UnAuthorizedException("Not Authorized");

            //Act

            var actualException = await Record.ExceptionAsync(() => blogService.CreateBlogAsync(testBlogPostDto));


            //Assert

            Assert.NotNull(actualException);
            Assert.IsType<UnAuthorizedException>(actualException);
            Assert.Equal(expectedException.Message, actualException.Message);

            A.CallTo(() => _jwtHandlerStub.HttpContextExist()).MustHaveHappenedOnceExactly();


        }

        

        [Fact]
        public async void CreateBlogAsync_WithValidParameters_ReturnsBlogDetailsDto()
        {

            //Arrange
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Blog, BlogDetailsDto>();
                cfg.CreateMap<BlogPostDto, Blog>();
            }
            );

            var _mapperStub = new Mapper(config);

            var _blogRepositoryStub = A.Fake<IBlogRepository>();
            var _jwtHandlerStub = A.Fake<IJwtHandler>();
            var _uriServiceStub = A.Fake<IUriService>();
            var _httpContextAccessorStub = A.Fake<IHttpContextAccessor>();
            var _dateTimeHandlerStub = A.Fake<IDateTimeHandler>();  

            var _blogDetailsDtoValidatorStub = A.Fake<BaseValidator<BlogDetailsDto>>();
            var _blogPostDtoValidatorStub = A.Fake<BaseValidator<BlogPostDto>>();
            var _blogUpdateDtoValidatorStub = A.Fake<BaseValidator<BlogUpdateDto>>();

            var testBlogDataObject = new TestBlogData();
            var testBlogData = testBlogDataObject.GetBlog(0);
            var testBlogPostDto = testBlogDataObject.GetBlogPostDtoObject();
            var testBlogDetailsDto = testBlogDataObject.GetBlogDetailsDto(0);

            A.CallTo(() => _jwtHandlerStub.HttpContextExist()).Returns(true);
            A.CallTo(() => _jwtHandlerStub.GetClaimName()).Returns(testBlogDetailsDto.AuthorName);
            A.CallTo(() => _jwtHandlerStub.GetClaimId()).Returns((1).ToString());
            A.CallTo(() => _dateTimeHandlerStub.GetDateTimeInUtcNow()).Returns(testBlogDataObject.GetDateTime());

            A.CallTo(() => _blogRepositoryStub
                            .CreateBlogAsync(A<Blog>.That.Matches(blog=>CheckObjectEquality(blog,testBlogData)))).Returns(testBlogData);
            //A.CallTo(() => _blogRepositoryStub.CreateBlogAsync(testBlogData)).Returns(testBlogData);



            var blogService = new BlogService(_blogRepositoryStub, _mapperStub, _jwtHandlerStub, _uriServiceStub, _httpContextAccessorStub,_dateTimeHandlerStub, _blogDetailsDtoValidatorStub, _blogPostDtoValidatorStub, _blogUpdateDtoValidatorStub);

            var expectedBlog = testBlogDetailsDto;

            //Act

            var actualBlog = await blogService.CreateBlogAsync(testBlogPostDto);


            //Assert

            Assert.NotNull(actualBlog);
            Assert.IsType<BlogDetailsDto>(actualBlog);
            actualBlog.Should().BeEquivalentTo(expectedBlog);

            A.CallTo(() => _jwtHandlerStub.HttpContextExist()).MustHaveHappenedOnceExactly();
            A.CallTo(() => _jwtHandlerStub.GetClaimName()).MustHaveHappenedOnceExactly();
            A.CallTo(() => _jwtHandlerStub.GetClaimId()).MustHaveHappenedOnceExactly();
            A.CallTo(() => _dateTimeHandlerStub.GetDateTimeInUtcNow()).MustHaveHappenedTwiceExactly();
            A.CallTo(() => _blogRepositoryStub.CreateBlogAsync(A<Blog>.That.Matches(blog => CheckObjectEquality(blog, testBlogData)))).MustHaveHappenedOnceExactly();

        }



        //public async Task<PagedResponse<List<BlogDetailsDto>>> GetAllAsync(PaginationFilter filter)
        /*
                [Fact]
                public async void GetAllAsync_WithPaginationFilter_ReturnsPagedResponse()
                {
                    //Arrange
                    var config = new MapperConfiguration(cfg =>
                           cfg.CreateMap<Blog, BlogDetailsDto>()       
                    );
                    var _mapperStub = new Mapper(config);

                    var _blogRepositoryStub = A.Fake<IBlogRepository>();
                    var _jwtHandlerStub = A.Fake<IJwtHandler>();
                    var _uriServiceStub = A.Fake<IUriService>();
                    var _httpContextAccessorStub = A.Fake<IHttpContextAccessor>();

                    var _blogDetailsDtoValidatorStub = A.Fake<BaseValidator<BlogDetailsDto>>();
                    var _blogPostDtoValidatorStub = A.Fake<BaseValidator<BlogPostDto>>();
                    var _blogUpdateDtoValidatorStub = A.Fake<BaseValidator<BlogUpdateDto>>();

                    var route = "https://localhost:7057/api/Blog?pageNumber=1&pageSize=10";

                    var filter = new PaginationFilter();
                    var validFilter = new PaginationFilter(filter.PageNumber, filter.PageSize);

                    var testBlogDataObject = new TestBlogData();
                    var testBlogData = testBlogDataObject.GetAllBlog();
                    var testBlogDetailsDtoList = testBlogDataObject.GetAllBlogDetailsDto();

                    // var route = _httpContextAccessor.HttpContext.Request.Path.Value;
                    A.CallTo(() => _httpContextAccessorStub.HttpContext.Request.Path.Value).Returns("https://localhost:7057/api/Blog?pageNumber=1&pageSize=10");
                    A.CallTo(() => _blogRepositoryStub.GetAllAsync(filter.PageNumber, filter.PageSize)).Returns(testBlogData);
                    A.CallTo(() => _blogRepositoryStub.CountAsync()).Returns(10);

                    var expectedResponse = PaginationHandler.CreatePagedReponse(testBlogDetailsDtoList, validFilter, 10, _uriServiceStub, route);

                    var blogService = new BlogService(_blogRepositoryStub, _mapperStub, _jwtHandlerStub, _uriServiceStub, _httpContextAccessorStub, _blogDetailsDtoValidatorStub, _blogPostDtoValidatorStub, _blogUpdateDtoValidatorStub);

                    //Act

                    var actualResponse = await blogService.GetAllAsync(filter);
                    //Assert

                    Assert.NotNull(actualResponse);
                    actualResponse.Should().BeEquivalentTo(expectedResponse);

                    A.CallTo(() => _httpContextAccessorStub.HttpContext.Request.Path.Value).MustHaveHappenedOnceExactly();
                    A.CallTo(() => _blogRepositoryStub.GetAllAsync(filter.PageNumber, filter.PageSize)).MustHaveHappenedOnceExactly();
                    A.CallTo(() => _blogRepositoryStub.CountAsync()).MustHaveHappenedOnceExactly();

                }
         */
        //public async Task<BlogDetailsDto> GetBlogByIdAsync(int id)
        [Fact]
        public async void GetBlogByIdAsync_WithUnExistingBlog_ReturnNotFoundException()
        {
            //Arrange
            var config = new MapperConfiguration(cfg =>
                cfg.CreateMap<Blog, BlogDetailsDto>()
            );
            var _mapperStub = new Mapper(config);

            var _blogRepositoryStub = A.Fake<IBlogRepository>();
            var _jwtHandlerStub = A.Fake<IJwtHandler>();
            var _uriServiceStub = A.Fake<IUriService>();
            var _httpContextAccessorStub = A.Fake<IHttpContextAccessor>();
            var _dateTimeHandlerStub = A.Fake<IDateTimeHandler>();

            var _blogDetailsDtoValidatorStub = A.Fake<BaseValidator<BlogDetailsDto>>();
            var _blogPostDtoValidatorStub = A.Fake<BaseValidator<BlogPostDto>>();
            var _blogUpdateDtoValidatorStub = A.Fake<BaseValidator<BlogUpdateDto>>();

            A.CallTo(() => _blogRepositoryStub.GetBlogByIdAsync(1)).Returns((Blog)null);

            var blogService = new BlogService(_blogRepositoryStub, _mapperStub, _jwtHandlerStub, _uriServiceStub, _httpContextAccessorStub, _dateTimeHandlerStub, _blogDetailsDtoValidatorStub, _blogPostDtoValidatorStub, _blogUpdateDtoValidatorStub);

            var expectedException = new NotFoundException("No Story Found With This Id.");

            //Act

            var actualException = await Record.ExceptionAsync(() => blogService.GetBlogByIdAsync(1));


            //Assert

            Assert.NotNull(actualException);
            Assert.IsType<NotFoundException>(actualException);
            Assert.Equal(expectedException.Message, actualException.Message);

            A.CallTo(() => _blogRepositoryStub.GetBlogByIdAsync(1)).MustHaveHappenedOnceExactly();

        }

        [Fact]
        public async void GetBlogByIdAsync_WithExistingBlog_ReturnBlogDetailsDtoObject()
        {
            //Arrange
            var config = new MapperConfiguration(cfg =>
                cfg.CreateMap<Blog, BlogDetailsDto>()
            );
            var _mapperStub = new Mapper(config);

            var _blogRepositoryStub = A.Fake<IBlogRepository>();
            var _jwtHandlerStub = A.Fake<IJwtHandler>();
            var _uriServiceStub = A.Fake<IUriService>();
            var _httpContextAccessorStub = A.Fake<IHttpContextAccessor>();
            var _dateTimeHandlerStub = A.Fake<IDateTimeHandler>();

            var _blogDetailsDtoValidatorStub = A.Fake<BaseValidator<BlogDetailsDto>>();
            var _blogPostDtoValidatorStub = A.Fake<BaseValidator<BlogPostDto>>();
            var _blogUpdateDtoValidatorStub = A.Fake<BaseValidator<BlogUpdateDto>>();

            var testBlogDataObject = new TestBlogData();

            A.CallTo(() => _blogRepositoryStub.GetBlogByIdAsync(1)).Returns(testBlogDataObject.GetBlog(1));

            var blogService = new BlogService(_blogRepositoryStub, _mapperStub, _jwtHandlerStub, _uriServiceStub, _httpContextAccessorStub, _dateTimeHandlerStub, _blogDetailsDtoValidatorStub, _blogPostDtoValidatorStub, _blogUpdateDtoValidatorStub);

            var expectedUser = testBlogDataObject.GetBlogDetailsDto(1);

            //Act

            var actualUser = await blogService.GetBlogByIdAsync(1);


            //Assert

            Assert.NotNull(actualUser);
            Assert.IsType<BlogDetailsDto>(actualUser);
            actualUser.Should().BeEquivalentTo(expectedUser);

            A.CallTo(() => _blogRepositoryStub.GetBlogByIdAsync(1)).MustHaveHappenedOnceExactly();

        }

        //public async Task<BlogDetailsDto> GetBlogByTitleAsync(string title)

        [Fact]
        public async void GetBlogByTitleAsync_WithUnExistingBlog_ReturnNotFoundException()
        {
            //Arrange
            var config = new MapperConfiguration(cfg =>
                cfg.CreateMap<Blog, BlogDetailsDto>()
            );
            var _mapperStub = new Mapper(config);

            var _blogRepositoryStub = A.Fake<IBlogRepository>();
            var _jwtHandlerStub = A.Fake<IJwtHandler>();
            var _uriServiceStub = A.Fake<IUriService>();
            var _httpContextAccessorStub = A.Fake<IHttpContextAccessor>();
            var _dateTimeHandlerStub = A.Fake<IDateTimeHandler>();

            var _blogDetailsDtoValidatorStub = A.Fake<BaseValidator<BlogDetailsDto>>();
            var _blogPostDtoValidatorStub = A.Fake<BaseValidator<BlogPostDto>>();
            var _blogUpdateDtoValidatorStub = A.Fake<BaseValidator<BlogUpdateDto>>();

            A.CallTo(() => _blogRepositoryStub.GetBlogByTitleAsync("Title")).Returns((Blog)null);

            var blogService = new BlogService(_blogRepositoryStub, _mapperStub, _jwtHandlerStub, _uriServiceStub, _httpContextAccessorStub, _dateTimeHandlerStub, _blogDetailsDtoValidatorStub, _blogPostDtoValidatorStub, _blogUpdateDtoValidatorStub);

            var expectedException = new NotFoundException("No Story Found With This Title.");

            //Act

            var actualException = await Record.ExceptionAsync(() => blogService.GetBlogByTitleAsync("Title"));


            //Assert

            Assert.NotNull(actualException);
            Assert.IsType<NotFoundException>(actualException);
            Assert.Equal(expectedException.Message, actualException.Message);

            A.CallTo(() => _blogRepositoryStub.GetBlogByTitleAsync("Title")).MustHaveHappenedOnceExactly();

        }

        [Fact]
        public async void GetBlogByTitleAsync_WithExistingBlog_ReturnBlogDetailsDtoObject()
        {
            //Arrange
            var config = new MapperConfiguration(cfg =>
                cfg.CreateMap<Blog, BlogDetailsDto>()
            );
            var _mapperStub = new Mapper(config);

            var _blogRepositoryStub = A.Fake<IBlogRepository>();
            var _jwtHandlerStub = A.Fake<IJwtHandler>();
            var _uriServiceStub = A.Fake<IUriService>();
            var _httpContextAccessorStub = A.Fake<IHttpContextAccessor>();
            var _dateTimeHandlerStub = A.Fake<IDateTimeHandler>();

            var _blogDetailsDtoValidatorStub = A.Fake<BaseValidator<BlogDetailsDto>>();
            var _blogPostDtoValidatorStub = A.Fake<BaseValidator<BlogPostDto>>();
            var _blogUpdateDtoValidatorStub = A.Fake<BaseValidator<BlogUpdateDto>>();

            var testBlogDataObject = new TestBlogData();
            var testBlogData = testBlogDataObject.GetBlog(1);

            A.CallTo(() => _blogRepositoryStub.GetBlogByTitleAsync(testBlogData.Title)).Returns(testBlogData);

            var blogService = new BlogService(_blogRepositoryStub, _mapperStub, _jwtHandlerStub, _uriServiceStub, _httpContextAccessorStub, _dateTimeHandlerStub, _blogDetailsDtoValidatorStub, _blogPostDtoValidatorStub, _blogUpdateDtoValidatorStub);

            var expectedUser = testBlogDataObject.GetBlogDetailsDto(1);

            //Act

            var actualUser = await blogService.GetBlogByTitleAsync(testBlogData.Title);


            //Assert

            Assert.NotNull(actualUser);
            Assert.IsType<BlogDetailsDto>(actualUser);
            actualUser.Should().BeEquivalentTo(expectedUser);

            A.CallTo(() => _blogRepositoryStub.GetBlogByTitleAsync(testBlogData.Title)).MustHaveHappenedOnceExactly();

        }

        //public async Task<PagedResponse<List<BlogDetailsDto>>> GetBlogsOfAuthorAsync(string username, PaginationFilter filter)

        //public async Task<BlogDetailsDto> UpdateBlogByIdAsync(BlogUpdateDto blog, int id)

        [Fact]
        public async void UpdateBlogByIdAsync_WithNoHttpContext_ReturnsUnAuthorizedException()
        {

            //Arrange
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Blog, BlogDetailsDto>();
                cfg.CreateMap<BlogUpdateDto, Blog>();
            }
           );
            var _mapperStub = new Mapper(config);

            var _blogRepositoryStub = A.Fake<IBlogRepository>();
            var _jwtHandlerStub = A.Fake<IJwtHandler>();
            var _uriServiceStub = A.Fake<IUriService>();
            var _httpContextAccessorStub = A.Fake<IHttpContextAccessor>();
            var _dateTimeHandlerStub = A.Fake<IDateTimeHandler>();

            var _blogDetailsDtoValidatorStub = A.Fake<BaseValidator<BlogDetailsDto>>();
            var _blogPostDtoValidatorStub = A.Fake<BaseValidator<BlogPostDto>>();
            var _blogUpdateDtoValidatorStub = A.Fake<BaseValidator<BlogUpdateDto>>();

            var testBlogDataObject = new TestBlogData();
            var testBlogUpdateDto = testBlogDataObject.GetBlogUpdateDtoObject();

            A.CallTo(() => _jwtHandlerStub.HttpContextExist()).Returns(false);

            var blogService = new BlogService(_blogRepositoryStub, _mapperStub, _jwtHandlerStub, _uriServiceStub, _httpContextAccessorStub, _dateTimeHandlerStub, _blogDetailsDtoValidatorStub, _blogPostDtoValidatorStub, _blogUpdateDtoValidatorStub);

            var expectedException = new UnAuthorizedException("Not Authorized");

            //Act

            var actualException = await Record.ExceptionAsync(() => blogService.UpdateBlogByIdAsync(testBlogUpdateDto,1));


            //Assert

            Assert.NotNull(actualException);
            Assert.IsType<UnAuthorizedException>(actualException);
            Assert.Equal(expectedException.Message, actualException.Message);

            A.CallTo(() => _jwtHandlerStub.HttpContextExist()).MustHaveHappenedOnceExactly();

        }

        [Fact]
        public async void UpdateBlogByIdAsync_WithInvalidBlogId_ReturnsNotFoundException()
        {

            //Arrange
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Blog, BlogDetailsDto>();
                cfg.CreateMap<BlogUpdateDto, Blog>();
            }
           );
            var _mapperStub = new Mapper(config);

            var _blogRepositoryStub = A.Fake<IBlogRepository>();
            var _jwtHandlerStub = A.Fake<IJwtHandler>();
            var _uriServiceStub = A.Fake<IUriService>();
            var _httpContextAccessorStub = A.Fake<IHttpContextAccessor>();
            var _dateTimeHandlerStub = A.Fake<IDateTimeHandler>();

            var _blogDetailsDtoValidatorStub = A.Fake<BaseValidator<BlogDetailsDto>>();
            var _blogPostDtoValidatorStub = A.Fake<BaseValidator<BlogPostDto>>();
            var _blogUpdateDtoValidatorStub = A.Fake<BaseValidator<BlogUpdateDto>>();

            var testBlogDataObject = new TestBlogData();
            var testBlogUpdateDto = testBlogDataObject.GetBlogUpdateDtoObject();

            A.CallTo(() => _jwtHandlerStub.HttpContextExist()).Returns(true);
            A.CallTo(() => _blogRepositoryStub.GetBlogByIdAsync(1)).Returns((Blog)null);

            var blogService = new BlogService(_blogRepositoryStub, _mapperStub, _jwtHandlerStub, _uriServiceStub, _httpContextAccessorStub, _dateTimeHandlerStub, _blogDetailsDtoValidatorStub, _blogPostDtoValidatorStub, _blogUpdateDtoValidatorStub);

            var expectedException = new NotFoundException("No Story Found With this id!");

            //Act

            var actualException = await Record.ExceptionAsync(() => blogService.UpdateBlogByIdAsync(testBlogUpdateDto, 1));


            //Assert

            Assert.NotNull(actualException);
            Assert.IsType<NotFoundException>(actualException);
            Assert.Equal(expectedException.Message, actualException.Message);

            A.CallTo(() => _jwtHandlerStub.HttpContextExist()).MustHaveHappenedOnceExactly();
            A.CallTo(() => _blogRepositoryStub.GetBlogByIdAsync(1)).MustHaveHappenedOnceExactly();
        }

        [Fact]
        public async void UpdateBlogByIdAsync_WithUnAuthorizedUser_ReturnsUnAuthorizedException()
        {

            //Arrange
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Blog, BlogDetailsDto>();
                cfg.CreateMap<BlogUpdateDto, Blog>();
            }
           );
            var _mapperStub = new Mapper(config);

            var _blogRepositoryStub = A.Fake<IBlogRepository>();
            var _jwtHandlerStub = A.Fake<IJwtHandler>();
            var _uriServiceStub = A.Fake<IUriService>();
            var _httpContextAccessorStub = A.Fake<IHttpContextAccessor>();
            var _dateTimeHandlerStub = A.Fake<IDateTimeHandler>();

            var _blogDetailsDtoValidatorStub = A.Fake<BaseValidator<BlogDetailsDto>>();
            var _blogPostDtoValidatorStub = A.Fake<BaseValidator<BlogPostDto>>();
            var _blogUpdateDtoValidatorStub = A.Fake<BaseValidator<BlogUpdateDto>>();

            var testBlogDataObject = new TestBlogData();
            var testBlogUpdateDto = testBlogDataObject.GetBlogUpdateDtoObject();
            var testBlogData = testBlogDataObject.GetBlog(1);

            A.CallTo(() => _jwtHandlerStub.HttpContextExist()).Returns(true);
            A.CallTo(() => _blogRepositoryStub.GetBlogByIdAsync(1)).Returns(testBlogData);
            A.CallTo(() => _jwtHandlerStub.GetClaimId()).Returns( (10).ToString() );
            var blogService = new BlogService(_blogRepositoryStub, _mapperStub, _jwtHandlerStub, _uriServiceStub, _httpContextAccessorStub, _dateTimeHandlerStub, _blogDetailsDtoValidatorStub, _blogPostDtoValidatorStub, _blogUpdateDtoValidatorStub);


            var expectedException = new UnAuthorizedException("Not Authorized");

            //Act

            var actualException = await Record.ExceptionAsync(() => blogService.UpdateBlogByIdAsync(testBlogUpdateDto, 1));


            //Assert

            Assert.NotNull(actualException);
            Assert.IsType<UnAuthorizedException>(actualException);
            Assert.Equal(expectedException.Message, actualException.Message);

            A.CallTo(() => _jwtHandlerStub.HttpContextExist()).MustHaveHappenedOnceExactly();
            A.CallTo(() => _blogRepositoryStub.GetBlogByIdAsync(1)).MustHaveHappenedOnceExactly();
            A.CallTo(() => _jwtHandlerStub.GetClaimId()).MustHaveHappenedOnceExactly();
        }

        [Fact]
        public async void UpdateBlogByIdAsync_WithValidParameters_ReturnsBlogDetailsDto()
        {

            //Arrange
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Blog, BlogDetailsDto>();
                cfg.CreateMap<BlogUpdateDto, Blog>();
            }
           );
            var _mapperStub = new Mapper(config);

            var _blogRepositoryStub = A.Fake<IBlogRepository>();
            var _jwtHandlerStub = A.Fake<IJwtHandler>();
            var _uriServiceStub = A.Fake<IUriService>();
            var _httpContextAccessorStub = A.Fake<IHttpContextAccessor>();
            var _dateTimeHandlerStub = A.Fake<IDateTimeHandler>();

            var _blogDetailsDtoValidatorStub = A.Fake<BaseValidator<BlogDetailsDto>>();
            var _blogPostDtoValidatorStub = A.Fake<BaseValidator<BlogPostDto>>();
            var _blogUpdateDtoValidatorStub = A.Fake<BaseValidator<BlogUpdateDto>>();

            var testBlogDataObject = new TestBlogData();
            var testBlogUpdateDto = testBlogDataObject.GetBlogUpdateDtoObject();
            var testBlogData = testBlogDataObject.GetBlog(1);
            var testBlogDataCallable = _mapperStub.Map<Blog>(testBlogUpdateDto);

            A.CallTo(() => _jwtHandlerStub.HttpContextExist()).Returns(true);
            A.CallTo(() => _blogRepositoryStub.GetBlogByIdAsync(1)).Returns(testBlogData);
            A.CallTo(() => _jwtHandlerStub.GetClaimId()).Returns((testBlogData.AuthorId).ToString());

            A.CallTo(() => _blogRepositoryStub
                                .UpdateBlogByIdAsync(A<Blog>.That.Matches( blog => CheckObjectEquality(blog, testBlogDataCallable)),1))
                                .Returns(testBlogData);

            var blogService = new BlogService(_blogRepositoryStub, _mapperStub, _jwtHandlerStub, _uriServiceStub, _httpContextAccessorStub, _dateTimeHandlerStub, _blogDetailsDtoValidatorStub, _blogPostDtoValidatorStub, _blogUpdateDtoValidatorStub);


            var expectedBlog = testBlogDataObject.GetBlogDetailsDto(1);

            //Act

            var actualBlog = await  blogService.UpdateBlogByIdAsync(testBlogUpdateDto, 1);


            //Assert

            Assert.NotNull(actualBlog);
            Assert.IsType<BlogDetailsDto>(actualBlog);
            actualBlog.Should().BeEquivalentTo(expectedBlog);

            A.CallTo(() => _jwtHandlerStub.HttpContextExist()).MustHaveHappenedOnceExactly();
            A.CallTo(() => _blogRepositoryStub.GetBlogByIdAsync(1)).MustHaveHappenedOnceExactly();
            A.CallTo(() => _jwtHandlerStub.GetClaimId()).MustHaveHappenedOnceExactly();
            A.CallTo(() => _blogRepositoryStub.UpdateBlogByIdAsync(A<Blog>.That.Matches(blog => CheckObjectEquality(blog, testBlogDataCallable)), 1)).MustHaveHappenedOnceExactly();
        }

        //public async Task<Boolean> DeleteBlogByIdAsync(int id)
        [Fact]
        public async void DeleteBlogByIdAsync_WithNoHttpContext_ReturnUnAuthorizedException()
        {
            //Arrange
            var config = new MapperConfiguration(cfg =>
                cfg.CreateMap<Blog, BlogDetailsDto>()
            );
            var _mapperStub = new Mapper(config);

            var _blogRepositoryStub = A.Fake<IBlogRepository>();
            var _jwtHandlerStub = A.Fake<IJwtHandler>();
            var _uriServiceStub = A.Fake<IUriService>();
            var _httpContextAccessorStub = A.Fake<IHttpContextAccessor>();
            var _dateTimeHandlerStub = A.Fake<IDateTimeHandler>();

            var _blogDetailsDtoValidatorStub = A.Fake<BaseValidator<BlogDetailsDto>>();
            var _blogPostDtoValidatorStub = A.Fake<BaseValidator<BlogPostDto>>();
            var _blogUpdateDtoValidatorStub = A.Fake<BaseValidator<BlogUpdateDto>>();


            A.CallTo(() => _jwtHandlerStub.HttpContextExist()).Returns(false);

            var blogService = new BlogService(_blogRepositoryStub, _mapperStub, _jwtHandlerStub, _uriServiceStub, _httpContextAccessorStub, _dateTimeHandlerStub, _blogDetailsDtoValidatorStub, _blogPostDtoValidatorStub, _blogUpdateDtoValidatorStub);

            var expectedException = new UnAuthorizedException("Not Authorized");

            //Act

            var actualException = await Record.ExceptionAsync(() => blogService.DeleteBlogByIdAsync(1));


            //Assert

            Assert.NotNull(actualException);
            Assert.IsType<UnAuthorizedException>(actualException);
            Assert.Equal(expectedException.Message, actualException.Message);

            A.CallTo(() => _jwtHandlerStub.HttpContextExist()).MustHaveHappenedOnceExactly();

        }

        [Fact]
        public async void DeleteBlogByIdAsync_WithUnExistingBlog_ReturnNotFoundException()
        {
            //Arrange
            var config = new MapperConfiguration(cfg =>
                cfg.CreateMap<Blog, BlogDetailsDto>()
            );
            var _mapperStub = new Mapper(config);

            var _blogRepositoryStub = A.Fake<IBlogRepository>();
            var _jwtHandlerStub = A.Fake<IJwtHandler>();
            var _uriServiceStub = A.Fake<IUriService>();
            var _httpContextAccessorStub = A.Fake<IHttpContextAccessor>();
            var _dateTimeHandlerStub = A.Fake<IDateTimeHandler>();

            var _blogDetailsDtoValidatorStub = A.Fake<BaseValidator<BlogDetailsDto>>();
            var _blogPostDtoValidatorStub = A.Fake<BaseValidator<BlogPostDto>>();
            var _blogUpdateDtoValidatorStub = A.Fake<BaseValidator<BlogUpdateDto>>();


            A.CallTo(() => _jwtHandlerStub.HttpContextExist()).Returns(true);
            A.CallTo(() => _blogRepositoryStub.GetBlogByIdAsync(1)).Returns((Blog)null);


            var blogService = new BlogService(_blogRepositoryStub, _mapperStub, _jwtHandlerStub, _uriServiceStub, _httpContextAccessorStub, _dateTimeHandlerStub, _blogDetailsDtoValidatorStub, _blogPostDtoValidatorStub, _blogUpdateDtoValidatorStub);

            var expectedException = new NotFoundException("No Story Found With such id!");

            //Act

            var actualException = await Record.ExceptionAsync(() => blogService.DeleteBlogByIdAsync(1));


            //Assert

            Assert.NotNull(actualException);
            Assert.IsType<NotFoundException>(actualException);
            Assert.Equal(expectedException.Message, actualException.Message);

            A.CallTo(() => _jwtHandlerStub.HttpContextExist()).MustHaveHappenedOnceExactly();
            A.CallTo(() => _blogRepositoryStub.GetBlogByIdAsync(1)).MustHaveHappenedOnceExactly();

        }

        [Fact]
        public async void DeleteBlogByIdAsync_WithUnAuthorizedUser_ReturnUnAuthorizedException()
        {
            //Arrange
            var config = new MapperConfiguration(cfg =>
                cfg.CreateMap<Blog, BlogDetailsDto>()
            );
            var _mapperStub = new Mapper(config);

            var _blogRepositoryStub = A.Fake<IBlogRepository>();
            var _jwtHandlerStub = A.Fake<IJwtHandler>();
            var _uriServiceStub = A.Fake<IUriService>();
            var _httpContextAccessorStub = A.Fake<IHttpContextAccessor>();
            var _dateTimeHandlerStub = A.Fake<IDateTimeHandler>();

            var _blogDetailsDtoValidatorStub = A.Fake<BaseValidator<BlogDetailsDto>>();
            var _blogPostDtoValidatorStub = A.Fake<BaseValidator<BlogPostDto>>();
            var _blogUpdateDtoValidatorStub = A.Fake<BaseValidator<BlogUpdateDto>>();

            var testBlogDataObject = new TestBlogData();
            var testBlogData = testBlogDataObject.GetBlog(1);

            A.CallTo(() => _jwtHandlerStub.HttpContextExist()).Returns(true);
            A.CallTo(() => _blogRepositoryStub.GetBlogByIdAsync(1)).Returns(testBlogData);
            A.CallTo(() => _jwtHandlerStub.GetClaimId()).Returns( (10).ToString() );


            var blogService = new BlogService(_blogRepositoryStub, _mapperStub, _jwtHandlerStub, _uriServiceStub, _httpContextAccessorStub, _dateTimeHandlerStub, _blogDetailsDtoValidatorStub, _blogPostDtoValidatorStub, _blogUpdateDtoValidatorStub);

            var expectedException = new UnAuthorizedException("Not Authorized");

            //Act

            var actualException = await Record.ExceptionAsync(() => blogService.DeleteBlogByIdAsync(1));


            //Assert

            Assert.NotNull(actualException);
            Assert.IsType<UnAuthorizedException>(actualException);
            Assert.Equal(expectedException.Message, actualException.Message);

            A.CallTo(() => _jwtHandlerStub.HttpContextExist()).MustHaveHappenedOnceExactly();
            A.CallTo(() => _blogRepositoryStub.GetBlogByIdAsync(1)).MustHaveHappenedOnceExactly();
            A.CallTo(() => _jwtHandlerStub.GetClaimId()).MustHaveHappenedOnceExactly();

        }

        [Fact]
        public async void DeleteBlogByIdAsync_WithValidParameters_ReturnTrue()
        {
            //Arrange
            var config = new MapperConfiguration(cfg =>
                cfg.CreateMap<Blog, BlogDetailsDto>()
            );
            var _mapperStub = new Mapper(config);

            var _blogRepositoryStub = A.Fake<IBlogRepository>();
            var _jwtHandlerStub = A.Fake<IJwtHandler>();
            var _uriServiceStub = A.Fake<IUriService>();
            var _httpContextAccessorStub = A.Fake<IHttpContextAccessor>();
            var _dateTimeHandlerStub = A.Fake<IDateTimeHandler>();

            var _blogDetailsDtoValidatorStub = A.Fake<BaseValidator<BlogDetailsDto>>();
            var _blogPostDtoValidatorStub = A.Fake<BaseValidator<BlogPostDto>>();
            var _blogUpdateDtoValidatorStub = A.Fake<BaseValidator<BlogUpdateDto>>();

            var testBlogDataObject = new TestBlogData();
            var testBlogData = testBlogDataObject.GetBlog(1);

            A.CallTo(() => _jwtHandlerStub.HttpContextExist()).Returns(true);
            A.CallTo(() => _blogRepositoryStub.GetBlogByIdAsync(1)).Returns(testBlogData);
            A.CallTo(() => _jwtHandlerStub.GetClaimId()).Returns((1).ToString());
            A.CallTo(() => _blogRepositoryStub.DeleteBlogAsync(testBlogData)).Returns(true);

            var blogService = new BlogService(_blogRepositoryStub, _mapperStub, _jwtHandlerStub, _uriServiceStub, _httpContextAccessorStub, _dateTimeHandlerStub, _blogDetailsDtoValidatorStub, _blogPostDtoValidatorStub, _blogUpdateDtoValidatorStub);


            //Act

            var actualResult = await blogService.DeleteBlogByIdAsync(1);


            //Assert

            
            Assert.IsType<Boolean>(actualResult);
            actualResult.Should().BeTrue();

            A.CallTo(() => _jwtHandlerStub.HttpContextExist()).MustHaveHappenedOnceExactly();
            A.CallTo(() => _blogRepositoryStub.GetBlogByIdAsync(1)).MustHaveHappenedOnceExactly();
            A.CallTo(() => _jwtHandlerStub.GetClaimId()).MustHaveHappenedOnceExactly();
            A.CallTo(() => _blogRepositoryStub.DeleteBlogAsync(testBlogData)).MustHaveHappenedOnceExactly();

        }




    }
}
