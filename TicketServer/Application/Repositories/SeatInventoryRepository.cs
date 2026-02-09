using Microsoft.EntityFrameworkCore;
using TicketServer.Domain.Seats;
using TicketServer.Infrastructure.Database;

namespace TicketServer.Application.Repositories;

public class SeatInventoryRepository : ISeatInventoryRepository
{
    private readonly SeatContext _context;

    public SeatInventoryRepository(SeatContext context)
    {
        _context = context;
    }

    public Task<int> GetTotalSeats(string flightNumber)
    {
        return _context.FlightInventories
            .Where(f => f.FlightNumber == flightNumber)
            .Select(f => f.TotalSeats)
            .FirstOrDefaultAsync();
    }
    public Task<int> GetAvailableSeats(string flightNumber)
    {
        return _context.FlightInventories
            .Where(f => f.FlightNumber == flightNumber)
            .Select(f => f.AvailableSeats)
            .FirstOrDefaultAsync();
    }   
    
    public Task<Seat?> GetSeat(string flightNumber, DateTimeOffset date, ClassType classType, string seatNumber)
    {
        return _context.Seats.FirstOrDefaultAsync(s =>
            s.FlightNumber == flightNumber &&
            s.Date == date &&
            s.SeatClass == classType &&
            s.SeatNumber == seatNumber);
    }

    public Task<List<Seat>> GetSeats()
    {
        return _context.Seats.ToListAsync();
    }

    public Task UpdateTotalSeats(string flightNumber, int newTotalSeats)
    {
        _context.FlightInventories
            .Where(f => f.FlightNumber == flightNumber)
            .ExecuteUpdateAsync(row => row.SetProperty(f => f.TotalSeats, newTotalSeats));
        
        _context.SaveChangesAsync();

        return Task.CompletedTask;
    }
    public Task UpdateAvailableSeats(string flightNumber, int newAvailableSeats)
    {

        _context.FlightInventories
            .Where(f => f.FlightNumber == flightNumber)
            .ExecuteUpdateAsync(row => row.SetProperty(f => f.AvailableSeats, newAvailableSeats));

        _context.SaveChangesAsync();

        return Task.CompletedTask;
    }
    public Task UpdateSeatStatus(Seat seat, SeatStatus newStatus, string heldByUserId)
    {
        _context.Seats
            .Where(s => s.SeatId == seat.SeatId)
            .ExecuteUpdateAsync(row => row
                .SetProperty(s => s.Status, newStatus)
                .SetProperty(s => s.HeldByUserId, heldByUserId));

        _context.SaveChangesAsync();

        return Task.CompletedTask;
    }
}