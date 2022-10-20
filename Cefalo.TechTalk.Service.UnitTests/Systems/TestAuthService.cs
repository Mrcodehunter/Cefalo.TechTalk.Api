using AutoMapper;
using Cefalo.TechTalk.Database.Models;
using Cefalo.TechTalk.Repository.Contracts;
using Cefalo.TechTalk.Service.Contracts;
using Cefalo.TechTalk.Service.DTOs;
using Cefalo.TechTalk.Service.Services;
using Cefalo.TechTalk.Service.UnitTests.Fixtures;
using Cefalo.TechTalk.Service.Utils.Contracts;
using Cefalo.TechTalk.Service.Utils.CustomErrorHandler;
using Cefalo.TechTalk.Service.Utils.DtoValidators;
using Cefalo.TechTalk.Service.Utils.Services;
using FakeItEasy;
using FluentAssertions;
using Newtonsoft.Json.Linq;
using NPOI.SS.Formula.Functions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cefalo.TechTalk.Service.UnitTests.Systems
{
    public class TestAuthService
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
        //public async Task<UserDetailsDto> SignUpAsync(UserSignUpDto userSignUpDto)
        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        public async void SignUpAsync_ToSignUp_ReturnSignUpDto(int id)
        {
            // Arrange
            var config = new MapperConfiguration(cfg =>
               {
                   cfg.CreateMap<User, UserDetailsDto>();
                   cfg.CreateMap<UserSignUpDto, User>();
               }
            );
            var _mapperStub = new Mapper(config);

            var _userRepositoryStub = A.Fake<IUserRepository>();
            var _passwordHandlerStub = A.Fake<IPasswordHandler>();
            var _jwtHandlerStub = A.Fake<IJwtHandler>();
            var _cookieHandlerStub = A.Fake<ICookieHandler>();
            var _dateTimeHandlerStub = A.Fake<IDateTimeHandler>();
            var _userSignInDtoValidatorStub = A.Fake<BaseValidator<UserSignInDto>>();
            var _userSignUpDtoValidatorStub = A.Fake<BaseValidator<UserSignUpDto>>();

            var testAuthDataObject = new TestAuthData();
            var returnableUser = testAuthDataObject.GetReturnableUser(id);
            var userSignUpDto = testAuthDataObject.GetUserSignUpDto();
            var callableUser = testAuthDataObject.GetCallableUser();
            var userDetailsDto = testAuthDataObject.GetUserDetailsDtoObject(id);

            A.CallTo(() => _passwordHandlerStub.CreatePasswordHash(userSignUpDto.Password)).Returns(testAuthDataObject.GetAByteTuple());
            A.CallTo(() => _dateTimeHandlerStub.GetDateTimeInUtcNow()).Returns(testAuthDataObject.GetDateTime());
            A.CallTo(() => _userRepositoryStub
                                .CreateUserAsync(A<User>.That.Matches(us => CheckObjectEquality(us, callableUser))))
                                .Returns(returnableUser);
            A.CallTo(() => _jwtHandlerStub.CreateToken(returnableUser)).Returns(userDetailsDto.Token);
            A.CallTo(() => _cookieHandlerStub.Set("Token", userDetailsDto.Token)).DoesNothing();

            var authService = new AuthService(_userRepositoryStub, _mapperStub, _passwordHandlerStub, _jwtHandlerStub, _cookieHandlerStub, _dateTimeHandlerStub, _userSignInDtoValidatorStub, _userSignUpDtoValidatorStub);

            var expectedUser = userDetailsDto;

            // Act
            var actualUser = await authService.SignUpAsync(userSignUpDto);


            // Assert
            Assert.NotNull(actualUser);
            Assert.IsType<UserDetailsDto>(actualUser);
            actualUser.Should().BeEquivalentTo(expectedUser);

            A.CallTo(() => _passwordHandlerStub.CreatePasswordHash(userSignUpDto.Password)).MustHaveHappenedOnceExactly();
            A.CallTo(() => _dateTimeHandlerStub.GetDateTimeInUtcNow()).MustHaveHappenedTwiceOrMore();
            A.CallTo(() => _userRepositoryStub
                                .CreateUserAsync(A<User>.That.Matches(us => CheckObjectEquality(us, callableUser))))
                                .MustHaveHappenedOnceExactly();
            A.CallTo(() => _jwtHandlerStub.CreateToken(returnableUser)).MustHaveHappenedOnceExactly();
            A.CallTo(() => _cookieHandlerStub.Set("Token", userDetailsDto.Token)).MustHaveHappenedOnceExactly();


        }


        //public async Task<UserDetailsDto> SignInAsync(UserSignInDto userSignInDto)
        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        public async void SignInAsync_WithInvalidUserNameOrPassword_ReturnsBadRequestException(int id)
        {
            // Arrange
            var config = new MapperConfiguration(cfg =>
                cfg.CreateMap<User, UserDetailsDto>()
            );
            var _mapperStub = new Mapper(config);

            var _userRepositoryStub = A.Fake<IUserRepository>();
            var _passwordHandlerStub = A.Fake<IPasswordHandler>();
            var _jwtHandlerStub = A.Fake<IJwtHandler>();
            var _cookieHandlerStub = A.Fake<ICookieHandler>();
            var _dateTimeHandlerStub = A.Fake<IDateTimeHandler>();
            var _userSignInDtoValidatorStub = A.Fake<BaseValidator<UserSignInDto>>();
            var _userSignUpDtoValidatorStub = A.Fake<BaseValidator<UserSignUpDto>>();

            var testAuthDataObject = new TestAuthData();
            var returnableUser = testAuthDataObject.GetReturnableUser(id);
            var userSignInDto = testAuthDataObject.GetUserSignInDto();

            A.CallTo(() => _userRepositoryStub.GetUserByUserNameAsync(userSignInDto.UserName)).Returns(returnableUser);
            A.CallTo(() => _passwordHandlerStub.VerifyPasswordHash(userSignInDto.Password, returnableUser.PasswordHash, returnableUser.PasswordSalt)).Returns(false);

            var authService = new AuthService(_userRepositoryStub, _mapperStub, _passwordHandlerStub, _jwtHandlerStub, _cookieHandlerStub, _dateTimeHandlerStub, _userSignInDtoValidatorStub, _userSignUpDtoValidatorStub);

            var expectedException = new BadRequestException("Username or password is incorrect");

            // Act
            var actualException = await Record.ExceptionAsync(() => authService.SignInAsync(userSignInDto));


            // Assert
            Assert.NotNull(actualException);
            Assert.IsType<BadRequestException>(actualException);
            Assert.Equal(expectedException.Message, actualException.Message);

            A.CallTo(() => _userRepositoryStub.GetUserByUserNameAsync(userSignInDto.UserName)).MustHaveHappenedOnceExactly();
            A.CallTo(() => _passwordHandlerStub.VerifyPasswordHash(userSignInDto.Password, returnableUser.PasswordHash, returnableUser.PasswordSalt)).MustHaveHappenedOnceExactly();

        }

        [Theory]
        [InlineData(1)]
        [InlineData(3)]
        public async void SignInAsync_WithValidCredentials_ReturnsUserDetailsDto(int id)
        {
            // Arrange
            var config = new MapperConfiguration(cfg =>
                cfg.CreateMap<User, UserDetailsDto>()
            );
            var _mapperStub = new Mapper(config);

            var _userRepositoryStub = A.Fake<IUserRepository>();
            var _passwordHandlerStub = A.Fake<IPasswordHandler>();
            var _jwtHandlerStub = A.Fake<IJwtHandler>();
            var _cookieHandlerStub = A.Fake<ICookieHandler>();
            var _dateTimeHandlerStub = A.Fake<IDateTimeHandler>();
            var _userSignInDtoValidatorStub = A.Fake<BaseValidator<UserSignInDto>>();
            var _userSignUpDtoValidatorStub = A.Fake<BaseValidator<UserSignUpDto>>();

            var testAuthDataObject = new TestAuthData();
            var returnableUser = testAuthDataObject.GetReturnableUser(id);
            var userSignInDto = testAuthDataObject.GetUserSignInDto();
            var userDetailsDto = testAuthDataObject.GetUserDetailsDtoObject(id);

            A.CallTo(() => _userRepositoryStub.GetUserByUserNameAsync(userSignInDto.UserName)).Returns(returnableUser);
            A.CallTo(() => _passwordHandlerStub.VerifyPasswordHash(userSignInDto.Password, returnableUser.PasswordHash, returnableUser.PasswordSalt)).Returns(true);
            A.CallTo(() => _jwtHandlerStub.CreateToken(returnableUser)).Returns(userDetailsDto.Token);
            A.CallTo(() => _cookieHandlerStub.Set("Token", userDetailsDto.Token)).DoesNothing();

            var authService = new AuthService(_userRepositoryStub, _mapperStub, _passwordHandlerStub, _jwtHandlerStub, _cookieHandlerStub, _dateTimeHandlerStub, _userSignInDtoValidatorStub, _userSignUpDtoValidatorStub);

            var expectedUser = userDetailsDto;

            // Act
            var actualUser = await authService.SignInAsync(userSignInDto);


            // Assert
            Assert.NotNull(actualUser);
            Assert.IsType<UserDetailsDto>(actualUser);
            actualUser.Should().BeEquivalentTo(expectedUser);

            A.CallTo(() => _userRepositoryStub.GetUserByUserNameAsync(userSignInDto.UserName)).MustHaveHappenedOnceExactly();
            A.CallTo(() => _passwordHandlerStub.VerifyPasswordHash(userSignInDto.Password, returnableUser.PasswordHash, returnableUser.PasswordSalt)).MustHaveHappenedOnceExactly();
            A.CallTo(() => _jwtHandlerStub.CreateToken(returnableUser)).MustHaveHappenedOnceExactly();
            A.CallTo(() => _cookieHandlerStub.Set("Token", userDetailsDto.Token)).MustHaveHappenedOnceExactly();

        }

        //public async Task<UserDetailsDto> VerifyTokenAsync()

        [Fact]
        public async void VerifyTokenAsync_WithNoHttpContext_ReturnsBadRequestException()
        {
            // Arrange
            var config = new MapperConfiguration(cfg =>
                cfg.CreateMap<User, UserDetailsDto>()
            );
            var _mapperStub = new Mapper(config);

            var _userRepositoryStub = A.Fake<IUserRepository>();
            var _passwordHandlerStub = A.Fake<IPasswordHandler>();
            var _jwtHandlerStub = A.Fake<IJwtHandler>();
            var _cookieHandlerStub = A.Fake<ICookieHandler>();
            var _dateTimeHandlerStub = A.Fake<IDateTimeHandler>();
            var _userSignInDtoValidatorStub = A.Fake<BaseValidator<UserSignInDto>>();
            var _userSignUpDtoValidatorStub = A.Fake<BaseValidator<UserSignUpDto>>();

            A.CallTo(() => _jwtHandlerStub.HttpContextExist()).Returns(false);

            var authService = new AuthService(_userRepositoryStub, _mapperStub, _passwordHandlerStub, _jwtHandlerStub, _cookieHandlerStub, _dateTimeHandlerStub, _userSignInDtoValidatorStub, _userSignUpDtoValidatorStub);

            var expectedException = new BadRequestException("Not Loged In");

            // Act
            var actualException = await Record.ExceptionAsync(() => authService.VerifyTokenAsync());


            // Assert
            Assert.NotNull(actualException);
            Assert.IsType<BadRequestException>(actualException);
            Assert.Equal(expectedException.Message, actualException.Message);

            A.CallTo(() => _jwtHandlerStub.HttpContextExist()).MustHaveHappenedOnceExactly();

        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        public async void VerifyTokenAsync_ToVerifyToken_ReturnsUserDetailsDto(int id)
        {
            // Arrange
            var config = new MapperConfiguration(cfg =>
                cfg.CreateMap<User, UserDetailsDto>()
            );
            var _mapperStub = new Mapper(config);

            var _userRepositoryStub = A.Fake<IUserRepository>();
            var _passwordHandlerStub = A.Fake<IPasswordHandler>();
            var _jwtHandlerStub = A.Fake<IJwtHandler>();
            var _cookieHandlerStub = A.Fake<ICookieHandler>();
            var _dateTimeHandlerStub = A.Fake<IDateTimeHandler>();
            var _userSignInDtoValidatorStub = A.Fake<BaseValidator<UserSignInDto>>();
            var _userSignUpDtoValidatorStub = A.Fake<BaseValidator<UserSignUpDto>>();

            var testAuthDataObject = new TestAuthData();
            var returnableUser = testAuthDataObject.GetReturnableUser(id);

            A.CallTo(() => _jwtHandlerStub.HttpContextExist()).Returns(true);
            A.CallTo(() => _jwtHandlerStub.GetClaimName()).Returns(returnableUser.UserName);
            A.CallTo(() => _userRepositoryStub.GetUserByUserNameAsync(returnableUser.UserName)).Returns(returnableUser);
            A.CallTo(() => _cookieHandlerStub.Get("Token")).Returns("A-Token");
            A.CallTo(() => _cookieHandlerStub.Set("Token", "A-Token")).DoesNothing();

            var authService = new AuthService(_userRepositoryStub, _mapperStub, _passwordHandlerStub, _jwtHandlerStub, _cookieHandlerStub, _dateTimeHandlerStub, _userSignInDtoValidatorStub, _userSignUpDtoValidatorStub);

            var expectedUser = testAuthDataObject.GetUserDetailsDtoObject(id);

            // Act
            var actualUser = await authService.VerifyTokenAsync();


            // Assert
            Assert.NotNull(actualUser);
            Assert.IsType<UserDetailsDto>(actualUser);
            actualUser.Should().BeEquivalentTo(expectedUser);

            A.CallTo(() => _jwtHandlerStub.HttpContextExist()).MustHaveHappenedOnceExactly();
            A.CallTo(() => _jwtHandlerStub.GetClaimName()).MustHaveHappenedOnceExactly();
            A.CallTo(() => _userRepositoryStub.GetUserByUserNameAsync(returnableUser.UserName)).MustHaveHappenedOnceExactly();
            A.CallTo(() => _cookieHandlerStub.Get("Token")).MustHaveHappenedOnceExactly();
            A.CallTo(() => _cookieHandlerStub.Set("Token", "A-Token")).MustHaveHappenedOnceExactly();

        }



        //public Task<string> Logout()
        [Fact]
        public void Logout_ToLogout_ReturnsNothing()
        {
            // Arrange
            var config = new MapperConfiguration(cfg =>
                cfg.CreateMap<User, UserDetailsDto>()
            );
            var _mapperStub = new Mapper(config);

            var _userRepositoryStub = A.Fake<IUserRepository>();
            var _passwordHandlerStub = A.Fake<IPasswordHandler>();
            var _jwtHandlerStub = A.Fake<IJwtHandler>();
            var _cookieHandlerStub = A.Fake<ICookieHandler>();
            var _dateTimeHandlerStub = A.Fake<IDateTimeHandler>();
            var _userSignInDtoValidatorStub = A.Fake<BaseValidator<UserSignInDto>>();
            var _userSignUpDtoValidatorStub = A.Fake<BaseValidator<UserSignUpDto>>();

            A.CallTo(() => _cookieHandlerStub.Remove("Token")).DoesNothing();

            var authService = new AuthService(_userRepositoryStub, _mapperStub, _passwordHandlerStub, _jwtHandlerStub, _cookieHandlerStub, _dateTimeHandlerStub, _userSignInDtoValidatorStub, _userSignUpDtoValidatorStub);

            // Act

            var expectedResponse = authService.Logout();


            //Assert

            Assert.Null(expectedResponse);

            A.CallTo(() => _cookieHandlerStub.Remove("Token")).MustHaveHappenedOnceExactly();
        }
    }
}
