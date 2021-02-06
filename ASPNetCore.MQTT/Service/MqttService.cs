using Microsoft.Extensions.Logging;
using MQTTnet;
using MQTTnet.Server;
using System.Text;
using System.Threading.Tasks;
using MQTTnet.Protocol;

namespace ASPNetCore.MQTT
{
	public class MqttService
	{
		private static IMqttServer mqttServer;
		private readonly ILogger log;

		public MqttService(ILoggerFactory loggerFactory)
		{
			log = loggerFactory.CreateLogger<MqttService>();
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
			log.LogInformation($"MQTT Broker发布主题[{model.Topic}]成功！");
		}
	}
}
