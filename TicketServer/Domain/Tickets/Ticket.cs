namespace TicketServer.Domain.Tickets;

public class Ticket
{
    public Guid Id { get; private set; }
    
    public int TicketId { get; private set; }
    
    private Ticket()
    {
        Id = Guid.NewGuid();
        TicketId = -1;
    }

    public static Ticket Create(Guid id)
    {
        return new Ticket
        {
            Id = id
        };
    }
}