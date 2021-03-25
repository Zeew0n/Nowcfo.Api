using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Events;
using System;
using System.IO;

namespace Nowcfo.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var hostBuilder = CreateHostBuilder(args);
            var host = hostBuilder.Build();
            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                try
                {
                    var hostingEnvironment = services.GetRequiredService<IWebHostEnvironment>();
                    services.GetRequiredService<Microsoft.Extensions.Configuration.IConfiguration>();
                    string filePath;
                    if (hostingEnvironment.IsDevelopment())
                    {
                        string appDataDirectory = Path.Combine(hostingEnvironment.WebRootPath, "Logs");
                        if (!Directory.Exists(appDataDirectory))
                            Directory.CreateDirectory(appDataDirectory);

                        filePath = Path.Combine(appDataDirectory, "log_.txt");
                    }
                    else
                        filePath = Path.Combine($@"D:\", "log_.txt");

                    Log.Logger = new LoggerConfiguration()
                        .MinimumLevel.Debug()
                        .MinimumLevel.Override("Microsoft", LogEventLevel.Error)
                        .Enrich.FromLogContext()
                        .WriteTo.Debug()
                        .WriteTo.File(filePath, rollingInterval: RollingInterval.Day)
                        .CreateLogger();

                    host.Run();
                }
                catch (Exception ex)
                {
                    Log.Fatal(ex, "The Application failed to start.");
                }
                finally
                {
                    Log.CloseAndFlush();
                }
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder => { webBuilder.UseStartup<Startup>(); });
    }
}