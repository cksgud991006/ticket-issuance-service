using Microsoft.EntityFrameworkCore;
using TicketServer.Domain.Tickets;
using System.Reflection.Metadata;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;
using System.Xml;
using System.Net.Http.Headers;

namespace TicketServer.Repositories;
public class AppDbContext: DbContext
{

    public DbSet<Ticket> Tickets { get; set; }
    public DbSet<TicketPolicy> TicketPolicy { get; set; }

    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Ticket>(entity =>
        {
            entity.HasKey(c => c.TicketId);

            entity.Property(c => c.Id)
                .IsRequired();
        });

        modelBuilder.Entity<TicketPolicy>(entity =>
        {
            entity.HasIndex(cp => cp.Id)
                .IsUnique();

            entity.Property(cp => cp.TotalQuantity)
                .IsRequired()
                .HasDefaultValue(1000);

            entity.Property(cp => cp.IssuedTickets)
                .IsRequired()
                .HasDefaultValue(0);
        });
    }
}