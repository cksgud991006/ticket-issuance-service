using Microsoft.EntityFrameworkCore;
using TicketServer.Domain.Tickets;

namespace TicketServer.Repositories;

public static class DbInitializer
{
    public static void Seed(IServiceProvider serviceProvider)
    {
        var scope = serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        
        context.Database.Migrate();

        if (!context.TicketPolicy.Any())
        {
            context.TicketPolicy.Add(TicketPolicy.CreateDefault());
            context.SaveChanges();
        }

        else
        {
            context.TicketPolicy.ExecuteUpdate(p =>
                p.SetProperty(
                    x => x.IssuedTickets,
                    x => context.Tickets.Count()
                )
            );
        }
    }
}