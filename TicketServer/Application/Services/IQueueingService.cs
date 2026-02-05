using TicketServer.Domain.Status;

namespace TicketServer.Application.Services;

public interface IQueueingService
{
    public Task<QueueStatus> GetPositionInQueueAsync(
        Guid id);

    public Task EnqueueAsync(
        Guid id,
        DateTimeOffset RequestTime);
}