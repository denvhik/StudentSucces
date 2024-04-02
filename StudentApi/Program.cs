using ADO_NET.ViewService;
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
        CallView callView = serviceProvider.GetRequiredService<CallView>();
        MainMenu menuService = new (studentService,callView);
        await menuService.ShowMenu();

    }
}
