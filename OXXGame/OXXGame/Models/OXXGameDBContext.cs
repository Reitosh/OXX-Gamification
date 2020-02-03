using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace OXXGame.Models
{
    public class OXXGameDBContext : DbContext
    {
        public OXXGameDBContext(DbContextOptions<OXXGameDBContext> options)
            : base(options)
        { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {

        }

        public DbSet<User> Users { get; set; }
        public DbSet<Task> Tests { get; set; }
        public DbSet<SingleTestResult> SingleTestResult { get; set; }
        public DbSet<Result> Results { get; set; }

    }
}