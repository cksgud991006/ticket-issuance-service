using TicketServer.Domain.Tickets;

namespace TicketServer.Repositories;

public interface ITicketRepository
{
    public Task<bool> HasUserReceivedTicket(int userId);

    public Task<Ticket?> TryIssueTicket(int userId);
}