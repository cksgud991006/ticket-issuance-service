using TicketServer.Application.Services;
using TicketServer.Domain.Tickets;
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
        var queueResponse = await service.GetPositionInQueueAsync(id);

        return queueResponse switch
        {
            { IsInQueue: true } =>
                Results.Ok(
                    new TicketWaitResponse(
                        id,
                        queueResponse.Position
                    )
                ),
            { IsInQueue: false } =>
                Results.Ok(
                    new TicketWaitResponse(
                        id,
                        -1
                    )
                ),
            _ =>
                Results.StatusCode(500)
        };
    }

    private static async Task<IResult> GetTicketStatus(
        Guid id,
        ISeatInventoryService service)
    {
        // TODO: implement ticket status retrieval
        var ticketResponse = await service.GetTicketInfoAsync(id);
        return Results.Ok(ticketResponse);
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
        
        return result switch
        {
            { Success: true } =>
                Results.Created(
                    $"/tickets/{result.BookingId}",
                    new TicketIssueResponse(
                        result.BookingId,
                        result.FlightNumber,
                        result.SeatNumber,
                        result.Date,
                        result.Details,
                        result.Success
                    )
                ),

            { Success: false } =>
                Results.Conflict(
                    new TicketIssueFailure(
                        result.Details,
                        result.Success
                    )
                ),
            _ =>
                Results.StatusCode(500)
        };
    }
}