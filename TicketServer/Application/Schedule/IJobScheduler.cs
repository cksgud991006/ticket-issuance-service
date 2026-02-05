namespace TicketServer.Schedule;

public interface IJobScheduler
{
    Task<int> GetWaitingPositionAsync(Guid id);
    Task ScheduleAsync(Guid id, DateTimeOffset scheduleTime);
}