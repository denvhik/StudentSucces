using Microsoft.Extensions.DependencyInjection;
using Dal.Auth;
using BllAuth.Services;
namespace BllAuth;

public static class BllAuthDependencyExtension
{
    public static IServiceCollection AddBllAuthService(this IServiceCollection service)
    {

        service.AddDalAuthService();
        service.AddScoped<IAuthService, AuthService>();
        return service;
    }
}
