using AutoFixture;
using Cefalo.TechTalk.Database.Context;
using Cefalo.TechTalk.Database.Models;
using Cefalo.TechTalk.Repository.UnitTests.Fixtures;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cefalo.TechTalk.Repository.UnitTests.Helpers
{
    public class InMemoryContext
    {
       // var options = new DbContextOptionsBuilder<DataContext>().UseInMemoryDatabase(databaseName: "fakeDataContext").Options;
        //fakeDataContext = new DataContext(options);
        private readonly TechTalkContext _fakeDbcontext;
        private readonly Fixture _fixture;
        private readonly TestUserRepositoryData _testUserRepositoryData;
        public InMemoryContext()
        {
            var options = new DbContextOptionsBuilder<TechTalkContext>().UseInMemoryDatabase(databaseName: "fakeDbContext").Options;
            _fakeDbcontext = new TechTalkContext(options);
            _fixture = new Fixture();
            _testUserRepositoryData = new TestUserRepositoryData();

        }

        public virtual TechTalkContext GetFakeContext()
        {
            return _fakeDbcontext;  
        }

        public virtual Fixture GetFixture()
        {
            return this._fixture;
        }

        public virtual User CreateUser(int id)
        {
            //var user =  _fixture.Create<User>();
            var user = _testUserRepositoryData.CreateUser(id);
           return user;
        }
        public virtual List<User> CreateUserList()
        {
            var users = _fixture.Create<List<User>>();
            return users;
        }

        public async virtual void AddUserList(List<User> users)
        {
            await _fakeDbcontext.AddRangeAsync(users);
        }

        public async virtual  void AddUser(User user)
        {
            await _fakeDbcontext.AddAsync(user);
        }


    }
}
