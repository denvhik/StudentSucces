using Amazon.SQS.Model;
using SQSSample.Models;

namespace SQSSample.SQSServices;

public interface ISQSService
{
    Task<SendMessageResponse> SendMessageAsync(NotificationMessage message);
}