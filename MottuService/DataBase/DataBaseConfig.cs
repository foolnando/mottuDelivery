using Microsoft.EntityFrameworkCore;

namespace MottuService.DataBase;

public class MottuDataBaseContext : DbContext
{
    public DbSet<Vehicle> Vehicles { get; set; }
    public DbSet<Driver> Drivers { get; set; }
    public DbSet<RentDriverVehicle> Rents { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<RentDriverVehicle>()
            .HasKey(r => new { r.DriverId, r.VehicleId });

        modelBuilder.Entity<RentDriverVehicle>()
            .HasOne(r => r.Driver)
            .WithMany(d => d.Rents)
            .HasForeignKey(r => r.DriverId);

        modelBuilder.Entity<RentDriverVehicle>()
            .HasOne(r => r.Vehicle)
            .WithMany(v => v.Rents)
            .HasForeignKey(r => r.VehicleId);
    }
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql("Host=localhost;Database=mottu;Username=postgres;Password=postgres");
        optionsBuilder.LogTo(Console.WriteLine, LogLevel.Information);
        optionsBuilder.EnableSensitiveDataLogging();
    }

}