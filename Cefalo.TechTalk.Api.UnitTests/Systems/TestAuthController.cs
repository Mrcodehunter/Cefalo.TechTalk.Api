using Cefalo.TechTalk.Api.Controllers;
using Cefalo.TechTalk.Api.UnitTests.Fixtures;
using Cefalo.TechTalk.Repository.Contracts;
using Cefalo.TechTalk.Service.Contracts;
using Cefalo.TechTalk.Service.DTOs;
using Cefalo.TechTalk.Service.Utils.Contracts;
using FakeItEasy;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cefalo.TechTalk.Api.UnitTests.Systems
{
    public class TestAuthController
    {
        //public async Task<ActionResult> SignUpAsync(UserSignUpDto userSignUpDto)
        [Theory]
        [InlineData(3)]
        [InlineData(4)]
        public async void SignUpAsync_WithSignUpDto_ReturnsUserDetailsDto(int id)
        {

            //Arrange
            var _authServiceStub = A.Fake<IAuthService>();
           

            var testAuthControllerData = new TestAuthControllerData();

            var userSignUpDto = testAuthControllerData.GetUserSignUpDto();
            var userDetailsDto = testAuthControllerData.GetUserDetailsDto(id);

            A.CallTo(() => _authServiceStub.SignUpAsync(userSignUpDto)).Returns(userDetailsDto);

            var authController = new AuthController(_authServiceStub);

            //Act

            var actualResponse = await authController.SignUpAsync(userSignUpDto);

            //Assert

            Assert.NotNull(actualResponse);
            actualResponse.Should().BeOfType<OkObjectResult>();
            A.CallTo(() => _authServiceStub.SignUpAsync(userSignUpDto)).MustHaveHappenedOnceExactly();

            var responseObject = (OkObjectResult)actualResponse;
            responseObject.Value.Should().NotBeNull();
            responseObject.Value.Should().BeOfType<UserDetailsDto>();
            responseObject.Value.Should().BeEquivalentTo(userDetailsDto);
            responseObject.StatusCode.Should().Be(200);

        }


        //public async Task<ActionResult> SignInAsync(UserSignInDto userSignInDto)

        [Theory]
        [InlineData(0)]
        [InlineData(5)]
        public async void SignInAsync_WithSignInDto_ReturnsUserDetailsDto(int id)
        {

            //Arrange
            var _authServiceStub = A.Fake<IAuthService>();
           

            var testAuthControllerData = new TestAuthControllerData();

            var userSignInDto = testAuthControllerData.GetUserSignInDto();
            var userDetailsDto = testAuthControllerData.GetUserDetailsDto(id);

            A.CallTo(() => _authServiceStub.SignInAsync(userSignInDto)).Returns(userDetailsDto);

            var authController = new AuthController(_authServiceStub);

            //Act

            var actualResponse = await authController.SignInAsync(userSignInDto);

            //Assert

            Assert.NotNull(actualResponse);
            actualResponse.Should().BeOfType<OkObjectResult>();
            A.CallTo(() => _authServiceStub.SignInAsync(userSignInDto)).MustHaveHappenedOnceExactly();

            var responseObject = (OkObjectResult)actualResponse;
            responseObject.Value.Should().NotBeNull();
            responseObject.Value.Should().BeOfType<UserDetailsDto>();
            responseObject.Value.Should().BeEquivalentTo(userDetailsDto);
            responseObject.StatusCode.Should().Be(200);

        }

        //public async Task<ActionResult> VerifyTokenAsync()
        [Theory]
        [InlineData(0)]
        [InlineData(5)]
        public async void VerifyAsync_ToVerifyToken_ReturnsUserDetailsDto(int id)
        {

            //Arrange
            var _authServiceStub = A.Fake<IAuthService>();

            var testAuthControllerData = new TestAuthControllerData();

            var userDetailsDto = testAuthControllerData.GetUserDetailsDto(id);

            A.CallTo(() => _authServiceStub.VerifyTokenAsync()).Returns(userDetailsDto);

            var authController = new AuthController(_authServiceStub);

            //Act

            var actualResponse = await authController.VerifyTokenAsync();

            //Assert

            Assert.NotNull(actualResponse);
            actualResponse.Should().BeOfType<OkObjectResult>();
            A.CallTo(() => _authServiceStub.VerifyTokenAsync()).MustHaveHappenedOnceExactly();

            var responseObject = (OkObjectResult)actualResponse;
            responseObject.Value.Should().NotBeNull();
            responseObject.Value.Should().BeOfType<UserDetailsDto>();
            responseObject.Value.Should().BeEquivalentTo(userDetailsDto);
            responseObject.StatusCode.Should().Be(200);

        }


        //public async Task<ActionResult> Logout()
        [Fact]
        public async void Logout_ToLogout_ReturnsTrue()
        {

            //Arrange
            var _authServiceStub = A.Fake<IAuthService>();
           
            A.CallTo(() => _authServiceStub.Logout()).Returns((string)null);

            var authController = new AuthController(_authServiceStub);

            //Act

            var actualResponse = await authController.Logout();

            //Assert

            actualResponse.Should().BeOfType<OkObjectResult>();
            A.CallTo(() => _authServiceStub.Logout()).MustHaveHappenedOnceExactly();

            var responseObject = (OkObjectResult)actualResponse;
            responseObject.Value.Should().NotBeNull();
            responseObject.StatusCode.Should().Be(200);
            
        }
       
    }
}
