using System.ComponentModel.DataAnnotations;

namespace TicketServer.Dto;

public record TicketIssueRequest(
    [Required] Guid Id,
    [Required] DateTimeOffset RequestTime,
    [Required] string IdempotencyKey
);