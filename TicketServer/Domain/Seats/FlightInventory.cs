using System.ComponentModel.DataAnnotations;

namespace TicketServer.Domain.Seats;

public class FlightInventory
{
    [Key]
    public int FlightId { get; private set; } // PK
    public string FlightNumber { get; set; } = null!;
    public int TotalSeats { get; set; }
    public int AvailableSeats { get; set; }
}