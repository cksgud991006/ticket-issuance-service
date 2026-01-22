using System.ComponentModel.DataAnnotations;

namespace TicketServer.Dto;

public record TicketIssueRequest(
    [Required] int UserId,
    [Required] string IdempotencyKey
);