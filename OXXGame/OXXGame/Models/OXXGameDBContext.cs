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
        public string Tlf { get; set; }
        public byte[] Password { get; set; }
        public int LoginCounter { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public bool IsAdmin { get; set; }
    }

    public class Categories
    {
        [Key]
        public string Category { get; set; }
    }

    public class ResultsPerCategory
    {
        public int UserId { get; set; }
        public string Category { get; set; }
        public int Lvl { get; set; }
        public int Counter { get; set; }
    }

    public class Tasks
    {
        public int id { get; set; }
        public string Test { get; set; }
        public int Difficulty { get; set; }
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
    }

    public class SingleTestResults
    {
        public int UserId { get; set; }
        public int TestId { get; set; }
        public string Passed { get; set; }
        public int Attempts { get; set; }
        public string TimeUsed { get; set; }
        public string CodeLink { get; set; }
    }

    public class OXXGameDBContext : DbContext
    {
        // Konstruktør for opprettelse av context til bruk i tester
        public OXXGameDBContext(DbContextOptions<OXXGameDBContext> options)
            : base(options)
        { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<SingleTestResults>()
                .HasKey(k => new { k.UserId, k.TestId });
            modelBuilder.Entity<ResultsPerCategory>()
                .HasKey(k => new { k.UserId, k.Category });
        }

        public DbSet<Users> Users { get; set; }
        public DbSet<Categories> Categories { get; set; }
        public DbSet<ResultsPerCategory> ResultsPerCategory { get; set; }
        public DbSet<Tasks> Tasks { get; set; }
        public DbSet<SingleTestResults> SingleTestResults { get; set; }
        public DbSet<Results> Results { get; set; }
    }
}