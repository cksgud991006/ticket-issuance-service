using TicketServer.Domain.Seats;
using TicketServer.Domain.Response;

namespace TicketServer.Application.Services;

public interface ISeatInventoryService
{

    public Task<long> GetAvailableSeatCountAsync(string flightNumber);

    public Task<string[]> GetTotalFlightSeatsAsync(string flightNumber);
    
    public Task<string[]> GetReservedFlightSeatsAsync(string flightNumber);

    public Task GetTicketInfoAsync(
        Guid id);

    public Task<SeatInventoryResponse> ReserveSeatAsync(
        string flightNumber,
        ClassType classType,
        string seatId,
        Guid id);
}