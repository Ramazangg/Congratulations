using Congratulations.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Congratulations.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext() => Database.EnsureCreated();
        public DbSet<User> Users { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=Data.db");
        }
    }
}
