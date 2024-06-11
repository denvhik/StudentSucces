using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;
using Amazon.Lambda.Core;
using Amazon.Lambda.SQSEvents;
using Amazon.SimpleNotificationService;
using Amazon.SimpleNotificationService.Model;
using Amazon.SQS;
using Amazon.SQS.Model;
using Microsoft.Extensions.Configuration;
using System.Text.Json.Nodes;


// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace DumpStudentDataLambda;

public class Function
{

    private readonly IConfiguration _configuration;
    private readonly IAmazonDynamoDB _dynamoDbClient;
    private readonly IAmazonSQS _sqsClient;
    private readonly IAmazonSimpleNotificationService _snsClient;
    /// <summary>
    /// Default constructor. This constructor is used by Lambda to construct the instance. When invoked in a Lambda environment
    /// the AWS credentials will come from the IAM role associated with the function and the AWS region will be set to the
    /// region the Lambda function is executed in.
    /// </summary>
    /// 
    public Function() : this(new ConfigurationBuilder()
     .SetBasePath(Directory.GetCurrentDirectory())
     .AddJsonFile("aws-lambda-tools-defaults.json")
     .Build())
    {
    }

    public Function(IConfiguration configuration) : this(new AmazonDynamoDBClient(), new AmazonSQSClient(), new AmazonSimpleNotificationServiceClient(),configuration)
    {
    }
    public Function(IAmazonDynamoDB dynamoDbClient, IAmazonSQS sqsClient, IAmazonSimpleNotificationService snsClient, IConfiguration configuration)
    {
        _dynamoDbClient = dynamoDbClient;
        _sqsClient = sqsClient;
        _snsClient = snsClient;
        _configuration = configuration;
    }

    /// <summary>
    /// This method is called for every Lambda invocation. This method takes in an SQS event object and can be used 
    /// to respond to SQS messages.
    /// </summary>
    /// <param name="evnt">The event for the Lambda function handler to process.</param>
    /// <param name="context">The ILambdaContext that provides methods for logging and describing the Lambda environment.</param>
    /// <returns></returns>
    public async Task FunctionHandler(SQSEvent evnt, ILambdaContext context)
    {
        foreach (SQSEvent.SQSMessage message in evnt.Records)
        {
            try
            {
                await ProcessMessageAsync(message, context);
                await DeleteMessageAsync(message, context);
            }
            catch (Exception ex)
            {
                context.Logger.LogError($"Error processing message: {ex.Message}");
                await SendMessageToDeadLetterQueue(message, context);
            }
        }
    }

    private async Task DeleteMessageAsync(SQSEvent.SQSMessage message, ILambdaContext context)
    {
        var queueUrl = _configuration.GetSection("SQSurl").Value;
        var deleteMessageRequest = new DeleteMessageRequest
        {
            QueueUrl = queueUrl,
            ReceiptHandle = message.ReceiptHandle
        };
        await _sqsClient.DeleteMessageAsync(deleteMessageRequest);
    }

    private async Task SendMessageToDeadLetterQueue(SQSEvent.SQSMessage message, ILambdaContext context)
    {
        var DeadLetterQueueUrl = _configuration.GetSection("SQSurlDLQ").Value;
        var sendMessageRequest = new SendMessageRequest
        {
            QueueUrl = DeadLetterQueueUrl,
            MessageBody = message.Body
        };
        await _sqsClient.SendMessageAsync(sendMessageRequest);
    }

    private async Task ProcessMessageAsync(SQSEvent.SQSMessage message, ILambdaContext context)
    {
        context.Logger.LogInformation($"Processed message {message.Body}");
        var tableName = _configuration.GetSection("DynamoDBTableName").Value;
        var snsTopicArn = _configuration.GetSection("SNSTopicARN").Value;
        var jsonObject = JsonNode.Parse(message.Body);

        var executorId = jsonObject["ExecutorId"].ToString();
        var currentDateTime = jsonObject["CurrentDateTime"].ToString();
        var actionName = jsonObject["ActionName"].ToString();
        var entityId = jsonObject["EntityId"].ToString();
        var body = jsonObject["Body"].ToString();

        var putItemRequest = new PutItemRequest
        {
            TableName = tableName,
            Item = new Dictionary<string, AttributeValue>
            {
                { "ExecutorId", new AttributeValue { S = executorId } },
                { "CurrentDateTime", new AttributeValue { S = currentDateTime } },
                { "ActionName", new AttributeValue { S = actionName } },
                { "EntityId", new AttributeValue { N = entityId } },
                { "Body", new AttributeValue { S = body } }
            }
        };

        var response = await _dynamoDbClient.PutItemAsync(putItemRequest);

        if (response.HttpStatusCode == System.Net.HttpStatusCode.OK)
        {
            context.Logger.LogInformation($"Data saved to DynamoDB successfully.{response.HttpStatusCode}");

       
            var publishRequest = new PublishRequest
            {
                TopicArn = snsTopicArn,
                Message = $"Message processed and saved to DynamoDB: {message.Body}"
            };
            await _snsClient.PublishAsync(publishRequest);
        }
        else
        {
            context.Logger.LogError("Failed to save data to DynamoDB.");
        }

    }
}


