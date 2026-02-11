using TicketServer.Domain.Seats;

namespace TicketServer.Domain.Response;

public class SeatInventoryResponse
{
    public bool Success { get; private set; }
    public string FlightNumber { get; init; } = null!;

    public DateTimeOffset Date { get; private set; }

    public ClassType SeatClass { get; private set; }

    public string SeatNumber { get; private set; } = null!;
    public Guid BookingId { get; private set; } = Guid.Empty;
    public string Details { get; private set; } = null!;

    private SeatInventoryResponse(bool success, string flightNumber, DateTimeOffset date, ClassType seatClass,  string seatNumber, Guid bookingId, string details)
    {
        Success = success;
        FlightNumber = flightNumber;
        Date = date;
        SeatClass = seatClass;
        BookingId = bookingId;
        SeatNumber = seatNumber;
        Details = details;
    }

    public static SeatInventoryResponse CreateSuccessResponse(string flightNumber, DateTimeOffset date, ClassType seatClass, string seatNumber, Guid bookingId, string details) =>
        new SeatInventoryResponse(true, flightNumber, date, seatClass, seatNumber, bookingId, details);

    public static SeatInventoryResponse AlreadyReservedResponse(string flightNumber, DateTimeOffset date, ClassType seatClass, string seatNumber, string details) =>
        new SeatInventoryResponse(false, flightNumber, date, seatClass, seatNumber, Guid.Empty, details);

    public static SeatInventoryResponse CreateFailureResponse(string flightNumber, DateTimeOffset date, ClassType seatClass, string seatNumber, string details) =>
        new SeatInventoryResponse(false, flightNumber, date, seatClass, seatNumber, Guid.Empty, details);

    public static SeatInventoryResponse NoAvailableSeatsResponse(string flightNumber, DateTimeOffset date, ClassType seatClass, string seatNumber, string details) =>
        new SeatInventoryResponse(false, flightNumber, date, seatClass, seatNumber, Guid.Empty, details);
}