using TicketServer.Application.Services;
using TicketServer.Domain.Tickets;
using TicketServer.Dto;
using TicketServer.Api.Dto;

namespace TicketServer.Endpoints;

public static class TicketEndpoint
{
    public static void MapTicketEndPoints(this WebApplication app)
    {
        // GET 
        app.MapGet("/queue/{id}/status", GetQueueStatus); // pulling
        app.MapGet("/tickets/{ticketId}", () => GetTicketStatus); // placeholder
        
        // POST
        app.MapPost("/queue", Enqueue); 
        app.MapPost("/seat", ReserveSeat);

    }

    private static async Task<IResult> GetQueueStatus(
        Guid id,
        IQueueingService service)
    {
        // polling queue status from redis

        var position = await service.GetPositionInQueueAsync(id);

        return Results.Ok();
    }

    private static async Task<IResult> GetTicketStatus()
    {
        // placeholder
        return Results.Ok();
    }

    private static async Task<IResult> Enqueue(
        TicketWaitRequest request,
        IQueueingService service)
    {
        await service.EnqueueAsync(request.Id, request.RequestTime);
        return Results.Ok();
    }

    private static async Task<IResult> ReserveSeat(
        TicketSeatRequest request,
        ISeatInventoryService service)
    {
        var result = await service.ReserveSeatAsync(request.FlightNumber, request.Date, request.SeatClass, request.SeatId, request.Id);
        // TODO: process return to proper result
        /*
        return result switch
        {
            { IsSuccess: true } =>
                Results.Created(
                    $"/Tickets/{result.Ticket!.TicketId}",
                    new TicketIssueResponse(
                        result.Ticket!.Id,
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
        */
        return Results.Ok(result);
    }
}