using StackExchange.Redis;

namespace TicketServer.Schedule.Redis;

public static class RedisKeys
{
    public const string JobScheduleKey = "job:schedule";
}