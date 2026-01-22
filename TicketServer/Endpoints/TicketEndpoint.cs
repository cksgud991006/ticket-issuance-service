using TicketServer.Services;
using TicketServer.Domain.Tickets;
using TicketServer.Dto;

namespace TicketServer.Endpoints;

public static class TicketEndpoint
{
    public static void MapTicketEndPoints(this WebApplication app)
    {
        // GET 
        app.MapGet("/Tickets/users/{userId}", GetTicketByUserId);
        app.MapGet("/Tickets/{TicketId}", GetTicketByTicketId);
        
        // POST
        app.MapPost("/Tickets", IssueTicket);
    }

    private static async Task<IResult> GetTicketByUserId(
        int userId)
    {
        return Results.Ok();
    } 

    private static async Task<IResult> GetTicketByTicketId(
        int TicketId)
    {
        return Results.Ok();
    } 

    private static async Task<IResult> IssueTicket(
        TicketIssueRequest request,
        ITicketService service)
    {
        var result = await service.IssueAsync(request.UserId);
        return result switch
        {
            { IsSuccess: true } =>
                Results.Created(
                    $"/Tickets/{result.Ticket!.TicketId}",
                    new TicketIssueResponse(
                        result.Ticket!.UserId,
                        result.Ticket!.TicketId
                    )
                ),

            { FailureReason: TicketIssueFailureReason.AlreadyIssued } =>
                Results.Conflict("Ticket already issued"),

            { FailureReason: TicketIssueFailureReason.SoldOut } =>
                Results.Conflict("Ticket sold out"),

            _ =>
                Results.StatusCode(500)
        };
    }
}