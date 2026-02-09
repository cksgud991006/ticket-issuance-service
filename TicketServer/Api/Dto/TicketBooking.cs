using System.ComponentModel.DataAnnotations;
using TicketServer.Domain.Seats;

namespace TicketServer.Api.Dto;


public record TicketSeatRequest(
    [Required] string FlightId,
    [Required] string FlightNumber,
    [Required] DateTimeOffset Date,
    [Required] ClassType SeatClass,
    [Required] string SeatId,
    [Required] Guid Id
);

public record TicketWaitRequest(
    [Required] Guid Id,
    [Required] DateTimeOffset RequestTime,
    [Required] string IdempotencyKey
);

public record TicketWaitResponse(
    [Required] Guid Id,
    [Required] int Position
);

public record TicketIssueResponse(
    [Required] Guid Id,
    [Required] string FlightNumber,
    [Required] string SeatNumber,
    [Required] DateTimeOffset Date,
    [Required] string Details,
    [Required] bool Success
);

public record TicketIssueFailure(
    [Required] string Details,
    [Required] bool Success
);
