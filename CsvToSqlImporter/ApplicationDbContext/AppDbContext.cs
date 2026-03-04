using CsvToSqlImporter.Entites;
using Microsoft.EntityFrameworkCore;


namespace CsvToSqlImporter.ApplicationDbContext
{
    public class AppDbContext : DbContext
    {

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
           options.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=AppTripsDb;Trusted_Connection=True;TrustServerCertificate=True;");
           options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<AppTrip>()
                .HasIndex(t => t.PULocationID);

            modelBuilder.Entity<AppTrip>()
                .HasIndex(t => t.TripDistance);

            modelBuilder.Entity<AppTrip>()
                .Property(t => t.FareAmount).HasPrecision(18, 2);
            modelBuilder.Entity<AppTrip>()
                .Property(t => t.TipAmount).HasPrecision(18, 2);
        }

        public DbSet<AppTrip> AppTrips { get; set; }

    }
    
}
