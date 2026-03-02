using System.ComponentModel.DataAnnotations;
using TicketServer.Domain.Seats;

namespace TicketServer.Api.Dto;

public record PostResponse(
    [Required] bool Success
);

public record SeatInfo(
    [Required] string SeatClass,
    [Required] string SeatNumber
);

public record TicketSeatRequest(
    [Required] string FlightNumber,
    [Required] DateTimeOffset Date,
    [Required] ClassType SeatClass,
    [Required] string SeatNumber,
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

public record TicketSessionResponse(
    [Required] Guid Id,
    [Required] long TimeExpiry
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
