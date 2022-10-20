using Cefalo.TechTalk.Database.Context;
using Cefalo.TechTalk.Database.Models;
using Cefalo.TechTalk.Repository.Repositories;
using Cefalo.TechTalk.Repository.UnitTests.Helpers;
using FakeItEasy;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cefalo.TechTalk.Repository.UnitTests.Systems
{
    public class TestUserRepository
    {
        //public async Task<User> CreateUserAsync(User user)

        //public async Task<List<User>> GetAllAsync()

        //public async Task<User> GetUserByIdAsync(int id)

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        public async void GetUserByIdAsync_WithId_ReturnsUser(int id)
        {
            //Arrange
            var contextObject = new InMemoryContext();

            var _fakeContext = contextObject.GetFakeContext();

            var userRepository = new UserRepository(_fakeContext);

            var user = contextObject.CreateUser(id);

            _fakeContext.Add(user);

            await _fakeContext.SaveChangesAsync();

            //Act

            var actualUser = await userRepository.GetUserByIdAsync(id);

            //Assert

            Assert.NotNull(actualUser);
           
        }
        
        //public async Task<User> UpdateUserByIdAsync(User user, int id)

        //public async Task<User> GetUserByNameAsync(string name)
        
        //public async Task<User> GetUserByEmailAsync(string email)
        
        //public async Task<User> GetUserByUserNameAsync(string userName)
        
    }
}
