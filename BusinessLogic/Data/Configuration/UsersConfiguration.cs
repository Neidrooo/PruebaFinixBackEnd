using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Data.Configuration
{
    public class UsersConfiguration : IEntityTypeConfiguration<Users>
    {
        public void Configure(EntityTypeBuilder<Users> builder)
        {
            builder.Property(e=> e.FirstName).IsRequired();
            builder.Property(e => e.LastName).IsRequired();
            builder.Property(e => e.Email).IsRequired();
            builder.Property(e => e.Username).IsRequired();
            builder.Property(e => e.Password).IsRequired();

        }
    }
}
