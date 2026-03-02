using System.ComponentModel;
using System.Globalization;
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

    // Key to store the count of available seats for a flight, e.g., "flight:available_count:AA123:2024-07-01T00:00:00Z": 150
    public static string GetFlightAvailableCountKey(string flightNumber) 
        => $"{SeatInventoryPrefix}:available_count:{flightNumber}";

    // Key to store all available seats for a flight, e.g., "flight:all:AA123:2024-07-01T00:00:00Z": "ECONOMY:C167"
    public static string GetMasterFlightKey(string flightNumber) 
        => $"{SeatInventoryPrefix}:{MasterFlightKeyPrefix}:{flightNumber}";


    // Key to store reserved seats for a flight, e.g., "flight:occupied:AA123:2024-07-01T00:00:00Z": "ECONOMY:C167"
    public static string GetReservedFlightKey(string flightNumber) 
        => $"{SeatInventoryPrefix}:{ReservedFlightKeyPrefix}:{flightNumber}";

    // Key to represent a specific seat, e.g., "ECONOMY:C167"
    public static string GetSeatField(ClassType classType, string seatId)
        => $"{classType}:{seatId}";
}