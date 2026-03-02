using TicketServer.Application.Repositories;
using TicketServer.Domain.Seats;
using TicketServer.Domain.Redis;
using StackExchange.Redis;
using TicketServer.Domain.Response;

namespace TicketServer.Application.Services;
public class SeatInventoryService : ISeatInventoryService
{
    private readonly ILogger<SeatInventoryService> _logger;
    private readonly ISeatInventoryRepository _seatInventoryRepository;
    private readonly IDatabase _redis;
    private readonly ISeatInventoryRepository _seatRepository;

    public SeatInventoryService(ILogger<SeatInventoryService> logger,
                              ISeatInventoryRepository seatInventoryRepository,
                              IConnectionMultiplexer connectionMultiplexer,
                              ISeatInventoryRepository seatRepository)
    {
        _logger = logger;           
        _seatInventoryRepository = seatInventoryRepository;
        _redis = connectionMultiplexer.GetDatabase();
        _seatRepository = seatRepository;
    }

    public async Task<long> GetAvailableSeatCountAsync(string flightNumber)
    {
        var availableCountKey = RedisKeys.GetFlightAvailableCountKey(flightNumber);
        var availableCount = await _redis.StringGetAsync(availableCountKey);
        return availableCount.HasValue ? long.Parse(availableCount!) : 0;
    }

    public async Task<string[]> GetTotalFlightSeatsAsync(string flightNumber)
    {
        var masterFlightKey = RedisKeys.GetMasterFlightKey(flightNumber);
        var seatValues = await _redis.SetMembersAsync(masterFlightKey);

        return seatValues.Select(s => s.ToString()).ToArray();
    }

    public async Task<string[]> GetReservedFlightSeatsAsync(string flightNumber)
    {
        var reservedFlightKey = RedisKeys.GetReservedFlightKey(flightNumber);
        var seatVales = await _redis.SetMembersAsync(reservedFlightKey);

        return seatVales.Select(s => s.ToString()).ToArray();
    }

    public async Task GetTicketInfoAsync(Guid id)
    {
        // TODO: implement ticket info retrieval
        
    }

    public async Task<SeatInventoryResponse> ReserveSeatAsync(string flightNumber, ClassType classType, string SeatNumber, Guid id)
    {
        
        var pos = await _redis.SortedSetRankAsync(RedisKeys.JobActiveKey, id.ToString());
        if (pos == null) {
            return SeatInventoryResponse.CreateFailureResponse(flightNumber, classType, SeatNumber, "User is not in the active queue.");
        }

        var availableCountKey = RedisKeys.GetFlightAvailableCountKey(flightNumber);
        var masterFlightKey = RedisKeys.GetMasterFlightKey(flightNumber);
        var reservedFlightKey = RedisKeys.GetReservedFlightKey(flightNumber);
        var seatField = RedisKeys.GetSeatField(classType, SeatNumber) ;

        _logger.LogInformation("Attempting to reserve seat. Flight: {FlightNumber}, Class: {ClassType}, Seat: {SeatNumber}, UserId: {UserId}", flightNumber, classType, SeatNumber, id);
        _logger.LogInformation("Redis Keys - AvailableCountKey: {AvailableCountKey}, MasterFlightKey: {MasterFlightKey}, ReservedFlightKey: {ReservedFlightKey}, SeatField: {SeatField}", availableCountKey, masterFlightKey, reservedFlightKey, seatField);
        var result = await _redis.ScriptEvaluateAsync(
            RedisLuaScripts.LoadSeatInventoryScript.ExecutableScript,
            [availableCountKey, masterFlightKey, reservedFlightKey],
            [seatField]);
        
        var data = (RedisResult[]) result!;

        int ResponseCode = (int)data[0];
        string details = (string)data[1]!;
        Guid bookingId = Guid.NewGuid();

        switch (ResponseCode)
        {
            case 0:
                return SeatInventoryResponse.AlreadyReservedResponse(flightNumber, classType, SeatNumber, details);
            case 1:
                var seat = await _seatInventoryRepository.GetSeat(flightNumber, classType, SeatNumber);
                await _seatRepository.UpdateSeatStatus(seat!, SeatStatus.Held, id.ToString());
                return SeatInventoryResponse.CreateSuccessResponse(flightNumber, classType, SeatNumber, bookingId, details);
            case -1:
                return SeatInventoryResponse.CreateFailureResponse(flightNumber, classType, SeatNumber, details);
            case -2:
                return SeatInventoryResponse.NoAvailableSeatsResponse(flightNumber, classType, SeatNumber, details);
            default:
                throw new InvalidOperationException("Unexpected Response code from Redis script.");
        }
    }
}