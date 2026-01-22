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
        Guid id)
    {

        if (await _TicketRepository.HasUserReceivedTicket(id))
        {
            return TicketIssueResult.AlreadyIssued();
        }

        var ticket = await _TicketRepository.TryIssueTicket(id); 
        if (ticket == null)
        {
            return TicketIssueResult.SoldOut();
        }

        return TicketIssueResult.Success(ticket);
    }
}