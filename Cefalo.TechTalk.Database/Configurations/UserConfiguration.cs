using Cefalo.TechTalk.Database.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cefalo.TechTalk.Database.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder
                .HasKey(x => x.Id);

            builder
                .Property(x => x.Name)
                .IsRequired(true)
                .HasMaxLength(60);

            builder
                .HasIndex(x => x.UserName)
                .IsUnique(true);
            builder
                .Property(x => x.UserName)
                .HasMaxLength(30)
                .IsRequired(true);

            builder
                .Property(x => x.PasswordHash)
                .IsRequired(true);
                

            builder
                .Property(x => x.PasswordSalt)
                .IsRequired(true);


            builder
                .Property(x => x.Email)
                .IsRequired(true);
            builder
                .HasIndex(x => x.Email)
                .IsUnique(true);

            
            
            
        }
    }
}
