using Microsoft.EntityFrameworkCore;
using TicketServer.Domain.Seats;

namespace TicketServer.Infrastructure.Database;
public class SeatContext: DbContext
{

    public DbSet<Seat> Seats { get; set; }
    public DbSet<FlightInventory> FlightInventories { get; set; }

    public SeatContext(DbContextOptions<SeatContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Seat>(entity =>
        {
            // Primary Key
            entity.HasIndex(c => c.SeatId)
                .IsUnique();

            entity.Property(c => c.FlightNumber)
                .IsRequired();

            entity.Property(c => c.Date)
                .IsRequired();

            entity.Property(c => c.SeatClass)
                .IsRequired();

            entity.Property(c => c.SeatNumber)
                .IsRequired();

            entity.Property(c => c.Status)
                .IsRequired();
        });

        modelBuilder.Entity<FlightInventory>(entity =>
        {
            // Primary Key
            entity.HasIndex(fi => fi.FlightId)
                .IsUnique();

            entity.Property(fi => fi.FlightNumber)
                .IsRequired();

            entity.Property(fi => fi.TotalSeats)
                .IsRequired()
                .HasDefaultValue(200);

            entity.Property(fi => fi.AvailableSeats)
                .IsRequired()
                .HasDefaultValue(0);
        });
    }
}