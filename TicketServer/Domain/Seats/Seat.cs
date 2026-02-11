using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace TicketServer.Domain.Seats;    

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum ClassType
{
    Economy,
    Business,
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
    [Key]
    public int SeatId { get; private set; } // PK
    public string FlightNumber { get; init; } = null!;

    public DateTimeOffset Date { get; set; }

    public ClassType SeatClass { get; init; }

    public string SeatNumber { get; init; } = null!;
    public SeatStatus Status { get; set; }
    public string? HeldByUserId { get; set; }

    public static Seat Create(string flightNumber, DateTimeOffset date, ClassType seatClass, string seatNumber, SeatStatus status)
    {
        return new Seat
        {
            FlightNumber = flightNumber,
            Date = date,
            SeatClass = seatClass,
            SeatNumber = seatNumber,
            Status = status
        };
    }
}