namespace StudentWebApi.Models;
public class NotificationRequest
{
    public string ExecutorId { get; set; }
    public string ActionName { get; set; }
    public int EntityId { get; set; }
}
