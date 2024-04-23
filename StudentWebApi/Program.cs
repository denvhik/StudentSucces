using BLL;
using DAL;
using Serilog;
using StudentWebApi.ErrorHanldeMiddleware;
using StudentWebApi.ErrorHanldeMiddleware.ErrorDetailsModel;

namespace StudentWebApi;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddDalService();   
        builder.Services.AddBllService();
        builder.Services.AddMemoryCache();
        builder.Services.AddLogging();
        builder.Services.AddExceptionHandler<GlobalExtensionHandler>();
        builder.Services.AddProblemDetails();
        builder.Services.AddControllers();
      
        Log.Logger = new LoggerConfiguration()
            .ReadFrom.Configuration(builder.Configuration).CreateLogger();
        builder.Host.UseSerilog();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }
        //app.UseMiddleware<ExceptionMiddlewareExtension>();
        //app.UseMiddleware<CustomErrorHandlingMiddleware>();
        app.UseExceptionHandler();
        app.UseHttpsRedirection();
        app.UseAuthorization();
        app.MapControllers();
        app.Run();
    }
}
