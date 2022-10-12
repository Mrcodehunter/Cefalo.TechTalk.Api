﻿using AutoMapper;
using Cefalo.TechTalk.Database.Models;
using Cefalo.TechTalk.Repository.Contracts;
using Cefalo.TechTalk.Service.DTOs;
using Cefalo.TechTalk.Service.Services;
using Cefalo.TechTalk.Service.UnitTests.Fixtures;
using Cefalo.TechTalk.Service.Utils.Contracts;
using Cefalo.TechTalk.Service.Utils.CustomErrorHandler;
using Cefalo.TechTalk.Service.Utils.DtoValidators;
using Cefalo.TechTalk.Service.Utils.Models;
using FakeItEasy;
using FluentAssertions;
using NPOI.SS.Formula.Functions;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cefalo.TechTalk.Service.UnitTests.Systems
{
    public class TestUserService
    {

        /* Exception? ex = null;
           try
           {
               _ = _userService.GetUserByIdAsync(1);
           }
           catch(Exception e)
           {
               ex = e;
           }*/

        //public async Task<List<UserDetailsDto>> GetAllAsync();
        
        [Fact]
        public async void GetAllAsync_ForEmptyUserList_ReturnsEmptyUserList()
        {
            // Arrange
            var config = new MapperConfiguration(cfg =>
                cfg.CreateMap<User, UserDetailsDto>()
            );
            var _mapperStub = new Mapper(config);

            var _userRepositoryStub = A.Fake<IUserRepository>();
            var _passwordHandlerStub = A.Fake<IPasswordHandler>();
            var _jwtHandlerStub = A.Fake<IJwtHandler>();
            var _userUpdateDtoValidatorStub = A.Fake<BaseValidator<UserUpdateDto>>();



            var expectedUsers = new List<UserDetailsDto>();

            A.CallTo(() => _userRepositoryStub.GetAllAsync()).Returns(new List<User>());

            var _userService = new UserService(_userRepositoryStub, _mapperStub, _passwordHandlerStub, _jwtHandlerStub, _userUpdateDtoValidatorStub);



            // Act
            var actualUsers = await _userService.GetAllAsync();

            //Assert
            Assert.Empty(actualUsers);
            A.CallTo(() => _userRepositoryStub.GetAllAsync()).MustHaveHappenedOnceExactly();
            actualUsers.Should().BeEquivalentTo(expectedUsers);

        }

        [Fact]
        public async void GetAllAsync_ForUserList_ReturnsUserDetailsDtoList()
        {
            // Arrange
            var config = new MapperConfiguration(cfg =>
                cfg.CreateMap<User, UserDetailsDto>()
            );
            var _mapperStub = new Mapper(config);

            var _userRepositoryStub = A.Fake<IUserRepository>();
            var _passwordHandlerStub = A.Fake<IPasswordHandler>();
            var _jwtHandlerStub = A.Fake<IJwtHandler>();
            var _userUpdateDtoValidatorStub = A.Fake<BaseValidator<UserUpdateDto>>();

            var testUserData = (new TestUserData()).GetAllUsers();
            
            var expectedUsers = _mapperStub.Map<List<UserDetailsDto>>(testUserData);

            A.CallTo(() => _userRepositoryStub.GetAllAsync()).Returns(testUserData);

            var _userService = new UserService(_userRepositoryStub, _mapperStub, _passwordHandlerStub, _jwtHandlerStub, _userUpdateDtoValidatorStub);

            

            // Act
            var actualUsers = await _userService.GetAllAsync();

            //Assert
            Assert.NotNull(actualUsers);
            A.CallTo(() => _userRepositoryStub.GetAllAsync()).MustHaveHappenedOnceExactly();
            actualUsers.Should().BeEquivalentTo(expectedUsers);

        }

        //public async Task<UserDetailsDto> GetUserByIdAsync(int id)
        [Fact]
        public async void GetUserByIdAsync_WithUnexistingUser_ReturnsNotFoundException()
        {
            // Arrange
            var config = new MapperConfiguration(cfg =>
                cfg.CreateMap<User, UserDetailsDto>()
            );
            var _mapperStub = new Mapper(config);

            var _userRepositoryStub = A.Fake< IUserRepository > ();
            var _passwordHandlerStub = A.Fake< IPasswordHandler > ();
            var _jwtHandlerStub = A.Fake< IJwtHandler > ();
            var _userUpdateDtoValidatorStub = A.Fake< BaseValidator < UserUpdateDto >> ();

            A.CallTo(() => _userRepositoryStub.GetUserByIdAsync(1)).Returns((User)null);

            var _userService = new UserService(_userRepositoryStub, _mapperStub, _passwordHandlerStub, _jwtHandlerStub, _userUpdateDtoValidatorStub);
            var expectedException = new NotFoundException("No user Exists With This Id.");

            // Act
            var actualException = await Record.ExceptionAsync(() => _userService.GetUserByIdAsync(1));
          
           
            // Assert
            Assert.NotNull(actualException);
            Assert.IsType<NotFoundException>(actualException);
            Assert.Equal(expectedException.Message,actualException.Message);

        }
        
        [Fact]
        public async void GetUserByIdAsync_WithExistingUser_ReturnsUserDetailsDto()
        {
            // Arrange
            var config = new MapperConfiguration(cfg =>
                cfg.CreateMap<User, UserDetailsDto>()
            );
            var _mapperStub = new Mapper(config);

            var _userRepositoryStub = A.Fake<IUserRepository>();
            var _passwordHandlerStub = A.Fake<IPasswordHandler>();
            var _jwtHandlerStub = A.Fake<IJwtHandler>();
            var _userUpdateDtoValidatorStub = A.Fake<BaseValidator<UserUpdateDto>>();
            

            var testUserData = new TestUserData();

            A.CallTo(() => _userRepositoryStub.GetUserByIdAsync(0)).Returns(testUserData.GetUser(0));

            var _userService = new UserService(_userRepositoryStub, _mapperStub, _passwordHandlerStub, _jwtHandlerStub, _userUpdateDtoValidatorStub);

            var expectedUser = _mapperStub.Map<UserDetailsDto>(testUserData.GetUser(0));

            // Act
            var actualUser = await _userService.GetUserByIdAsync(0);
           
            //Assert
            Assert.NotNull(actualUser);
            Assert.IsType<UserDetailsDto>(actualUser);
            Assert.Equal(expectedUser.Name, actualUser.Name);
            Assert.Equal(expectedUser.UserName, actualUser.UserName);
            Assert.Equal(expectedUser.Id, actualUser.Id);
            Assert.Equal(expectedUser.UserName, actualUser.UserName);

        }


        //public async Task<UserDetailsDto> UpdateUserByIdAsync(UserUpdateDto user, int id);
        
        [Fact]
        public async void UpdateUSerByIdAsync_WithNoHttpContext_ReturnsUnAuthorizedException()
        {
            //Arrange
            var config = new MapperConfiguration(cfg =>
                cfg.CreateMap<User, UserDetailsDto>()  
            );
            var _mapperStub = new Mapper(config);

            var _userRepositoryStub = A.Fake<IUserRepository>();
            var _passwordHandlerStub = A.Fake<IPasswordHandler>();
            var _jwtHandlerStub = A.Fake<IJwtHandler>();
            var _userUpdateDtoValidatorStub = A.Fake<BaseValidator<UserUpdateDto>>();

            var testUserData = new TestUserData();
            var userUpdateDto = testUserData.GetUserUpdateDto();

            var expectedException = new UnAuthorizedException("Unauthorized");
            A.CallTo(() => _jwtHandlerStub.HttpContextExist()).Returns(false);

            var _userService = new UserService(_userRepositoryStub, _mapperStub, _passwordHandlerStub, _jwtHandlerStub, _userUpdateDtoValidatorStub);
            
            //Act
            var actualException = await Record.ExceptionAsync(() => _userService.UpdateUserByIdAsync(userUpdateDto,1));
            //Assert
            Assert.NotNull(actualException);
            Assert.IsType<UnAuthorizedException>(actualException);
            Assert.Equal(expectedException.Message, actualException.Message);

            A.CallTo(()=> _jwtHandlerStub.HttpContextExist()).MustHaveHappenedOnceExactly();


        }
        
        [Fact]
        public async void UpdateUSerByIdAsync_WithInvalidHttpContext_ReturnsUnAuthorizedException()
        {
            //Arrange
            var config = new MapperConfiguration(cfg =>
                cfg.CreateMap<User, UserDetailsDto>()
            );
            var _mapperStub = new Mapper(config);

            var _userRepositoryStub = A.Fake<IUserRepository>();
            var _passwordHandlerStub = A.Fake<IPasswordHandler>();
            var _jwtHandlerStub = A.Fake<IJwtHandler>();
            var _userUpdateDtoValidatorStub = A.Fake<BaseValidator<UserUpdateDto>>();

            var testUserData = new TestUserData();
            var userUpdateDto = testUserData.GetUserUpdateDto();

            var expectedException = new UnAuthorizedException("Unauthorized");

            A.CallTo(() => _jwtHandlerStub.HttpContextExist()).Returns(true);
            A.CallTo(() => _jwtHandlerStub.GetClaimId()).Returns((10).ToString());

            var _userService = new UserService(_userRepositoryStub, _mapperStub, _passwordHandlerStub, _jwtHandlerStub, _userUpdateDtoValidatorStub);
            
            //Act
            var actualException = await Record.ExceptionAsync(() => _userService.UpdateUserByIdAsync(userUpdateDto, 1));
            //Assert
            Assert.NotNull(actualException);
            Assert.IsType<UnAuthorizedException>(actualException);
            Assert.Equal(expectedException.Message, actualException.Message);

            A.CallTo(() => _jwtHandlerStub.HttpContextExist()).MustHaveHappenedOnceExactly();
            A.CallTo(() => _jwtHandlerStub.GetClaimId()).MustHaveHappenedOnceExactly();
        }
        
        /*
        [Fact]
        public async void UpdateUSerByIdAsync_WithInvalidUserUpdateDto_ReturnsBadRequestException()
        {
            //Arrange
            var config = new MapperConfiguration(cfg =>
                cfg.CreateMap<User, UserDetailsDto>()
            );
            var _mapperStub = new Mapper(config);

            var _userRepositoryStub = A.Fake<IUserRepository>();
            var _passwordHandlerStub = A.Fake<IPasswordHandler>();
            var _jwtHandlerStub = A.Fake<IJwtHandler>();
            var _userUpdateDtoValidatorStub = new BaseValidator<UserUpdateDto>() ;

            var testUserData = new TestUserData();
            var userUpdateDto = testUserData.GetUserUpdateDto();
            userUpdateDto.Name = null;

            var expectedException = new BadRequestException();

            A.CallTo(() => _jwtHandlerStub.HttpContextExist()).Returns(true);
            A.CallTo(() => _jwtHandlerStub.GetClaimId()).Returns((1).ToString());
           
            var _userService = new UserService(_userRepositoryStub, _mapperStub, _passwordHandlerStub, _jwtHandlerStub, _userUpdateDtoValidatorStub);
            
            
            //Act
            var actualException = await Record.ExceptionAsync(() => _userService.UpdateUserByIdAsync(userUpdateDto, 1));
            //Assert
            Assert.NotNull(actualException);
            Assert.IsType<BadRequestException>(actualException);
            

        }
        */

        [Fact]
        public async void UpdateUSerByIdAsync_WithValidUserUpdateDtoAndValidHttpContext_ReturnsUpdatedUserDetailsDto()
        {
            //Arrange
            var _userRepositoryStub = A.Fake<IUserRepository>();
            var _mapperStub = A.Fake<IMapper>();
            var _passwordHandlerStub = A.Fake<IPasswordHandler>();
            var _jwtHandlerStub = A.Fake<IJwtHandler>();
            var _userUpdateDtoValidatorStub = A.Fake<BaseValidator<UserUpdateDto>>();

            var testUserData = new TestUserData();
            
            var userUpdateDto = testUserData.GetUserUpdateDto();

            User user = testUserData.GetUser(1);


            var expectedUpdatedUser = testUserData.CreateUserDetailsDtoObject(1);
            expectedUpdatedUser.Token = "A-New-Token";


            A.CallTo(() => _jwtHandlerStub.HttpContextExist()).Returns(true);

            A.CallTo(() => _jwtHandlerStub.GetClaimId()).Returns((1).ToString());

            A.CallTo(() => _mapperStub.Map<User>(userUpdateDto)).Returns(user);

            A.CallTo(() => _userRepositoryStub.UpdateUserByIdAsync(user, 1)).Returns(user);

            A.CallTo(() => _mapperStub.Map<UserDetailsDto>(user)).Returns(testUserData.CreateUserDetailsDtoObject(1));

            A.CallTo(() => _jwtHandlerStub.CreateToken(user)).Returns("A-New-Token");
            
            var _userService = new UserService(_userRepositoryStub, _mapperStub, _passwordHandlerStub, _jwtHandlerStub, _userUpdateDtoValidatorStub);

            
           

            //Act
            var actualUpdatedUser = await  _userService.UpdateUserByIdAsync(userUpdateDto, 1);
            //Assert
            Assert.NotNull(actualUpdatedUser);
            actualUpdatedUser.Should().BeEquivalentTo(expectedUpdatedUser);

            A.CallTo(() => _jwtHandlerStub.HttpContextExist()).MustHaveHappenedOnceExactly();

            A.CallTo(() => _jwtHandlerStub.GetClaimId()).MustHaveHappenedOnceExactly();

            A.CallTo(() => _mapperStub.Map<User>(userUpdateDto)).MustHaveHappenedOnceExactly();

            A.CallTo(() => _userRepositoryStub.UpdateUserByIdAsync(user, 1)).MustHaveHappenedOnceExactly();

            A.CallTo(() => _mapperStub.Map<UserDetailsDto>(user)).MustHaveHappenedOnceExactly();

            A.CallTo(() => _jwtHandlerStub.CreateToken(user)).MustHaveHappenedOnceExactly();


        }



        //public async Task<UserDetailsDto> GetUserByNameAsync(string name);

        [Fact]
        public async void GetUserByNameAsync_WithUnexistingUser_ReturnsNotFoundException()
        {
            // Arrange
            var config = new MapperConfiguration(cfg =>
                cfg.CreateMap<User, UserDetailsDto>()
            );
            var _mapperStub = new Mapper(config);

            var _userRepositoryStub = A.Fake<IUserRepository>();
            var _passwordHandlerStub = A.Fake<IPasswordHandler>();
            var _jwtHandlerStub = A.Fake<IJwtHandler>();
            var _userUpdateDtoValidatorStub = A.Fake<BaseValidator<UserUpdateDto>>();

            A.CallTo(() => _userRepositoryStub.GetUserByNameAsync("aName")).Returns((User)null);

            var _userService = new UserService(_userRepositoryStub, _mapperStub, _passwordHandlerStub, _jwtHandlerStub, _userUpdateDtoValidatorStub);
            var expectedException = new NotFoundException("No user Exists With This Name.");

            // Act
            var actualException = await Record.ExceptionAsync(() => _userService.GetUserByNameAsync("aName"));


            // Assert
            Assert.NotNull(actualException);
            Assert.IsType<NotFoundException>(actualException);
            Assert.Equal(expectedException.Message, actualException.Message);

            A.CallTo(() => _userRepositoryStub.GetUserByNameAsync("aName")).MustHaveHappenedOnceExactly();

        }

        [Fact]
        public async void GetUserByNameAsync_WithExistingUser_ReturnsUserDetailsDto()
        {
            // Arrange
            var config = new MapperConfiguration(cfg =>
                cfg.CreateMap<User, UserDetailsDto>()
            );
            var _mapperStub = new Mapper(config);

            var _userRepositoryStub = A.Fake<IUserRepository>();
            var _passwordHandlerStub = A.Fake<IPasswordHandler>();
            var _jwtHandlerStub = A.Fake<IJwtHandler>();
            var _userUpdateDtoValidatorStub = A.Fake<BaseValidator<UserUpdateDto>>();

            var testUserData = (new TestUserData()).GetUser(2);
            string userName = testUserData.UserName;

            A.CallTo(() => _userRepositoryStub.GetUserByNameAsync(userName)).Returns(testUserData);

            var _userService = new UserService(_userRepositoryStub, _mapperStub, _passwordHandlerStub, _jwtHandlerStub, _userUpdateDtoValidatorStub);
            var expectedUser = _mapperStub.Map<UserDetailsDto>(testUserData);

            // Act
            var actualUser = await  _userService.GetUserByNameAsync(userName);


            // Assert
            Assert.NotNull(actualUser);
            Assert.IsType<UserDetailsDto>(actualUser);
            actualUser.Should().BeEquivalentTo(expectedUser);
            A.CallTo(() => _userRepositoryStub.GetUserByNameAsync(userName)).MustHaveHappenedOnceExactly();

        }


        //public async Task<UserDetailsDto> GetUserByEmailAsync(string email);

        [Fact]
        public async void GetUserByEmailAsync_WithUnexistingUser_ReturnsNotFoundException()
        {
            // Arrange
            var config = new MapperConfiguration(cfg =>
                cfg.CreateMap<User, UserDetailsDto>()
            );
            var _mapperStub = new Mapper(config);

            var _userRepositoryStub = A.Fake<IUserRepository>();
            var _passwordHandlerStub = A.Fake<IPasswordHandler>();
            var _jwtHandlerStub = A.Fake<IJwtHandler>();
            var _userUpdateDtoValidatorStub = A.Fake<BaseValidator<UserUpdateDto>>();

            A.CallTo(() => _userRepositoryStub.GetUserByEmailAsync("aName")).Returns((User)null);

            var _userService = new UserService(_userRepositoryStub, _mapperStub, _passwordHandlerStub, _jwtHandlerStub, _userUpdateDtoValidatorStub);
            var expectedException = new NotFoundException("No user Exists With This Email.");

            // Act
            var actualException = await Record.ExceptionAsync(() => _userService.GetUserByEmailAsync("aName"));


            // Assert
            Assert.NotNull(actualException);
            Assert.IsType<NotFoundException>(actualException);
            Assert.Equal(expectedException.Message, actualException.Message);

            A.CallTo(() => _userRepositoryStub.GetUserByEmailAsync("aName")).MustHaveHappenedOnceExactly();

        }

        [Fact]
        public async void GetUserByEmailAsync_WithExistingUser_ReturnsUserDetailsDto()
        {
            // Arrange
            var config = new MapperConfiguration(cfg =>
                cfg.CreateMap<User, UserDetailsDto>()
            );
            var _mapperStub = new Mapper(config);

            var _userRepositoryStub = A.Fake<IUserRepository>();
            var _passwordHandlerStub = A.Fake<IPasswordHandler>();
            var _jwtHandlerStub = A.Fake<IJwtHandler>();
            var _userUpdateDtoValidatorStub = A.Fake<BaseValidator<UserUpdateDto>>();

            var testUserData = (new TestUserData()).GetUser(2);
            string Email = testUserData.Email;

            A.CallTo(() => _userRepositoryStub.GetUserByEmailAsync(Email)).Returns(testUserData);

            var _userService = new UserService(_userRepositoryStub, _mapperStub, _passwordHandlerStub, _jwtHandlerStub, _userUpdateDtoValidatorStub);
            var expectedUser = _mapperStub.Map<UserDetailsDto>(testUserData);

            // Act
            var actualUser = await _userService.GetUserByEmailAsync(Email);


            // Assert
            Assert.NotNull(actualUser);
            Assert.IsType<UserDetailsDto>(actualUser);
            actualUser.Should().BeEquivalentTo(expectedUser);
            A.CallTo(() => _userRepositoryStub.GetUserByEmailAsync(Email)).MustHaveHappenedOnceExactly();

        }


        //public async Task<UserDetailsDto> GetUserByUserNameAsync(string userName);

        [Fact]
        public async void GetUserByUserNameAsync_WithUnexistingUser_ReturnsNotFoundException()
        {
            // Arrange
            var config = new MapperConfiguration(cfg =>
                cfg.CreateMap<User, UserDetailsDto>()
            );
            var _mapperStub = new Mapper(config);

            var _userRepositoryStub = A.Fake<IUserRepository>();
            var _passwordHandlerStub = A.Fake<IPasswordHandler>();
            var _jwtHandlerStub = A.Fake<IJwtHandler>();
            var _userUpdateDtoValidatorStub = A.Fake<BaseValidator<UserUpdateDto>>();

            A.CallTo(() => _userRepositoryStub.GetUserByUserNameAsync("aName")).Returns((User)null);

            var _userService = new UserService(_userRepositoryStub, _mapperStub, _passwordHandlerStub, _jwtHandlerStub, _userUpdateDtoValidatorStub);
            var expectedException = new NotFoundException("No user Exists With This UserName.");

            // Act
            var actualException = await Record.ExceptionAsync(() => _userService.GetUserByUserNameAsync("aName"));


            // Assert
            Assert.NotNull(actualException);
            Assert.IsType<NotFoundException>(actualException);
            Assert.Equal(expectedException.Message, actualException.Message);

            A.CallTo(() => _userRepositoryStub.GetUserByUserNameAsync("aName")).MustHaveHappenedOnceExactly();

        }

        [Fact]
        public async void GetUserByUserNameAsync_WithExistingUser_ReturnsUserDetailsDto()
        {
            // Arrange
            var config = new MapperConfiguration(cfg =>
                cfg.CreateMap<User, UserDetailsDto>()
            );
            var _mapperStub = new Mapper(config);

            var _userRepositoryStub = A.Fake<IUserRepository>();
            var _passwordHandlerStub = A.Fake<IPasswordHandler>();
            var _jwtHandlerStub = A.Fake<IJwtHandler>();
            var _userUpdateDtoValidatorStub = A.Fake<BaseValidator<UserUpdateDto>>();

            var testUserData = (new TestUserData()).GetUser(2);
            string userName = testUserData.UserName;

            A.CallTo(() => _userRepositoryStub.GetUserByUserNameAsync(userName)).Returns(testUserData);

            var _userService = new UserService(_userRepositoryStub, _mapperStub, _passwordHandlerStub, _jwtHandlerStub, _userUpdateDtoValidatorStub);
            var expectedUser = _mapperStub.Map<UserDetailsDto>(testUserData);

            // Act
            var actualUser = await _userService.GetUserByUserNameAsync(userName);


            // Assert
            Assert.NotNull(actualUser);
            Assert.IsType<UserDetailsDto>(actualUser);
            actualUser.Should().BeEquivalentTo(expectedUser);
            A.CallTo(() => _userRepositoryStub.GetUserByUserNameAsync(userName)).MustHaveHappenedOnceExactly();

        }


    }
}