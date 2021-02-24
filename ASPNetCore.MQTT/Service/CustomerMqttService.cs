using Microsoft.Extensions.Logging;
using MQTTnet;
using MQTTnet.Server;
using System.Text;
using System.Threading.Tasks;

namespace ASPNetCore.MQTT
{
	public class CustomerMqttService
	{
		private static IMqttServer mqttServer;
		private readonly ILogger log;

		public CustomerMqttService(ILoggerFactory loggerFactory)
		{
			log = loggerFactory.CreateLogger<CustomerMqttService>();
		}

		public IMqttServer GetMqttServer()
		{
			if (mqttServer == null)
			{
				mqttServer = new MqttFactory().CreateMqttServer();
			}
			return mqttServer;
		}

		public async Task PublishAsync(AddMessage model)
		{
			var message = new MqttApplicationMessage()
			{
				Topic = model.Topic,
				Payload = Encoding.UTF8.GetBytes(model.Payload),
				QualityOfServiceLevel = model.MqttQualityOfServiceLevel
			};
			await mqttServer.PublishAsync(message);
			log.LogInformation($"MQTT Broker pushed topic [{model.Topic}] success!");
		}
	}
}
