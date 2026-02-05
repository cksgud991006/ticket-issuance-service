using TicketServer.Domain.Seats;

namespace TicketServer.Domain.Tickets;

public class Ticket
{
    public int Id { get; init; } // PK
    
    public static Ticket Create(int id, string flightId, DateTimeOffset date, ClassType classType)
    {
        return new Ticket
        {
            
        };
    }
}