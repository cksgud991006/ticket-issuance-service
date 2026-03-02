using TicketServer.Domain.Response;

namespace TicketServer.Application.Services;

public interface ISessionService
{
    public Task<SessionStatusResponse> GetSessionStatusAsync(
        Guid id);
}