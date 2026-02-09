using StackExchange.Redis;
using TicketServer.Domain.Redis;
using TicketServer.Application.Repositories;

namespace TicketServer.Application.Services;

public static class SeatInventoryLoader
{
    public static async Task<int> LoadSeatInventoryAsync(IServiceProvider serviceProvider)
    {
        var scope = serviceProvider.CreateScope();
        var seatInventoryRepository = scope.ServiceProvider.GetRequiredService<ISeatInventoryRepository>();
        var multiplexer = scope.ServiceProvider.GetRequiredService<IConnectionMultiplexer>();
        var redis = multiplexer.GetDatabase();
        var seats = await seatInventoryRepository.GetSeats();

        foreach (var seat in seats)
        {
            var flightKey = RedisKeys.GetMasterFlightKey(seat.FlightNumber, seat.Date);
            if (!await redis.KeyExistsAsync(flightKey))
            {
                var availableSeat = await seatInventoryRepository.GetAvailableSeats(seat.FlightNumber);
                await redis.StringSetAsync(flightKey, availableSeat);
            }

            var seatField = RedisKeys.GetSeatField(seat.FlightNumber, seat.Date, seat.SeatClass, seat.SeatNumber);

            await redis.SetAddAsync(flightKey, seatField);
        }

        return 1;
    }
}