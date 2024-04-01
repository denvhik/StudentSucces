//using DAL.Models;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.Extensions.Configuration;
//using Microsoft.Extensions.DependencyInjection;

//namespace DAL
//{
//    public class Program
//    {
//        public Program(IConfiguration configuration)
//        {
//            Configuration = configuration;
//        }

//        public IConfiguration Configuration { get; }

//        public void ConfigureServices(IServiceCollection services)
//        {
           
//            services.AddDbContext<StudentSuccesContext>(options =>
//                options.UseSqlServer(Configuration.GetConnectionString("StudentConnections")));
//        }

     
//    }
//}
