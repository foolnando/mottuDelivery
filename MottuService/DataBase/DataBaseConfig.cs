using Microsoft.EntityFrameworkCore;

namespace MottuService.DataBase;

public class MottuDataBaseContext : DbContext
{
    public static readonly ILoggerFactory Logger
        = LoggerFactory.Create(builder => { builder.AddConsole(); });
    public DbSet<Vehicle> Vehicles { get; set; }
    public DbSet<Driver> Drivers { get; set; }
    public DbSet<RentDriverVehicle> Rents { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<OrderNotification> Notifications { get; set; }




    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<RentDriverVehicle>()
        .HasKey(r => r.Id); 

        modelBuilder.Entity<RentDriverVehicle>()
            .HasOne(r => r.Driver)
            .WithMany(d => d.Rents)
            .HasForeignKey(r => r.DriverId);

        modelBuilder.Entity<RentDriverVehicle>()
            .HasOne(r => r.Vehicle) 
            .WithMany(v => v.Rents)
            .HasForeignKey(r => r.VehicleId);


        modelBuilder.Entity<OrderNotification>()
            .HasKey(n => new { n.Id });

        modelBuilder.Entity<OrderNotification>()
            .HasOne(n => n.Driver)
            .WithMany(d => d.Notifications)
            .HasForeignKey(n => n.DriverId);

        modelBuilder.Entity<OrderNotification>()
            .HasOne(n => n.Order)
            .WithMany(o => o.Notifications)
            .HasForeignKey(n => n.OrderId);

        modelBuilder.Entity<Order>()
            .HasKey(o => new { o.Id });

        modelBuilder.Entity<Order>()
            .HasOne(o => o.Rent)
            .WithMany()
            .HasForeignKey(n => n.RentId)
            .IsRequired(false);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql("Host=localhost;Database=mottu;Username=postgres;Password=postgres");
        optionsBuilder.LogTo(Console.WriteLine, LogLevel.Information);
        optionsBuilder
            .UseLoggerFactory(Logger);
        optionsBuilder.EnableSensitiveDataLogging();
    }

}