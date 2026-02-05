namespace TicketServer.Domain.Status;
public class QueueStatus
{
    public bool IsInQueue { get; private set; }
    public int Position { get; private set; }

    private QueueStatus(bool isInQueue, int position)
    {
        IsInQueue = isInQueue;
        Position = position;
    }

    public static QueueStatus InQueue(int position) =>
        new QueueStatus(true, position);    

    public static QueueStatus NotInQueue() =>
        new QueueStatus(false, -1);
}