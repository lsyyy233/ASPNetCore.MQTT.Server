using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using ASPNetCore.MQTT.Service;
using Microsoft.AspNetCore.Http;

namespace ASPNetCore.MQTT.Controllers
{
	[ApiController]
	[Route("api/v1/{controller}")]

	public class RemoteManagerController : ControllerBase
	{
		private readonly ILogger log;
		//private readonly CustomerMqttService _customerMqttService;
		private readonly ClienetService clientService;

		public RemoteManagerController(ILoggerFactory loggerFactory, CustomerMqttService customerMqttService, ClienetService clientService)
		{
			this.clientService = clientService;
			log = loggerFactory.CreateLogger<RemoteManagerController>();
			//this._customerMqttService = customerMqttService;
		}

		[HttpPost]
		public async Task<object> PushMessageAsync(AddMessage model)

		{
			return Ok();
		}

		public async Task<object> GetClientStatusAsync()
		{
			
		}

	}
}
