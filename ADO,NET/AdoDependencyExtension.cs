using AdoNet.CallReportingService;
using AdoNet.SimpleOperationsService;
using AdoNet.ViewService;
using Microsoft.Extensions.DependencyInjection;
namespace AdoNet;
public static class AdoDependencyExtension
{
    public static IServiceCollection AddAdoServices( this IServiceCollection services) 
    {
        services.AddScoped<IReportingService,ReportingService>();
        services.AddScoped<ICallViewService, CallViewService>();
        services.AddScoped<IDbSetView,DbsetView>();
        return services;
    }
}
