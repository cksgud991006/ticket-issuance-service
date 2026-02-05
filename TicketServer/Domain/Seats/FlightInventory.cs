namespace TicketServer.Domain.Seats;

public class FlightInventory
{
    // PK
    public int FlightId { get; set; }
    public string FlightNumber { get; set; } = null!;
    public int TotalSeats { get; set; }
    public int AvailableSeats { get; set; }
}