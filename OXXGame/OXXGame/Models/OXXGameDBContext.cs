using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace OXXGame.Models
{
    //Opprette klasser her, hvorav navnet representerer en tabellrad og variabler samsvarer med tabellkolonner. Eksempel:
    /* 
        public class Entitet
        {
            public int tall { get; set; }
            public string ord { get; set; }
        }
    */

    public class OXXGameDBContext : DbContext
    {
        public OXXGameDBContext(DbContextOptions<OXXGameDBContext> options)
            : base(options)
        { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {

        }

        // Opprette DbSet her hvorav navn samsvarer med tabellnavn i databasen. Eksempel:
        // public DbSet<Entitetsnavn> Tabellnavn { get; set; }

    }
}