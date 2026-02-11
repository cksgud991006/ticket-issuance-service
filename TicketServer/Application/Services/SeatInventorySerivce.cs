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

    public async Task<SeatInventoryResponse> GetTicketInfoAsync(Guid id)
    {
        // TODO: implement ticket info retrieval
        return SeatInventoryResponse.CreateSuccessResponse("", DateTimeOffset.Now, ClassType.Economy, "", Guid.NewGuid(), "Placeholder");
    }

    public async Task<SeatInventoryResponse> ReserveSeatAsync(string flightNumber, DateTimeOffset date, ClassType classType, string seatId, Guid id)
    {
        
        var pos = await _redis.SortedSetRankAsync(RedisKeys.JobActiveKey, id.ToString());
        if (pos == null) {
            return SeatInventoryResponse.CreateFailureResponse(flightNumber, date, classType, seatId, "User is not in the active queue.");
        }

        var availableCountKey = RedisKeys.GetFlightAvailableCountKey(flightNumber, date);
        var masterFlightKey = RedisKeys.GetMasterFlightKey(flightNumber, date);
        var reservedFlightKey = RedisKeys.GetReservedFlightKey(flightNumber, date);
        var seatField = RedisKeys.GetSeatField(classType, seatId) ;

        _logger.LogInformation("Attempting to reserve seat. Flight: {FlightNumber}, Date: {Date}, Class: {ClassType}, Seat: {SeatId}, UserId: {UserId}", flightNumber, date, classType, seatId, id);
        _logger.LogInformation("Redis Keys - AvailableCountKey: {AvailableCountKey}, MasterFlightKey: {MasterFlightKey}, ReservedFlightKey: {ReservedFlightKey}, SeatField: {SeatField}", availableCountKey, masterFlightKey, reservedFlightKey, seatField);
        var result = await _redis.ScriptEvaluateAsync(
            RedisLuaScripts.LoadSeatInventoryScript.ExecutableScript,
            [availableCountKey, masterFlightKey, reservedFlightKey],
            [seatField, id.ToString()]);
        
        var data = (RedisResult[]) result!;

        int ResponseCode = (int)data[0];
        string details = (string)data[1]!;
        Guid bookingId = Guid.NewGuid();

        switch (ResponseCode)
        {
            case 0:
                return SeatInventoryResponse.AlreadyReservedResponse(flightNumber, date, classType, seatId, details);
            case 1:
                var seat = await _seatInventoryRepository.GetSeat(flightNumber, date, classType, seatId);
                await _seatRepository.UpdateSeatStatus(seat!, SeatStatus.Held, id.ToString());
                return SeatInventoryResponse.CreateSuccessResponse(flightNumber, date, classType, seatId, bookingId, details);
            case -1:
                return SeatInventoryResponse.CreateFailureResponse(flightNumber, date, classType, seatId, details);
            case -2:
                return SeatInventoryResponse.NoAvailableSeatsResponse(flightNumber, date, classType, seatId, details);
            default:
                throw new InvalidOperationException("Unexpected Response code from Redis script.");
        }
    }
}