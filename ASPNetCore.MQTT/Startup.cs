using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
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
			services.AddSingleton<MqttService>();


		}

		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}
			app.UseRouting(); ;
			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllers();
			});
		}
	}
}
