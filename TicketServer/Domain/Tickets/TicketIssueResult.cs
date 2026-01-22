using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using TicketServer.Dto;

namespace TicketServer.Domain.Tickets;

public enum TicketIssueFailureReason
{
    AlreadyIssued,
    SoldOut,
    InvalidId
}

public class TicketIssueResult
{
    public bool IsSuccess { get; }
    public TicketIssueFailureReason? FailureReason { get; }
    public Ticket? Ticket { get; }
    private TicketIssueResult(bool success, TicketIssueFailureReason? failureReason, Ticket? ticket)
    {
        IsSuccess = success;
        FailureReason = failureReason;
        Ticket = ticket;
    }

    public static TicketIssueResult Success(Ticket ticket) 
        => new TicketIssueResult(true, null, ticket);
    public static TicketIssueResult AlreadyIssued()
        => new TicketIssueResult(false, TicketIssueFailureReason.AlreadyIssued, null);
    public static TicketIssueResult SoldOut() 
        => new TicketIssueResult(false, TicketIssueFailureReason.SoldOut, null);
    public static TicketIssueResult InvalidId()
        => new TicketIssueResult(false, TicketIssueFailureReason.InvalidId, null);
    
}