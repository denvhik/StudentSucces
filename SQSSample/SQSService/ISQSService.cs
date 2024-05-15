using Amazon.SQS.Model;
using StudentWebApi.Models;

namespace SQSSample.SQSService
{
    public interface ISQSService
    {
        Task<SendMessageResponse> SendMessageAsync(NotificationMessage message);
    }
}