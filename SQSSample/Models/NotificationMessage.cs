
namespace SQSSample.Models;

public class NotificationMessage
{
    public DateTime CurrentDateTime { get; set; }
    public string ExecutorId { get; set; }
    public string ActionName { get; set; }
    public int EntityId { get; set; }
    public object Body { get; set; }
}
