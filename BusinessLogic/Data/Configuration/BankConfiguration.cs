using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Data.Configuration
{
    public class BankConfiguration:IEntityTypeConfiguration<Banks>
    {
        public void Configure(EntityTypeBuilder<Banks> builder)
        {
            builder.Property(e => e.Id).ValueGeneratedOnAdd();
            builder.Property(e=> e.Uid).IsRequired();
            builder.Property(e => e.Account_number).IsRequired();
            builder.Property(e => e.Iban).IsRequired();
            builder.Property(e => e.Bank_name).IsRequired();
            builder.Property(e => e.Routing_number).IsRequired();
            builder.Property(e => e.Swift_bic).IsRequired();
            builder.Property(e => e.Created)
                .HasDefaultValueSql("CURRENT_TIMESTAMP").IsRequired();
        }
    }
}
