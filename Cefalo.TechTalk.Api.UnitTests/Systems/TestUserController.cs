using Cefalo.TechTalk.Api.Controllers;
using Cefalo.TechTalk.Api.UnitTests.Fixtures;
using Cefalo.TechTalk.Service.Contracts;
using Cefalo.TechTalk.Service.DTOs;
using FakeItEasy;
using FluentAssertions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cefalo.TechTalk.Api.UnitTests.Systems
{
    public class TestUserController
    {
        //public async Task<ActionResult> GetAllAsync()
        [Fact]
        public async void GetAllAsync_ForUserList_ReturnsUserList()
        {
            //Arrange
            var _userService = A.Fake<IUserService>();

            var testUserControllerDataObject = new TestUserControllerData();
            var userList = testUserControllerDataObject.GetUserList();

            A.CallTo(() => _userService.GetAllAsync()).Returns(userList);

            var userController = new UserController(_userService);

            //Act

            var actualResponse = await userController.GetAllAsync();

            //Assert

            Assert.NotNull(actualResponse);
            actualResponse.Should().BeOfType<OkObjectResult>();
            A.CallTo(() => _userService.GetAllAsync()).MustHaveHappenedOnceExactly();

            var responseObject = (OkObjectResult)actualResponse;
            responseObject.Value.Should().NotBeNull();
            responseObject.Value.Should().BeOfType<List<UserDetailsDto>>();
            responseObject.Value.Should().BeEquivalentTo(userList);
            responseObject.StatusCode.Should().Be(200);
        }

        //public async Task<ActionResult> GetUserByIdAsync(int id)
        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        public async void GetUserByIdAsync_WithUserId_ReturnsUserDetails(int id)
        {

            //Arrange
            var _userService = A.Fake<IUserService>();
            var testUserControllerDataObject = new TestUserControllerData();
            var userDetailsDto = testUserControllerDataObject.GetUserDetailsDto(id);

            A.CallTo(() => _userService.GetUserByIdAsync(id)).Returns(userDetailsDto);

            var userController = new UserController(_userService);

            //Act

            var actualResponse = await userController.GetUserByIdAsync(id);

            //Assert

            Assert.NotNull(actualResponse);
            actualResponse.Should().BeOfType<OkObjectResult>();
            A.CallTo(() => _userService.GetUserByIdAsync(id)).MustHaveHappenedOnceExactly();

            var responseObject = (OkObjectResult)actualResponse;
            responseObject.Value.Should().NotBeNull();
            responseObject.Value.Should().BeOfType<UserDetailsDto>();
            responseObject.Value.Should().BeEquivalentTo(userDetailsDto);
            responseObject.StatusCode.Should().Be(200);



        }
        //public async Task<ActionResult> UpdateUserAsync(UserUpdateDto userUpdateDto, int id)

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        public async void UpdateUserAsync_WithUserUpdateDtoAndId_ReturnsUserDetails(int id)
        {

            //Arrange
            var _userService = A.Fake<IUserService>();

            var testUserControllerDataObject = new TestUserControllerData();

            var userUpdateDto = testUserControllerDataObject.GetUserUpdateDto();

            var userDetailsDto = testUserControllerDataObject.GetUserDetailsDto(id);

            A.CallTo(() => _userService.UpdateUserByIdAsync(userUpdateDto,id)).Returns(userDetailsDto);

            var userController = new UserController(_userService);

            //Act

            var actualResponse = await userController.UpdateUserAsync(userUpdateDto, id);

            //Assert

            Assert.NotNull(actualResponse);
            actualResponse.Should().BeOfType<OkObjectResult>();
            A.CallTo(() => _userService.UpdateUserByIdAsync(userUpdateDto, id)).MustHaveHappenedOnceExactly();

            var responseObject = (OkObjectResult)actualResponse;
            responseObject.Value.Should().NotBeNull();
            responseObject.Value.Should().BeOfType<UserDetailsDto>();
            responseObject.Value.Should().BeEquivalentTo(userDetailsDto);
            responseObject.StatusCode.Should().Be(200);

        }

        //public async Task<ActionResult> GetUserByUserNameAsync(string userName)

        [Theory]
        [InlineData(1,"firstName")]
        [InlineData(2,"lastName")]
        public async void GetUserByUserNameAsync_WithUserName_ReturnsUserDetails(int id, string userName)
        {

            //Arrange
            var _userService = A.Fake<IUserService>();

            var testUserControllerDataObject = new TestUserControllerData();

            var userDetailsDto = testUserControllerDataObject.GetUserDetailsDtoByUserNameAndId(id,userName);

            A.CallTo(() => _userService.GetUserByUserNameAsync(userName)).Returns(userDetailsDto);

            var userController = new UserController(_userService);

            //Act

            var actualResponse = await userController.GetUserByUserNameAsync(userName);

            //Assert

            Assert.NotNull(actualResponse);
            actualResponse.Should().BeOfType<OkObjectResult>();
            A.CallTo(() => _userService.GetUserByUserNameAsync(userName)).MustHaveHappenedOnceExactly();

            var responseObject = (OkObjectResult)actualResponse;
            responseObject.Value.Should().NotBeNull();
            responseObject.Value.Should().BeOfType<UserDetailsDto>();
            responseObject.Value.Should().BeEquivalentTo(userDetailsDto);
            responseObject.StatusCode.Should().Be(200);

        }


    }
}
