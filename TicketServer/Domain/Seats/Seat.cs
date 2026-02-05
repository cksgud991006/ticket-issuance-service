using System.Runtime.Serialization;

namespace TicketServer.Domain.Seats;    

public enum ClassType
{
    [EnumMember(Value = "Economy")]
    Economy,
    [EnumMember(Value = "Business")]
    Business,
    [EnumMember(Value = "First")]
    First
}

public enum SeatStatus
{
    [EnumMember(Value = "Available")]
    Available,
    [EnumMember(Value = "Held")]
    Held,
    [EnumMember(Value = "Sold")]
    Sold
}

public class Seat
{
    public int SeatId { get; private set; } // PK
    public string FlightNumber { get; init; } = null!;

    public DateTimeOffset Date { get; private set; }

    public ClassType SeatClass { get; private set; }

    public string SeatNumber { get; private set; } = null!;
    public SeatStatus Status { get; private set; }
    public string HeldByUserId { get; private set; } = null!;

    public static Seat Create(int seatId, string flightNumber, DateTimeOffset date, ClassType seatClass, string seatNumber, SeatStatus status)
    {
        return new Seat
        {
            SeatId = seatId,
            FlightNumber = flightNumber,
            Date = date,
            SeatClass = seatClass,
            SeatNumber = seatNumber,
            Status = status
        };
    }
}