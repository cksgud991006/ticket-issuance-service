using TicketServer.Domain.Seats;

namespace TicketServer.Application.Repositories;

public interface ISeatInventoryRepository
{
    // data loading
    public Task<int> GetTotalSeats(string flightNumber);
    public Task<int> GetAvailableSeats(string flightNumber);
    public Task<Seat?> GetSeat(string flightNumber, ClassType classType, string seatNumber);
    public Task<List<Seat>> GetSeats();

    // data updating
    public Task UpdateTotalSeats(string flightNumber, int newTotalSeats);
    public Task UpdateAvailableSeats(string flightNumber, int newAvailableSeats);
    public Task UpdateSeatStatus(Seat seat, SeatStatus newStatus, string heldByUserId);
}