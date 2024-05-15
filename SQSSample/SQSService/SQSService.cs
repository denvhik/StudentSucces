using Amazon.SQS;
using Amazon.SQS.Model;
using Microsoft.Extensions.Configuration;
using StudentWebApi.Models;
using System.Text.Json;


namespace SQSSample.SQSService
{
    public class SQSService : ISQSService
    {
        private readonly IAmazonSQS _sqsClient;
        private readonly string _queueUrl;

        public SQSService(IAmazonSQS sqsClient, IConfiguration configuration)
        {
            _sqsClient = sqsClient;
            _queueUrl = configuration["SQS:QueueUrl"];
        }

        public async Task<SendMessageResponse> SendMessageAsync(NotificationMessage message)
        {
            var sendMessageRequest = new SendMessageRequest
            {
                QueueUrl = _queueUrl,
                MessageBody = JsonSerializer.Serialize(message)
            };

            return await _sqsClient.SendMessageAsync(sendMessageRequest);
        }
    }
}
