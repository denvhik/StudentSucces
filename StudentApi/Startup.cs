using ADO_NET.ViewService;
using ADONET.ViewService;
using BLL.MappingProfiles;
using BLL.Services.StudentService;
using DAL.Models;
using DAL.Repository.Implementation;
using DAL.StoredProcedures;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace StudentApi;

public class Startup
{
    public IConfiguration _Configuration { get; }

    public Startup(IConfiguration configuration)
    {
        _Configuration = configuration;
    }

    public void ConfigureServices(IServiceCollection services)
    {
        IConfiguration config = new ConfigurationBuilder()
           .SetBasePath(Directory.GetCurrentDirectory())
           .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
           .Build();
       
        var logConfiguration = new LoggerConfiguration()
        .ReadFrom.Configuration(_Configuration)
        .WriteTo.File("Logs/applog-.txt", rollingInterval: RollingInterval.Day);

        Log.Logger = logConfiguration.CreateLogger();
        services.AddDbContext<StudentSuccesContext>(options =>
        options.UseSqlServer(config["ConnectionStrings:StudentConnections"]));
        services.AddAutoMapper(typeof(StudentApiProfile));
        services.AddScoped<IStudentService, StudentService>();
        services.AddScoped<GenericRepository<Student>>();
        services.AddScoped<ICallStoredProcedureRepository,CallStoredProcedureRepository>();
        services.AddScoped<ICallView, CallView>();
        services.AddSingleton<IConfiguration>(provider=>config);
        services.AddLogging(loggingbuilder => loggingbuilder.AddSerilog());
    }


}
