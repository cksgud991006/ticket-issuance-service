using Microsoft.EntityFrameworkCore;
using TicketServer.Domain.Tickets;
using System.Data.SqlTypes;

namespace TicketServer.Repositories;

public class TicketRepository : ITicketRepository
{
    private readonly AppDbContext _context;

    public TicketRepository(AppDbContext context)
    {
        _context = context;
    }

    public Task<bool> HasUserReceivedTicket(int userId)
    {
        return _context.Tickets
            .AnyAsync(c => c.UserId == userId);
    }


    public async Task<Ticket?> TryIssueTicket(int userId)
    {
        var ticket = Ticket.Create(userId);
        _context.Tickets.Add(ticket);

        var affected = await _context.TicketPolicy
            .Where(p => p.IssuedTickets < p.TotalQuantity)
            .ExecuteUpdateAsync(p =>
                p.SetProperty(
                    x => x.IssuedTickets,
                    x => x.IssuedTickets + 1
                )
            );

        if (affected == 0)
            return null;
        
        await _context.SaveChangesAsync();
        return ticket;
    }
}