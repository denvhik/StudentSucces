using BLL.MappingProfiles;
using BLL.Services.HobbieService;
using BLL.Services.StudentBookService;
using BLL.Services.StudentDormitoryService;
using BLL.Services.StudentService;
using BLL.Services.SubjectService;
using BLL.Services.TeacherService;
using DAL;
using DAL.Repository.Implementation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace BLL;
public static class BllDependencyExtension
{
    public static IServiceCollection AddBllService(this IServiceCollection services) 
    {
        IConfiguration config = new ConfigurationBuilder()
          .SetBasePath(Directory.GetCurrentDirectory())
          .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
          .Build();
        var logConfiguration = new LoggerConfiguration()
       .ReadFrom.Configuration(config)
       .WriteTo.File("Logs/applog-.txt", rollingInterval: RollingInterval.Day);
        services.AddScoped<IHobbieService, HobbieService>();
        services.AddScoped<IStudentService, StudentService>();
        services.AddScoped<IStudentBookDetails, StudentBookDetail>();
        services.AddScoped<ITeacherService, TeacherService>();
        services.AddScoped<ISubjectService, SubjectService>();
        services.AddScoped<IStudentDormitoryService, StudentDormitoryService>();
        services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
        Log.Logger = logConfiguration.CreateLogger();
        services.AddAutoMapper(typeof(StudentApiProfile));
        services.AddLogging(loggingbuilder => loggingbuilder.AddSerilog());
        services.AddDalService();
        return services;
    }
}
