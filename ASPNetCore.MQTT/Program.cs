using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NLog;
using NLog.Extensions.Logging;

namespace ASPNetCore.MQTT
{
	public class Program
	{
		public static void Main(string[] args)
		{
			CreateHostBuilder(args).Build().Run();
		}

		public static IHostBuilder CreateHostBuilder(string[] args) =>
			Host.CreateDefaultBuilder(args)
				.ConfigureLogging((context, logging) =>
				{
					LogManager.Configuration = new NLogLoggingConfiguration(context.Configuration.GetSection("NLog"));
					logging
						.ClearProviders()
						.AddNLog(context.Configuration)
						.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Trace);
				})
				.ConfigureWebHostDefaults(webBuilder =>
				{
					IConfiguration Configuration = new ConfigurationBuilder()
						.Add(new JsonConfigurationSource
						{
							Path = "appsettings.json",
							Optional = false,
							ReloadOnChange = true
						})
						.Build();
					webBuilder
						.UseUrls(Configuration.GetValue<string>("urls"))
						.UseStartup<Startup>();
				});
	}
}
