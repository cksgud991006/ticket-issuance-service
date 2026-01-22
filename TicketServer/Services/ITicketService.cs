using TicketServer.Repositories;
using TicketServer.Domain.Tickets;

namespace TicketServer.Services;

public interface ITicketService
{
    public Task<TicketIssueResult> IssueAsync(
        int userId);
}