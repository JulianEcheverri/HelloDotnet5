using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace HelloDotnet5
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                // Configuring logging
                .ConfigureLogging((context, logging) =>
                {
                    // If is production environment
                    // Enabling logs will be shown in a json format
                    if (context.HostingEnvironment.IsProduction())
                    {
                        logging.ClearProviders(); // Remove the simple console
                        logging.AddJsonConsole(); // And then add the json console
                    }
                })
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
