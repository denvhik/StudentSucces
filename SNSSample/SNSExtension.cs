using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SNSSample.SNSservice;
namespace SNSSample;

public static class SNSExtension
{
    public static IServiceCollection AddSNSExtension(this IServiceCollection services) 
    {
       
       IConfiguration configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory()) 
            .AddJsonFile("appsettings.Json", optional: false, reloadOnChange: true) 
            .Build();

        services.AddScoped<ISNSService, SNSService>();
        return services;
    }
}
