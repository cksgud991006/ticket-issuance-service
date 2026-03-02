using TicketServer.Application.Services;
using TicketServer.Domain.Tickets;
using TicketServer.Api.Dto;

namespace TicketServer.Endpoints;

public static class TicketEndpoint
{
    public static void MapTicketEndPoints(this WebApplication app)
    {
        // GET 
        app.MapGet("/queue/status/{id}", GetQueueStatus); // pulling
        app.MapGet("/active/status/{id}", GetActiveStatus); // pulling
        app.MapGet("/seats/{flightNumber}", GetAvailableFlightSeatCount);
        app.MapGet("/seats/total/{flightNumber}", GetTotalFlightSeats);
        app.MapGet("/seats/reserved/{flightNumber}", GetReservedFlightSeats);
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

    private static async Task<IResult> GetActiveStatus(
        Guid id,
        ISessionService service)
    {
        // polling active status from redis
        var sessionStatus = await service.GetSessionStatusAsync(id);

        return sessionStatus switch
        {
            { IsActive: true } =>
                Results.Ok(
                    new TicketSessionResponse(
                        id,
                        sessionStatus.TimeExpiry.ToUnixTimeSeconds()
                    )
                ),
            { IsActive: false } =>
                Results.Ok(
                    new TicketSessionResponse(
                        id,
                        -1
                    )
                ),
            _ =>
                Results.StatusCode(500)
        };
    }

    private static async Task<IResult> GetAvailableFlightSeatCount(
        string flightNumber,
        ISeatInventoryService service)
    {
        return Results.Ok(
            await service.GetAvailableSeatCountAsync(flightNumber)
        );
    }

    private static async Task<IResult> GetTotalFlightSeats(
        string flightNumber,
        ISeatInventoryService service)
    {
        string[] rawSeats = await service.GetTotalFlightSeatsAsync(flightNumber);

        var structuredSeats = rawSeats.Select(s => {
            var parts = s.Split(':');
            return new SeatInfo(parts[0], parts[1]);
        }).ToArray();

        return Results.Ok(
            structuredSeats
        );
    }

    private static async Task<IResult> GetReservedFlightSeats(
        string flightNumber,
        ISeatInventoryService service)
    {
        string[] rawSeats = await service.GetReservedFlightSeatsAsync(flightNumber);

        var structuredSeats = rawSeats.Select(s => {
            var parts = s.Split(':');
            return new SeatInfo(parts[0], parts[1]);
        }).ToArray();

        return Results.Ok(
            structuredSeats
        );
    }

    private static async Task<IResult> GetTicketStatus(
        Guid id,
        ISeatInventoryService service)
    {
        // TODO: implement ticket status retrieval
        return Results.Ok();
    }

    private static async Task<IResult> Enqueue(
        TicketWaitRequest request,
        IQueueingService service)
    {
        await service.EnqueueAsync(request.Id, request.RequestTime);
        return Results.Ok(
            new PostResponse(true)
        );
    }

    private static async Task<IResult> ReserveSeat(
        TicketSeatRequest request,
        ISeatInventoryService service)
    {
        var result = await service.ReserveSeatAsync(request.FlightNumber, request.SeatClass, request.SeatNumber, request.Id);
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