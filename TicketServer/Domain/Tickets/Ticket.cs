namespace TicketServer.Domain.Tickets;

public class Ticket
{
    public int UserId { get; set; }
    public int TicketId { get; private set; }
    
    private Ticket() { }

    public static Ticket Create(int userId)
    {
        return new Ticket
        {
            UserId = userId
        };
    }
}