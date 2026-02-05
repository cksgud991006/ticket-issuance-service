using NRedisStack;
using NRedisStack.RedisStackCommands;
using StackExchange.Redis;
using TicketServer.Domain.Redis;

namespace TicketServer.Schedule;

public class JobScheduler : IJobScheduler
{
    private readonly IDatabase _redis;
    public JobScheduler(ConnectionMultiplexer connectionMultiplexer)
    {
        _redis = connectionMultiplexer.GetDatabase();
    }

    public async Task<int> GetWaitingPositionAsync(Guid id)
    {
        var rank = await _redis.SortedSetRankAsync(RedisKeys.JobWaitingKey, id.ToString());
        if (rank.HasValue)
        {
            return (int)rank.Value + 1; // Convert zero-based rank to one-based position
        }
        
        return -1; // Not found
    }

    public async Task ScheduleAsync(Guid id, DateTimeOffset scheduleTime)
    {
        await _redis.SortedSetAddAsync(RedisKeys.JobWaitingKey, id.ToString(), scheduleTime.ToUnixTimeSeconds());
    }
}