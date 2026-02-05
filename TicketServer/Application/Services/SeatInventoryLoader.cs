using TicketServer.Domain.Seats;
using NRedisStack;
using NRedisStack.RedisStackCommands;
using StackExchange.Redis;
using TicketServer.Domain.Redis;
using TicketServer.Application.Repositories;

namespace TicketServer.Application.Services;

public class SeatInventoryLoader : ISeatInventoryLoader
{
    private readonly ISeatInventoryRepository _seatInventoryRepository;
    private readonly IDatabase _redis;
        
    public SeatInventoryLoader(ISeatInventoryRepository seatInventoryRepository, 
                            IConnectionMultiplexer connectionMultiplexer)
    {
        _seatInventoryRepository = seatInventoryRepository;
        _redis = connectionMultiplexer.GetDatabase();
    }

    public async Task<int> TryLoadSeatInventoryAsync(
        string flightNumber,
        DateTimeOffset date,
        string classType,
        string seatId)
    {
        var flightKey = RedisKeys.GetFlightKey(flightNumber, date, classType);
        if (await _redis.KeyExistsAsync(flightKey))
        {
            return 0;
        }

        var remaingSeat = await _seatInventoryRepository.GetTotalSeats(flightNumber);
        await _redis.StringSetAsync(flightKey, remaingSeat);

        /*
        var seatKey = RedisKeys.GetSeatField(flightNumber, date, classType, seatId);
        if (await _redis.KeyExistsAsync(seatKey))
        {
            return 0;
        }
        */
        return 1;
    }
}