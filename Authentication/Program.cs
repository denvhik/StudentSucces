using BllAuth;
using AwsS3Service;

namespace AuthenticationWebApi;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
     
    
        builder.Services.AddControllers();
        builder.Services.AddBllAuthService();
        builder.Services.AwsService();
        builder.Services.ConfigureServices(builder.Configuration);

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();
        app.UseAuthentication();
        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}
