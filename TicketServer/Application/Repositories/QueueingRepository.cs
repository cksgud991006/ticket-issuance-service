using Microsoft.EntityFrameworkCore;
using TicketServer.Domain.Tickets;
using TicketServer.Infrastructure.Database;

namespace TicketServer.Application.Repositories;

public class QueueingRepository : IQueueingRepository
{
    private readonly TicketContext _context;

    public QueueingRepository(TicketContext context)
    {
        _context = context;
    }
}