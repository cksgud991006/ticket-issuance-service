using NRedisStack;
using NRedisStack.RedisStackCommands;
using StackExchange.Redis;
using TicketServer.Schedule.Redis;

namespace TicketServer.Schedule;
public class JobRunner : IJobRunner
{
    private readonly IDatabase _redis;
    private readonly IJobWorker _jobWorker;
    private readonly int _loadCount = 1000;
    private int _startCount = 0;

    public JobRunner(ConnectionMultiplexer connectionMultiplexer, IJobWorker jobWorker)
    {
        _redis = connectionMultiplexer.GetDatabase();
        _jobWorker = jobWorker;
    }
    
    public async Task RunAsync()
    {

        // Implementation for running the job worker
        var jobs = await _redis.SortedSetRangeByRankAsync(
                RedisKeys.JobScheduleKey,
                _startCount,
                _loadCount,
                Order.Ascending);

        var ids = jobs.Select(j => Guid.Parse(j.ToString())).ToArray();

        await _jobWorker.ExecuteAsync(ids);
    }
}
