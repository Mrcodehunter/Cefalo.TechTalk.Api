using Cefalo.TechTalk.Database.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace Cefalo.TechTalk.Database.Configurations
{
    public class BlogConfiguration : IEntityTypeConfiguration<Blog>
    {
        public void Configure(EntityTypeBuilder<Blog> builder)
        {
            builder
                .HasKey(x => x.Id);

            builder
                .Property(x => x.AuthorName)
                .IsRequired(true)
                .HasMaxLength(60);

            builder
                .Property(x => x.AuthorId)
                .IsRequired(true);

            builder
                .HasIndex(x => x.Title)
                .IsUnique(true);
            builder
                .Property(x => x.AuthorId)
                .HasMaxLength(1000)
                .IsRequired(true);
                
            builder
                .Property(x => x.Body)
                .HasMaxLength(10000)
                .IsRequired(true);

            builder
                .HasOne<User>(x => x.User)
                .WithMany(x => x.Blogs)
                .HasForeignKey(x => x.Id);

        }
    }
}
