using Cefalo.TechTalk.Database.Configurations;
using Cefalo.TechTalk.Database.Models;
using Microsoft.EntityFrameworkCore;


namespace Cefalo.TechTalk.Database.Context
{
    public class TechTalkContext : DbContext
    {
        public TechTalkContext(DbContextOptions<TechTalkContext> options): base(options){  }

        public DbSet<User> Users { get; set; }
        public DbSet<Blog> Blogs { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            new UserConfiguration().Configure(builder.Entity<User>());
            new BlogConfiguration().Configure(builder.Entity<Blog>());
        }
    }
}
