using System.ComponentModel;
using StackExchange.Redis;
using TicketServer.Domain.Seats;

namespace TicketServer.Domain.Redis;

public static class RedisKeys
{
    public const string JobWaitingKey = "job:waiting:zset";
    public const string JobActiveKey = "job:active:zset";
    public const string SeatInventoryPrefix = "flight";

    public static string GetFlightKey(string flightNumber, DateTimeOffset date, ClassType classType) 
        => $"{SeatInventoryPrefix}:{flightNumber}:{date}";

    public static string GetSeatField(string flightNumber, DateTimeOffset date, ClassType classType, string seatId)
        => $"{classType}:{seatId}";
}