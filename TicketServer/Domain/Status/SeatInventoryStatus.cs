using TicketServer.Domain.Seats

namespace TicketServer.Domain.Status;

public class SeatInventoryStatus
{
    public bool Success { get; private set; }
    public int SeatId { get; private set; } // PK
    public string FlightNumber { get; init; } = null!;

    public DateTimeOffset Date { get; private set; }

    public ClassType SeatClass { get; private set; }

    public string SeatNumber { get; private set; } = null!;

    private SeatInventoryStatus(bool success, int seatId, string flightNumber, DateTimeOffset date, ClassType seatClass,  string seatNumber)
    {
        Success = success;
        SeatId = seatId;
        FlightNumber = flightNumber;
        Date = date;
        SeatClass = seatClass;
        SeatNumber = seatNumber;
    }
}