using Dal.Auth.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
namespace Dal.Auth;
public static class DalAuthExtension
{
    public static IServiceCollection AddDalAuthService(this IServiceCollection services)
    {
        IConfiguration config = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .Build();
        services.AddSingleton(config);
        services.AddDbContext<AuthContext>(options =>
        options.UseSqlServer(config["ConnectionStrings:StudentConnections"]));
        return services;
    }
}