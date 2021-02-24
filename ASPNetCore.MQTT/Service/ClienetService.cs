using ASPNetCore.MQTT.Model;
using MQTTnet.Protocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ASPNetCore.MQTT.Service
{
	public class ClienetService
	{
		private readonly CustomerMqttService mqttService;
		private readonly List<ClientInfo> clientList = new List<ClientInfo>();
		private static ReaderWriterLock _readWriteLock = new ReaderWriterLock();

		public ClienetService(CustomerMqttService mqttService)
		{
			this.mqttService = mqttService;
		}

		public void AddClient(Guid clientId, string clientName)
		{
			clientList.Add(new ClientInfo
			{
				Id = new Guid(),
				ClientId = clientId,
				ClientName = clientName,
				Lock = false,
				Status = Status.OFF
			});
		}

		public void RemoveClient(Guid clientId)
		{
			ClientInfo client = clientList.FirstOrDefault(x => x.ClientId == clientId);
			if (client != null)
			{
				clientList.Remove(client);
			}
		}

		public async Task PowerOn(Guid clientId)
		{
			await mqttService.PublishAsync(new AddMessage
			{
				Topic = clientId.ToString(),
				Payload = "power_on"
			});
		}

		public async Task PowerOff(Guid clientId)
		{
			await mqttService.PublishAsync(new AddMessage
			{
				Topic = clientId.ToString(),
				Payload = "power_off",
				MqttQualityOfServiceLevel = MqttQualityOfServiceLevel.AtMostOnce
			});
		}

		public Guid? GetClientId(Guid id)
		{
			return clientList.FirstOrDefault(x => x.Id == id)?.ClientId;
		}
	}
}
