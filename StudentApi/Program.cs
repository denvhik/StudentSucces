using ADO_NET.ViewService;
using ADONET.CallReportingService;
using ADONET.ViewService;
using BLL.Services.StudentService;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
namespace StudentApi;
public class Program
{
    static  async Task Main()
    {
        
        ServiceCollection serviceCollection = new ();
        Startup startup = new (new ConfigurationBuilder().Build());
        startup.ConfigureServices(serviceCollection);
        ServiceProvider serviceProvider = serviceCollection.BuildServiceProvider();
        IStudentService studentService = serviceProvider.GetRequiredService<IStudentService>();
        ICallViewService callView = serviceProvider.GetRequiredService<ICallViewService>();
        IDbSetView dbSetView = serviceProvider.GetService<IDbSetView>();
        MainMenu menuService = new (studentService,callView, dbSetView);
        await menuService.ShowMenu();

    }
}
