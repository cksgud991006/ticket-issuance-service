using TicketServer.Application.Repositories;
using TicketServer.Domain.Seats;
using TicketServer.Domain.Redis;
using StackExchange.Redis;

namespace TicketServer.Application.Services;
public class SeatInventoryService : ISeatInventoryService
{
    private readonly ISeatInventoryRepository _seatInventoryRepository;
    private readonly ISeatInventoryLoader _seatInventoryLoader;
    private readonly IDatabase _redis;

    public SeatInventoryService(ISeatInventoryRepository seatInventoryRepository,
                              ISeatInventoryLoader seatInventoryLoader,
                              IConnectionMultiplexer connectionMultiplexer)
    {
        _seatInventoryRepository = seatInventoryRepository;
        _seatInventoryLoader = seatInventoryLoader;
        _redis = connectionMultiplexer.GetDatabase();
    }

    public async Task<bool> ReserveSeatAsync(string flightNumber, DateTimeOffset date, ClassType classType, string seatId, Guid id)
    {
        var result = await _seatInventoryLoader.TryLoadSeatInventoryAsync(flightNumber, date, classType, seatId);
        
        var flightKey = RedisKeys.GetFlightKey(flightNumber, date, classType);
        var seatField = RedisKeys.GetSeatField(flightNumber, date, classType, seatId) ;
        var val = await _redis.ScriptEvaluateAsync(
            RedisLuaScripts.LoadSeatInventoryScript.ExecutableScript,
            new RedisKey[] { flightKey },
            new RedisValue[] { seatField, id.ToString() });
        
        return (bool)val;
    }
}