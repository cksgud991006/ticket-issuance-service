namespace TicketServer.Schedule;

public interface IJobScheduler
{
    Task ScheduleAsync(Guid id, DateTimeOffset scheduleTime);
}