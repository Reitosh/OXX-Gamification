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
        public string Email { get; set; }
        public byte[] Password { get; set; }
        public int LoginCounter { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public bool IsAdmin { get; set; }
        public bool KnowHtml { get; set; }
        public bool KnowCss { get; set; }
        public bool KnowJavascript { get; set; }
        public bool KnowCsharp { get; set; }
        public bool KnowMvc { get; set; }
        public bool KnowNetframework { get; set; }
        public bool KnowTypescript { get; set; }
        public bool KnowVue { get; set; }
        public bool KnowReact { get; set; }
        public bool KnowAngular { get; set; }

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
        [Key]
        public int UserId { get; set; }
        public string TimeUsed { get; set; }
        public int TestsPassed { get; set; }
        public int TestsFailed { get; set; }
        public int Tests { get; set; }

        //public virtual Users user { get; set; }
    }

    public class SingleTestResults
    {
        public int UserId { get; set; }
        public int TestId { get; set; }
        public bool Passed { get; set; }
        public int Attempts { get; set; }
        public string TimeUsed { get; set; }
        public bool Submitted { get; set; }

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
        public DbSet<Tasks> Tasks { get; set; }
        public DbSet<SingleTestResults> SingleTestResults { get; set; }
        public DbSet<Results> Results { get; set; }
    }
}