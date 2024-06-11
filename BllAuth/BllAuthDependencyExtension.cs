using Microsoft.Extensions.DependencyInjection;
using Dal.Auth;
using Microsoft.Extensions.Configuration;
using BllAuth.Services.AuthService;
using BllAuth.Services.GenerateTokenService;
using BllAuth.Services.ImageUploadService;
using BllAuth.Services.EmailService;
using BllAuth.Services.LogOutService;

namespace BllAuth;

public static class BllAuthDependencyExtension
{
    public static IServiceCollection AddBllAuthService(this IServiceCollection service)
    {
        IConfiguration config = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .Build();
        service.AddDalAuthService();
        service.AddHttpContextAccessor();
        service.AddScoped<IPhotoService, PhotoService>();
        service.AddScoped<ILogoutService, LogoutService>();
        service.AddScoped<IEmailService, EmailService>();
        service.AddScoped<IAuthService, AuthService>();
        service.AddScoped<IGenerateTokenService, GenerateTokenService>();
        return service;
    }
}
