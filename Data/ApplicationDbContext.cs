using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Library_PenaltyCalculation.Models;
using Microsoft.AspNetCore.Identity;

namespace Library_PenaltyCalculation.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }



        public DbSet<Library_PenaltyCalculation.Models.Country> Country { get; set; }
        public DbSet<Library_PenaltyCalculation.Models.ReturnBook> ReturnBook { get; set; }

        public DbSet<Library_PenaltyCalculation.Models.Holidays> Holidays { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ReturnBook>()
        .HasOne(b => b.Country)
        .WithMany(a => a.ReturnBook)
        .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Holidays>()
        .HasOne(b => b.Country)
        .WithMany(a => a.Holidays)
        .OnDelete(DeleteBehavior.Restrict);

        }




    }
}
