using System.ComponentModel.DataAnnotations;

namespace TicketServer.Api.Dto;

public record TicketWaitResponse(
    [Required] Guid Id,
    [Required] int TicketId
);