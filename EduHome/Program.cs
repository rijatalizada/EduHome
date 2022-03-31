using Microsoft.AspNetCore.Hosting; 
using Microsoft.Extensions.Configuration; 
using Microsoft.Extensions.Hosting; 
using Microsoft.Extensions.Logging; 
using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Threading.Tasks;
using EduHome;
using EduHome.DAL;
using Microsoft.AspNetCore.Identity;

namespace EduHome 
{ 
    public class Program 
    { 
        public async static Task Main(string[] args) 
        { 
            var host = CreateHostBuilder(args).Build();

            using (var scope = host.Services.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
                DataInitializer dataInitializer = new DataInitializer(dbContext, roleManager);
                await dataInitializer.SeedData();
            }
            
            
            await host.RunAsync(); 
        } 
 
        public static IHostBuilder CreateHostBuilder(string[] args) => 
            Host.CreateDefaultBuilder(args) 
                .ConfigureWebHostDefaults(webBuilder => 
                { 
                    webBuilder.UseStartup<Startup>(); 
                }); 
    } 
}
