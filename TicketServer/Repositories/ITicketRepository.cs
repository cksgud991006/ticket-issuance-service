using TicketServer.Domain.Tickets;

namespace TicketServer.Repositories;

public interface ITicketRepository
{
    public Task<bool> HasUserReceivedTicket(Guid id);

    public Task<Ticket?> TryIssueTicket(Guid id);
}