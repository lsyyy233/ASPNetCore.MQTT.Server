using System;

namespace ASPNetCore.MQTT.Model
{
	public class ClientInfo
	{
		public Guid Id { get; set; }

		public Guid ClientId { get; set; }

		public string ClientName { get; set; }

		public bool Lock { get; set; } = false;

		public Status Status { get; set; } = Status.OFF;

		public Guid UserId { get; set; }
	}

	public enum Status
	{
		ON = 0,
		OFF = 1
	}
}
