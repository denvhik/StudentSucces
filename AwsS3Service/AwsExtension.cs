using Amazon.S3;
using Microsoft.Extensions.DependencyInjection;

namespace AwsS3Service;

public  static class AwsExtension
{
    public static IServiceCollection AwsService(this IServiceCollection service)
    {
        service.AddAWSService<IAmazonS3>();
        service.AddScoped<IS3Service, S3Service>();
        return service;
    }
}
