using MQTTnet.Protocol;

namespace ASPNetCore.MQTT
{
	public class AddMessage
	{
		public string Topic { get; set; }

		public string Payload { get; set; }

		public MqttQualityOfServiceLevel MqttQualityOfServiceLevel { get; set; }
	}
}
