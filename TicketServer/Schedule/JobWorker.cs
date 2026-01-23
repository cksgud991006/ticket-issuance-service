using NRedisStack;
using NRedisStack.RedisStackCommands;
using StackExchange.Redis;

namespace TicketServer.Schedule;

public class JobWorker : IJobWorker
{
    private readonly IDatabase _redis;

    public JobWorker(ConnectionMultiplexer connectionMultiplexer)
    {
        _redis = connectionMultiplexer.GetDatabase();
    }

    
    public async Task ExecuteAsync(Guid[] ids)
    {
        // redirect request to ticketing api
        foreach (var id in ids)
        {
            Console.WriteLine($"Executing job with ID: {id}");
        }
    }
}