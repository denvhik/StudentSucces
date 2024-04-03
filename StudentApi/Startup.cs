using ADO_NET.ViewService;
using ADONET.CallReportingService;
using ADONET.SimpleOperationsService;
using ADONET.ViewService;
using BLL.MappingProfiles;
using BLL.Services.HobbieService;
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
    public IConfiguration Configuration { get; }

    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public void ConfigureServices(IServiceCollection services)
    {
        IConfiguration config = new ConfigurationBuilder()
           .SetBasePath(Directory.GetCurrentDirectory())
           .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
           .Build();
       
        var logConfiguration = new LoggerConfiguration()
        .ReadFrom.Configuration(Configuration)
        .WriteTo.File("Logs/applog-.txt", rollingInterval: RollingInterval.Day);

        Log.Logger = logConfiguration.CreateLogger();
        services.AddDbContext<StudentSuccesContext>(options =>
        options.UseSqlServer(config["ConnectionStrings:StudentConnections"]));
        services.AddAutoMapper(typeof(StudentApiProfile));
        //-----------------------BLL---------------------------//
        services.AddScoped<IStudentService, StudentService>();
        services.AddScoped<IHobbieService, HobbieService>();
        


        //-----------------------DAL---------------------------//
        services.AddScoped<GenericRepository<Student>>();
        services.AddScoped<ICallStoredProcedureRepository,CallStoredProcedureRepository>();

        //-----------------------ADONET---------------------------//
        services.AddScoped<ICallViewService, CallViewService>();
        services.AddScoped<IReportingService, ReportingService>();
        services.AddScoped<IDbSetView, DbsetView>();

        services.AddSingleton<IConfiguration>(provider=>config);
        services.AddLogging(loggingbuilder => loggingbuilder.AddSerilog());
    }


}
