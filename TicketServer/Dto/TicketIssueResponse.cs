using System.ComponentModel.DataAnnotations;

namespace TicketServer.Dto;

public record TicketIssueResponse(
    [Required] Guid Id,
    [Required] int TicketId
);