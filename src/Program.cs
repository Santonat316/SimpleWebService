using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using MyWebApplication.Data;
using Serilog;

namespace MyWebApplication
{
    public class Program
    {
        /// <summary>
        ///     The <see cref="IConfiguration" /> for the service.
        ///     Constructed from multiple json files and environment variables.
        /// </summary>
        public static IConfiguration Configuration { get; } = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", false, true)
            .AddJsonFile($"appsettings.{Environment()}.json", true, true)
            .AddEnvironmentVariables()
            .Build();

        /// <summary>
        ///     Builds the <see cref="IWebHost" /> to host the service.
        /// </summary>
        /// <param name="args">Arguments for building the host.</param>
        /// <returns>An <see cref="IWebHost" /> instance to host the web service.</returns>
        public static IWebHost BuildWebHost(string[] args)
        {
            return WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .UseSerilog()
                .ConfigureKestrel(c => c.AddServerHeader = false)
                .UseUrls(Configuration.GetSection("Kestrel:Urls").Get<List<string>>().ToArray())
                .Build();
        }

        /// <summary>
        ///     The entry point for the web service. This is were code execution starts.
        /// </summary>
        /// <param name="args"></param>
        public static void Main(string[] args)
        {
            Log.Logger = CreateStaticLogger();
            try
            {
                var host = BuildWebHost(args);
                SeedData();
                host.Run();
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Host terminated unexpectedly");
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        private static void SeedData()
        {
            NameRepository.NameCollection.Add("id1");
            NameRepository.NameCollection.Add("id2");
        }


        private static ILogger CreateStaticLogger()
        {
            var logger = new LoggerConfiguration()
                .ReadFrom
                .Configuration(Configuration);

            return logger.CreateLogger();
        }

        private static string Environment()
        {
            return System.Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")
                   ?? EnvironmentName.Production;
        }
    }
}