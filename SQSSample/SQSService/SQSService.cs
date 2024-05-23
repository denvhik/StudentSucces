using Amazon.Runtime;
using Amazon.SQS;
using Amazon.SQS.Model;
using Microsoft.Extensions.Configuration;
using SQSSample.Models;
using System.Text.Json;

namespace SQSSample.SQSServices;
public class SQSService : ISQSService
{
    private readonly IConfiguration _configuration;
    public SQSService( IConfiguration configuration)
    {
       
        _configuration = configuration;
    }
    /// <summary>
    /// Sends a notification message to the specified AWS SQS queue.
    /// </summary>
    /// <param name="message">The notification message to be sent. Must not be null.</param>
    /// <returns>
    /// A task that represents the asynchronous send operation.
    /// The task result contains a <see cref="SendMessageResponse"/> object which contains the response from SQS.
    /// </returns>
    public async Task<SendMessageResponse> SendMessageAsync(NotificationMessage message)
    {
        try
        {
            var awsAccessKey = _configuration.GetSection("AWSSQS:AccessKey").Value;
            var awsSecretAccessKey = _configuration.GetSection("AWSSQS:SecretAccessKey").Value;
            var cred = new BasicAWSCredentials(awsAccessKey, awsSecretAccessKey);
            var client = new AmazonSQSClient(cred, Amazon.RegionEndpoint.EUNorth1);
            var queueUrl = _configuration.GetSection("AWSSQS:SQSurl").Value;
            var sendMessageRequest = new SendMessageRequest
            {
                QueueUrl = "https://sqs.eu-north-1.amazonaws.com/339712805014/SBQueue-Main",
                MessageBody = JsonSerializer.Serialize(message)
            };

            var response = await client.SendMessageAsync(sendMessageRequest);
            return response;
        }
        catch (AmazonSQSException ex)
        {
            throw new AmazonSQSException($"Failed to sent message to aws {ex.ErrorType},{ex.Message}");
        }
    }
}
