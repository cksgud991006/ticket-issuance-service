using System.ComponentModel.DataAnnotations;

namespace TicketServer.Api.Dto;

public record TicketWaitRequest(
    [Required] Guid Id,
    [Required] DateTimeOffset RequestTime,
    [Required] string IdempotencyKey
);