using BLL;
using AdoNet;

namespace StudentMVC;
public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddControllersWithViews().AddJsonOptions(options => options.JsonSerializerOptions.PropertyNamingPolicy = null).AddRazorRuntimeCompilation();
        builder.Services.AddBllService();
        builder.Services.AddAdoServices();
        //builder.Services.AddAntiforgery(options => 
        //{
        //    options.HeaderName = "X-CSRF-TOKEN";
        //});

        var app = builder.Build();

        app.Use(async (context, next) => {
            string path = context.Request.Path;
            if (path.EndsWith(".css") || path.EndsWith(".js"))
            {
                TimeSpan maxAge = new TimeSpan(7, 0, 0, 0); context.Response.Headers.Append("Cache-Control", "max-age=" + maxAge.TotalSeconds.ToString("0"));
            }
            else
            {
                context.Response.Headers.Append("Cache-Control", "no-cache");
                context.Response.Headers.Append("Cache-Control", "private, no-store");
            }
            await next();
        });
        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Home/Error");
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
        }

        app.UseHttpsRedirection();
        app.UseStaticFiles();

        app.UseRouting();

        app.UseAuthorization();

        app.MapControllerRoute(
            name: "default",
            pattern: "{controller=Home}/{action=Index}/{id?}");
            
        app.Run();
    }
}
