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