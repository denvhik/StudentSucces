using Amazon.SQS;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SQSSample.SQSServices;

namespace SQSSample;

public static class SQSExtension
{
    public static IServiceCollection AddSQSExtension(this IServiceCollection services)
    {

        IConfiguration configuration = new ConfigurationBuilder()
             .SetBasePath(Directory.GetCurrentDirectory())
             .AddJsonFile("appsettings.Json", optional: false, reloadOnChange: true)
             .Build();
        services.AddSingleton(configuration);
        services.AddScoped<ISQSService, SQSService>();
        return services;
    }
}
