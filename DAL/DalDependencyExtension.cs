using DAL.Models;
using DAL.Repositories.AuthRepository;
using DAL.Repository.BookDetails;
using DAL.Repository.StudentSortingRepository;
using DAL.StoredProcedures;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
namespace DAL;
public static class DalDependencyExtension
{
    public static IServiceCollection AddDalService(this IServiceCollection service) 
    {
        IConfiguration config = new ConfigurationBuilder()
          .SetBasePath(Directory.GetCurrentDirectory())
          .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
          .Build();
        service.AddSingleton(config);
        service.AddScoped<ICallStoredProcedureRepository, CallStoredProcedureRepository>();
        service.AddScoped<IStudentBookDetailRepositorys, StudentBookDetailsRepository>();
        service.AddScoped<IStudentSortingRepository, StudentSortingRepository>();
        service.AddScoped<IUserRepository, UserRepository>();
        service.AddDbContext<StudentSuccesContext>(options =>
      options.UseSqlServer(config["ConnectionStrings:StudentConnections"]));
        return service;
    }
}
