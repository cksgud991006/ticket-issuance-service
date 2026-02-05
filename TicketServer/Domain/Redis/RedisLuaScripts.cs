using StackExchange.Redis;
namespace TicketServer.Domain.Redis;
public static class RedisLuaScripts
{
    public static readonly LuaScript LoadSeatInventoryScript = LuaScript.Prepare(
        File.ReadAllText("Scripts/SeatInventory.lua"));
}