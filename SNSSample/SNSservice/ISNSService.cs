using Amazon.SimpleNotificationService.Model;

namespace SNSSample.SNSservice;
public interface ISNSService
{
    Task<PublishResponse> NotificationSendAsync(string message);
}
