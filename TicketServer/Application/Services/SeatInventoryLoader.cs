using StackExchange.Redis;
using TicketServer.Domain.Redis;
using TicketServer.Application.Repositories;

namespace TicketServer.Application.Services;

public class SeatInventoryLoader(IServiceScopeFactory scopeFactory, ILogger<SeatInventoryLoader> logger): IHostedService
{
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        using (IServiceScope scope = scopeFactory.CreateScope())
        {
            var seatInventoryRepository = scope.ServiceProvider.GetRequiredService<ISeatInventoryRepository>();
            var multiplexer = scope.ServiceProvider.GetRequiredService<IConnectionMultiplexer>();
            var redis = multiplexer.GetDatabase();
            var seats = await seatInventoryRepository.GetSeats();

            foreach (var seat in seats)
            {
                var flightAvailableCountKey = RedisKeys.GetFlightAvailableCountKey(seat.FlightNumber);
                if (!await redis.KeyExistsAsync(flightAvailableCountKey).WaitAsync(cancellationToken))
                {
                    var availableSeat = await seatInventoryRepository.GetAvailableSeats(seat.FlightNumber);
                    await redis.StringSetAsync(flightAvailableCountKey, availableSeat).WaitAsync(cancellationToken);
                }

                var flightKey = RedisKeys.GetMasterFlightKey(seat.FlightNumber);
                var seatField = RedisKeys.GetSeatField(seat.SeatClass, seat.SeatNumber);
                logger.LogInformation("Adding seat {SeatField} to Redis set for flight {FlightKey}", seatField, flightKey);
                await redis.SetAddAsync(flightKey, seatField).WaitAsync(cancellationToken);
            }
        }
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
        // Implementation for stopping the inventory loader if needed
    }
}