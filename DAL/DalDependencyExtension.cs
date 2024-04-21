using DAL.Models;
using DAL.Repository.BookDetails;
using DAL.Repository.Implementation;
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
        service.AddSingleton<IConfiguration>(config);
        service.AddScoped<ICallStoredProcedureRepository, CallStoredProcedureRepository>();
        service.AddScoped<IStudentBookDetailRepositorys, StudentBookDetailsRepository>();
        service.AddDbContext<StudentSuccesContext>(options =>
        options.UseSqlServer(config["ConnectionStrings:StudentConnections"]));
        return service;
    }
}
