using Microsoft.EntityFrameworkCore;
using TicketServer.Domain.Tickets;
using TicketServer.Repositories;

namespace TicketServer.Tests.Fakes;

public class FakeTicketRepository : ITicketRepository
{
    private readonly List<Ticket> _Tickets = new();

    private int _totalQuantity;
    private int _issuedTickets;

    public FakeTicketRepository()
    {
        _Tickets = new List<Ticket>();
        _totalQuantity = 1;
        _issuedTickets = 0;
    }

    public FakeTicketRepository(int totalQuantity)
    {
        _Tickets = new List<Ticket>();
        _totalQuantity = totalQuantity;
        _issuedTickets = 0;
    }

    public Task<bool> HasUserReceivedTicket(Guid id)
    {
        return Task.FromResult(
            _Tickets.Any(c => c.Id == id)
        );
    }


    public Task<bool> TryIssueTicket(Guid id)
    {

        // mirrors DB constraint: inventory exhausted
        if (_issuedTickets >= _totalQuantity)
            return Task.FromResult(false);

        var ticket = Ticket.Create(id);
        _Tickets.Add(ticket);
        _issuedTickets++;
        
        return Task.FromResult(true);
    }
}