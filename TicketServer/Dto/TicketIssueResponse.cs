using System.ComponentModel.DataAnnotations;

namespace TicketServer.Dto;

public record TicketIssueResponse(
    [Required] int UserId,
    [Required] int TicketId
);