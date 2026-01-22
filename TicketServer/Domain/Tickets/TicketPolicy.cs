using System.Data.Common;
using System.Diagnostics.Contracts;
using System.Reflection.Metadata.Ecma335;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;

namespace TicketServer.Domain.Tickets;

public class TicketPolicy
{
    public int Id { get; set; }
    public int TotalQuantity { get; set;}
    public int IssuedTickets { get; set;}

    public static TicketPolicy CreateDefault()
        => new TicketPolicy 
        {
            TotalQuantity = 1000000,
            IssuedTickets = 0
        };
}
