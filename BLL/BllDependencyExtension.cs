using BLL.MappingProfiles;
using BLL.Services.HobbieService;
using BLL.Services.StudentBookService;
using BLL.Services.StudentDormitoryService;
using BLL.Services.StudentsDetailsService;
using BLL.Services.StudentService;
using BLL.Services.SubjectService;
using BLL.Services.TeacherService;
using DAL.Repositories.AuthRepository;
using DAL.Repository.Implementation;
using DAL.Repository.StudentSortingRepository;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PasswordHasherAuth;
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
        services.AddMemoryCache();
        services.AddScoped<IHobbieService, HobbieService>();
        services.AddScoped<IStudentService, StudentService>();
        services.AddScoped<IStudentBookDetails, StudentBookDetail>();
        services.AddScoped<ITeacherService, TeacherService>();
        services.AddScoped<ISubjectService, SubjectService>();
        services.AddScoped<IStudentsDetailsService, StudentsDetailsService>();

        services.AddScoped<IPasswordHasher, PasswordHasher>();
       

        services.AddScoped<IStudentDormitoryService, StudentDormitoryService>();
        services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
        services.AddScoped <IStudentSortingRepository, StudentSortingRepository>();
        services.AddScoped<IUserRepository, UserRepository>();

        Log.Logger = logConfiguration.CreateLogger();
        services.AddAutoMapper(typeof(StudentApiProfile));
        services.AddLogging(loggingbuilder => loggingbuilder.AddSerilog());
        return services;
    }
}
