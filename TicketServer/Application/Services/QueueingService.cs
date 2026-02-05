using TicketServer.Application.Repositories;
using TicketServer.Domain.Status;
using TicketServer.Schedule;

namespace TicketServer.Application.Services;

public class QueueingService: IQueueingService
{
    private readonly IQueueingRepository _queueingRepository;
    private readonly IJobScheduler _jobScheduler;
    public QueueingService(IQueueingRepository queueingRepository,
                        IJobScheduler jobScheduler)
    {
        _queueingRepository = queueingRepository;
        _jobScheduler = jobScheduler;   
    }

    public async Task<QueueStatus> GetPositionInQueueAsync(
        Guid id)
    {
        var position = await _jobScheduler.GetWaitingPositionAsync(id);
        return QueueStatus.InQueue(position);
    }

    public async Task EnqueueAsync(
        Guid id,
        DateTimeOffset RequestTime)
    {

        await _jobScheduler.ScheduleAsync(id, RequestTime);
        
    }
}