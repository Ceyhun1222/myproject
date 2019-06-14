﻿using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Serilog;
using System.IO;

namespace ObstacleChecker.API
{
    public class Program
    {
        private static string _environmentName;

        public static void Main(string[] args)
        {
            var webHost =CreateWebHostBuilder(args).Build();

            var configuartion = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .AddJsonFile($"appsettings.{_environmentName}.json", optional: true, reloadOnChange: true)
                .Build();

            Log.Logger = new LoggerConfiguration()
                //.ReadFrom.Configuration(configuartion)
                .WriteTo.RollingFile("d:\\logtest\\log-{Date}.txt", fileSizeLimitBytes: null)
                .CreateLogger();

            webHost.Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .ConfigureLogging((hostingContext, config) =>
                {
                    config.ClearProviders();
                    _environmentName = hostingContext.HostingEnvironment.EnvironmentName;
                })
                .UseStartup<Startup>();
    }
}
