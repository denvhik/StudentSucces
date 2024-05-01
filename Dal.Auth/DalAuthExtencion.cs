using Dal.Auth.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
namespace DAL;
public static class DalAuthExtension
{

    public static IServiceCollection AddDalAuthService(this IServiceCollection services)
    {
       
        IConfiguration config = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .Build();

        services.AddSingleton(config);
        services.AddDbContext<AuthDbContext>(options =>
       options.UseSqlServer(config["ConnectionStrings:StudentConnections"]));
        services.AddScoped<DbContext, AuthDbContext>();
        return services;
    }
}