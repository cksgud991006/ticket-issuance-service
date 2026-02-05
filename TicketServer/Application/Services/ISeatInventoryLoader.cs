using TicketServer.Domain.Seats;

namespace TicketServer.Application.Services;

public interface ISeatInventoryLoader
{
    public Task<int> TryLoadSeatInventoryAsync(
        string flightNumber,
        DateTimeOffset date,
        ClassType classType,
        string seatId);
}