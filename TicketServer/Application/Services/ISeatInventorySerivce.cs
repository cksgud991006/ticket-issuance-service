using TicketServer.Domain.Seats;
using TicketServer.Domain.Response;

namespace TicketServer.Application.Services;

public interface ISeatInventoryService
{

    public Task<SeatInventoryResponse> GetTicketInfoAsync(
        Guid id);

    public Task<SeatInventoryResponse> ReserveSeatAsync(
        string flightNumber,
        DateTimeOffset date,
        ClassType classType,
        string seatId,
        Guid id);
}