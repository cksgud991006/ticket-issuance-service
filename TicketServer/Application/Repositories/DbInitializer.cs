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
    }
}