using NRedisStack;
using NRedisStack.RedisStackCommands;
using StackExchange.Redis;
using TicketServer.Schedule.Redis;

namespace TicketServer.Schedule;

public class JobScheduler : IJobScheduler
{
    private readonly IDatabase _redis;
    public JobScheduler(ConnectionMultiplexer connectionMultiplexer)
    {
        _redis = connectionMultiplexer.GetDatabase();
    }
    public async Task ScheduleAsync(Guid id, DateTimeOffset scheduleTime)
    {
        await _redis.SortedSetAddAsync(RedisKeys.JobScheduleKey, id.ToString(), scheduleTime.ToUnixTimeSeconds());

    }
}