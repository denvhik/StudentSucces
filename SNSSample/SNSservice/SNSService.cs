using Amazon.Runtime;
using Amazon.SimpleNotificationService;
using Amazon.SimpleNotificationService.Model;
using Microsoft.Extensions.Configuration;
using System.Text.Json;

namespace SNSSample.SNSservice;
public class SNSService : ISNSService
{
    private readonly IConfiguration configuration;

    public SNSService(IConfiguration configuration)
    {
        this.configuration = configuration;
    }

    /// <summary>
    /// Sends a notification message to an AWS SNS topic.
    /// </summary>
    /// <param name="message">The message to be sent.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the response from the SNS Publish operation.</returns>
    /// <exception cref="AmazonSimpleNotificationServiceException">Thrown when there is an error communicating with AWS SNS.</exception>
    /// <exception cref="ArgumentNullException">Thrown when the message is null or empty.</exception>
    /// <exception cref="Exception">Thrown when an unexpected error occurs.</exception>
    /// <remarks>
    /// This method uses AWS SDK for .NET to send a message to an Amazon SNS topic. The AWS credentials and region are loaded from the configuration.
    /// The message is serialized to JSON format before being sent. If an error occurs while communicating with AWS SNS, an AmazonSimpleNotificationServiceException
    /// is thrown. Any other exceptions are rethrown as generic Exception.
    /// </remarks>
    public async Task<PublishResponse> NotificationSendAsync(string message)
    {
        if (string.IsNullOrEmpty(message))
        {
            throw new ArgumentNullException(nameof(message), "Message cannot be null or empty.");
        }
        try
        {
            var awsAccessKey = configuration.GetSection("AWS:AccessKey").Value;
            var awsSecretAccessKey = configuration.GetSection("AWS:SecretAccessKey").Value;
            var cred = new BasicAWSCredentials(awsAccessKey, awsSecretAccessKey);
            var client = new AmazonSimpleNotificationServiceClient(cred, Amazon.RegionEndpoint.EUNorth1);
            var request = new PublishRequest
            {
                TopicArn = "arn:aws:sns:eu-north-1:339712805014:DemoMessage",
                Message = JsonSerializer.Serialize(message),
                Subject = "New Notification From Library"
            };
            var response = await client.PublishAsync(request);
            return response;
        }
        catch (AmazonSimpleNotificationServiceException ex)
        {
            
            
            throw new AmazonSimpleNotificationServiceException(ex.Message);
        }
        catch (Exception ex)
        {
          
            throw new Exception (ex.Message);
        }
    }
}
