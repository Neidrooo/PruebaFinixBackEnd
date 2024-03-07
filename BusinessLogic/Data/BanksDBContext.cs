using Core.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Data
{
    public class BanksDBContext : DbContext
    {
        public BanksDBContext(DbContextOptions<BanksDBContext> options) : base(options)
        {
        }
        public DbSet<Users> Users { get; set; }
        public DbSet<Banks> Banks { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        }
    }
}
