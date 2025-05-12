using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using WebAppPart1.Models;

namespace WebAppPart1.Context
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> contextOptions)
            : base(contextOptions)
        {

        }

        // Code - Approach 
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<Venue> Venues { get; set; }
        public DbSet<Event> Events { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Override default table names
            modelBuilder.Entity<Booking>().ToTable("Booking");
            modelBuilder.Entity<Event>().ToTable("Event");
            modelBuilder.Entity<Venue>().ToTable("Venue");

            base.OnModelCreating(modelBuilder);
        }
    }
}