using System.ComponentModel;
using StackExchange.Redis;
using TicketServer.Domain.Seats;

namespace TicketServer.Domain.Redis;

public static class RedisKeys
{
    public const string JobWaitingKey = "job:waiting:zset";
    public const string JobActiveKey = "job:active:zset";
    public const string SeatInventoryPrefix = "flight";
    public const string MasterFlightKeyPrefix = "all";
    public const string ReservedFlightKeyPrefix = "occupied";

    public static string GetMasterFlightKey(string flightNumber, DateTimeOffset date) 
        => $"{SeatInventoryPrefix}:{flightNumber}:{date}:{MasterFlightKeyPrefix}";

    public static string GetReservedFlightKey(string flightNumber, DateTimeOffset date) 
        => $"{SeatInventoryPrefix}:{flightNumber}:{date}:{ReservedFlightKeyPrefix}";

    public static string GetSeatField(string flightNumber, DateTimeOffset date, ClassType classType, string seatId)
        => $"{classType}:{seatId}";
}