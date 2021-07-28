using System.IO;
using System.Net;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Gevlee.RestTunes
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var webHostBuilder = BuildWebHost(args);
            if (!File.Exists("db.sqlite"))
            {
                var log = webHostBuilder.Services.GetRequiredService<ILogger<Program>>();
                log.LogInformation("Database not exists. Downloading...");
                new WebClient().DownloadFile(
                    "https://github.com/lerocha/chinook-database/raw/434f13993d7dc33e37271d082fe9eff379ea7abb/ChinookDatabase/DataSources/Chinook_Sqlite_AutoIncrementPKs.sqlite",
                    "db.sqlite");
                log.LogInformation("Database is ready. Running server...");
            }
            webHostBuilder.Run();
        }

        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .Build();
    }
}
