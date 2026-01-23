using StackExchange.Redis;

namespace TicketServer.Schedule;

public interface IJobWorker
{
    Task ExecuteAsync(Guid[] ids);
}