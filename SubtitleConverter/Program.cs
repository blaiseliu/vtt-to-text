using System;
using System.IO;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using SubtitleConverter.Processes;
using SubtitleConverter.Services;

namespace SubtitleConverter
{
    class Program
    {
        static void Main(string[] args)
        {
            var builder = new ConfigurationBuilder();
            BuildConfig(builder);

            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(builder.Build())
                .Enrich.FromLogContext()
                .WriteTo.Console()
                .CreateLogger();

            var host = Host.CreateDefaultBuilder()
                .ConfigureServices((context, services) =>
                {
                    services.AddTransient<IConvertToTextProcess, ConvertToTextProcess>();
                    services.AddTransient<IConvertToTextServices, ConvertToTextServices>();
                    services.AddTransient<IConvertToSrtProcess, ConvertToSrtProcess>();
                    services.AddTransient<IConvertToSrtServices, ConvertToSrtServices>();
                })
                .UseSerilog()
                .Build();

            var svc = ActivatorUtilities.CreateInstance<Startup>(host.Services);
            svc.Run(args);
        }
        static void BuildConfig(IConfigurationBuilder builder)
        {
            builder.SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production"}.json", optional: true)
                .AddEnvironmentVariables();
        }
    }
}
