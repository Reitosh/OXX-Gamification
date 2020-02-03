using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace OXXGame.Models
{

    public class Users
    {
        public int id { get; set; }
        public string Password { get; set; }
        public int LoginCounter { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Email { get; set; }
        /*public Boolean IsAdmin { get; set; }
        public Boolean KnowHtml { get; set; }
        public Boolean KnowCss { get; set; }
        public Boolean KnowJavascript { get; set; }
        public Boolean KnowCsharp { get; set; }
        public Boolean KnowMvc { get; set; }
        public Boolean KnowNetframework { get; set; }
        public Boolean KnowTypescript { get; set; }
        public Boolean KnowVue { get; set; }
        public Boolean KnowReact { get; set; }
        public Boolean KnowAngular { get; set; }*/

        //public virtual Results Result { get; set; }
        //public virtual List<SingleTestResults> SingleTestResults { get; set; }
    }

    public class Tasks
    {
        public int id { get; set; }
        public string Test { get; set; }
        public string Difficulty { get; set; }
        public string Category { get; set; }
    }

    public class Results
    {
        public int id { get; set; }
        public string TimeUsed { get; set; }
        public int TestsPassed { get; set; }
        public int TestsFailed { get; set; }
        public int Tests { get; set; }

        //public virtual Users user { get; set; }
    }

    public class SingleTestResults
    {
        public bool Passed { get; set; }
        public int Tries { get; set; }
        public string TimeSpent { get; set; }
        public int UserId { get; set; }
        public int TestId { get; set; }
        public Boolean Submitted { get; set; }

        //public virtual Tasks task { get; set; }
        //public virtual Users user { get; set; }
    }

    public class OXXGameDBContext : DbContext
    {
        public OXXGameDBContext(DbContextOptions<OXXGameDBContext> options)
            : base(options)
        { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<SingleTestResults>()
                .HasKey(k => new { k.UserId, k.TestId });
        }

        public DbSet<Users> Users { get; set; }
        public DbSet<Tasks> Tests { get; set; }
        public DbSet<SingleTestResults> SingleTestResults { get; set; }
        public DbSet<Results> Results { get; set; }
    }
}