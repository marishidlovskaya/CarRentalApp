using Microsoft.EntityFrameworkCore;
using System.Configuration;
using CarRental.Domain.Core.Models.Users;
using CarRental.Domain.Core.Models.Bookings;
using CarRental.Domain.Core.Models.Cars;

namespace CarRental.Infrastructure.Data
{
    public class ApplicationDbContext : DbContext
    {

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
           .HasMany(e => e.Cars)
           .WithMany(e => e.Users)
           .UsingEntity<Booking>(
            l => l.HasOne<Car>(e => e.Car).WithMany(e => e.Bookings),
            r => r.HasOne<User>(e => e.User).WithMany(e => e.Bookings));
        }
        public DbSet<User> Users { get; set; }
        public DbSet<Car> Cars { get; set; }
        public DbSet<Booking> Bookings { get; set; }
    }
}
