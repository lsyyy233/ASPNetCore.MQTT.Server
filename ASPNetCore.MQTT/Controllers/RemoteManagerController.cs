using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace ASPNetCore.MQTT.Controllers
{
	[ApiController]
	[Route("api/v1/{controller}")]

	public class RemoteManagerController : ControllerBase
	{
		private readonly ILogger log;
		private readonly MqttService mqttService;

		public RemoteManagerController(ILoggerFactory loggerFactory, MqttService mqttService)
		{
			log = loggerFactory.CreateLogger<RemoteManagerController>();
			this.mqttService = mqttService;
		}

		[HttpPost]
		public async Task<object> PushMessageAsync(AddMessage model)

		{
			await mqttService.PublishAsync(model);
			return Ok();
		}
	}
}
