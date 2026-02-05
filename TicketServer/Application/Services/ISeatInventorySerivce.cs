using TicketServer.Domain.Seats;

namespace TicketServer.Application.Services;

public interface ISeatInventoryService
{
    public Task<bool> ReserveSeatAsync(
        string flightNumber,
        DateTimeOffset date,
        ClassType classType,
        string seatId,
        Guid id);
}