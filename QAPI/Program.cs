using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Orleans;
using Orleans.Configuration;

namespace QAPI
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var client = new ClientBuilder()
                .Configure<ClusterOptions>(options =>
                {
                    options.ClusterId = "dev";
                    options.ServiceId = "QLearningOrleans";
                })
                .UseLocalhostClustering()
                .ConfigureLogging(logging => logging.AddConsole())
                .Build();

            await client.Connect(async ex =>
            {
                await Task.Delay(TimeSpan.FromSeconds(2));
                return true;
            });

            CreateWebHostBuilder(args)
                .ConfigureServices(s => s.AddSingleton(client))
                .Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();
    }
}
