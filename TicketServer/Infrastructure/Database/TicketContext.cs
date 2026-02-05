using Microsoft.EntityFrameworkCore;
using TicketServer.Domain.Tickets;

namespace TicketServer.Infrastructure.Database;
public class TicketContext: DbContext
{

    public DbSet<Ticket> Tickets { get; set; }

    public TicketContext(DbContextOptions<TicketContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
    }
}