using ASPNetCore.MQTT.Service;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json.Serialization;

namespace ASPNetCore.MQTT
{
	public class Startup
	{
		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;
		}

		public IConfiguration Configuration { get; }
		public void ConfigureServices(IServiceCollection services)
		{
			services.AddControllers().AddNewtonsoftJson(option =>
			{
				option.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
			}); ;
			services
				.AddRouting(x =>
				{
					x.LowercaseQueryStrings = true;
					x.LowercaseUrls = true;
				});
			services.AddLogging();
			services.AddHostedService<MqttHostService>();
			services.AddSingleton<CustomerMqttService>();
			services.AddSingleton<ClienetService>();


		}

		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}
			else
			{
				app.UseExceptionHandler(appBuilder =>
				{
					appBuilder.Run(async context =>
					{
						context.Response.StatusCode = 500;
						await context.Response.WriteAsync("Unexpected Error");
					});
				});
			}
			app.UseRouting(); ;
			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllers();
			});
		}
	}
}
