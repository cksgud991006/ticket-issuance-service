using TicketServer.Application.Repositories;
using TicketServer.Domain.Redis;
using TicketServer.Domain.Response;
using StackExchange.Redis;

namespace TicketServer.Application.Services;

public class SessionService: ISessionService
{
    private readonly IDatabase _redis;
    public SessionService(IConnectionMultiplexer connectionMultiplexer)
    {
        _redis = connectionMultiplexer.GetDatabase();   
    }

    public async Task<SessionStatusResponse> GetSessionStatusAsync(
        Guid id)
    {
        var key = RedisKeys.JobActiveKey;
        var score = await _redis.SortedSetScoreAsync(key, id.ToString());

        if (!score.HasValue)
        {
            return SessionStatusResponse.NotActive();
        }

        var timeExpiry = DateTimeOffset.FromUnixTimeSeconds((long)score.Value);

        return SessionStatusResponse.Active(timeExpiry);
    }
}