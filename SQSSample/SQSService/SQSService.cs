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

    public async Task<SendMessageResponse> SendMessageAsync(NotificationMessage message)
    {
        var awsAccessKey = _configuration.GetSection("AWSSQS:AccessKey").Value;
        var awsSecretAccessKey = _configuration.GetSection("AWSSQS:SecretAccessKey").Value;
        var cred = new BasicAWSCredentials(awsAccessKey, awsSecretAccessKey);
        var client = new AmazonSQSClient(cred, Amazon.RegionEndpoint.EUNorth1);
        var queueUrl = _configuration.GetSection("AWSSQS:SQSurl").Value;
        var sendMessageRequest = new SendMessageRequest
        {
            QueueUrl = queueUrl,
            MessageBody = JsonSerializer.Serialize(message)
        };

        var response =  await client.SendMessageAsync(sendMessageRequest);
        return response;
    }
}
