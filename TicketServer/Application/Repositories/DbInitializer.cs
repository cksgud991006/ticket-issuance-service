using Microsoft.EntityFrameworkCore;
using TicketServer.Domain.Seats;
using TicketServer.Infrastructure.Database;

namespace TicketServer.Application.Repositories;

public static class DbInitializer
{
    public static async Task Seed(IServiceProvider serviceProvider)
    {
        var scope = serviceProvider.CreateScope();
        var seatContext = scope.ServiceProvider.GetRequiredService<SeatContext>();
        
        seatContext.Database.Migrate();

        if (!seatContext.FlightInventories.Any())
        {
            seatContext.FlightInventories.AddRange(new FlightInventory
            {
                FlightNumber = "AA123",
                TotalSeats = 200,
                AvailableSeats = 200
            }, new FlightInventory
            {
                FlightNumber = "BB456",
                TotalSeats = 150,
                AvailableSeats = 150
            }, new FlightInventory
            {
                FlightNumber = "CC789",
                TotalSeats = 180,
                AvailableSeats = 180
            });
            await seatContext.SaveChangesAsync();
        }

        if (!seatContext.Seats.Any())
        {
            var seats = new List<Seat>();
            for (int i = 1; i <= 200; i++)
            {
                seats.Add(Seat.Create("AA123", DateTimeOffset.UtcNow.Date, ClassType.Economy, $"A{i}", SeatStatus.Available));
            }

            for (int i = 1; i <= 150; i++)
            {
                seats.Add(Seat.Create("BB456", DateTimeOffset.UtcNow.Date, ClassType.Economy, $"B{i}", SeatStatus.Available));
            }

            for (int i = 1; i <= 180; i++)
            {
                seats.Add(Seat.Create("CC789", DateTimeOffset.UtcNow.Date, ClassType.Economy, $"C{i}", SeatStatus.Available));
            }
            
            seatContext.Seats.AddRange(seats);
            await seatContext.SaveChangesAsync();
        }
    }
}