using TicketServer.Repositories;
using TicketServer.Domain.Tickets;

namespace TicketServer.Services;

public class TicketIssueService: ITicketService
{
    private readonly ITicketRepository _TicketRepository;

    public TicketIssueService(ITicketRepository TicketRepository)
    {
        _TicketRepository = TicketRepository;
    }

    public async Task<TicketIssueResult> IssueAsync(
        int userId)
    {

        if (await _TicketRepository.HasUserReceivedTicket(userId))
        {
            return TicketIssueResult.AlreadyIssued();
        }

        var Ticket = await _TicketRepository.TryIssueTicket(userId); 
        if (Ticket == null)
        {
            return TicketIssueResult.SoldOut();
        }

        return TicketIssueResult.Success(Ticket);
    }
}